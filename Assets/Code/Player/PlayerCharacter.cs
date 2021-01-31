using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    #region Input
    [SerializeField] private PlayerControls inputActions;
    public PlayerControls InputActions { get => inputActions; set => inputActions = value; }
    #endregion

    #region Components
    [SerializeField] private CapsuleCollider playerCollider;
    public CapsuleCollider PlayerCollider { get => playerCollider; set => playerCollider = value; }

    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI { get => playerUI; set => playerUI = value; }
    #endregion

    #region Values
    public static float CurrentPlayerSpeed;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField, Range(-1.0f, 1.0f)] private float deviation;
    public float Deviation { get => deviation; set => deviation = value; }

    [SerializeField] private float rotationSpeed;
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

    [SerializeField] private Vector3 currentDirection;
    public Vector3 CurrentDirection { get => currentDirection; set => currentDirection = value; }

    [SerializeField] private Vector3 moveVector = Vector3.zero;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private CharacterController characterController;

    #endregion
    #region Mouse Values

    [SerializeField] private LayerMask layerMask;
    public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
    #endregion
    #region Monobehaviour 
    private void Awake()
    {
        inputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPause;
        inputActions.Character.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerUI == null)
        {
            playerUI = GetComponent<PlayerUI>();
        }
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        SteerInDirection(deviation);
        PlayerJump();
    }

    private void OnDisable()
    {
        inputActions.UI.Pause.performed -= OnPause;
        inputActions.UI.Disable();
        inputActions.Character.Disable();
    }

    private void OnDestroy()
    {

    }

    #endregion
    #region PlayerInput Calls
    private void OnPause(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("OnPause");
        if (playerUI)
        {
            if (callbackContext.performed)
            {
                if (GameManager.Instance.GameMode.CurrentGameState != GameState.PAUSED)
                {
                    PlayerUI.OpenPauseMenu();
                }
                else
                {
                    PlayerUI.ClosePauseMenu();
                }
            }
        }
    }

    #endregion

    // Start is called before the first frame update
    #region Mouse
    public void LookAtMouse()
    {
        Vector3 playerToMouseDirection = MouseToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

        Vector3 forward = transform.TransformDirection(Vector3.up).normalized;
        if (Vector3.Dot(forward, playerToMouseDirection) < 0)
        {
            return;
        }

        //Vector3 offset = Vector3.right + new Vector3(13.59f, 0, 0);
        deviation = Vector3.Dot(Vector3.right.normalized, playerToMouseDirection.normalized);
        if(Quaternion.LookRotation(playerToMouseDirection).eulerAngles != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(playerToMouseDirection);
            if (lookRotation.eulerAngles != Vector3.zero)
            {
                lookRotation.x = 0f;
                lookRotation.z = 0f;
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, RotationSpeed * Time.deltaTime);
            }
        }
    }
    private Vector3 MouseToWorldPoint(Vector2 mouseScreen)
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseScreen);
        if (Physics.Raycast(ray, out RaycastHit rayHit, 100, layerMask))
        {
            return rayHit.point;
        }
        return transform.position;
    }
    private void PlayerJump()
    {
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            currentDirection.y = jumpSpeed;

        }

        currentDirection.y -= gravity * Time.deltaTime;
    }
    #endregion

    #region Character Methods
    //public void SteerInDirection(Vector2 direction)
    //{
    //    float dirX = direction.x;
    //    float halfScreen = Screen.width / 2;
    //    Debug.Log("dirX " + dirX);
    //    Debug.Log("Half Screen " + halfScreen);

    //    float normal = dirX / Screen.width;
    //    Debug.Log("Normal " + normal);
    //    Vector3 forward = transform.TransformDirection(Vector3.forward);
    //    Vector3 toOther = new Vector3(direction.x, 0, direction.y) - transform.position;
    //    print("Dot Product " + Vector3.Dot(forward, Vector3.forward));
    //    if (Vector3.Dot(forward, toOther) < 0)
    //    {
    //        print("The other transform is behind me!");
    //    }
    //    // get the touch position from the screen touch to world point
    //    Vector3 touchedPos = MouseToWorldPoint(direction);
    //    touchedPos.y = 0;
    //    touchedPos.z = 0;
    //    // lerp and set the position of the current object to that of the touch, but smoothly over time.
    //    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);

    //    //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x,0.0f, 0.0f) , Time.deltaTime * moveSpeed);
    //}
    public void SteerInDirection(Vector2 direction)
    {        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x, 0, 0), moveSpeed);
    }
    public void SteerInDirection(float direction)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction, 0, 0), Time.deltaTime * moveSpeed);
    }

    public void EnableCharacterInput()
    {
        inputActions.Character.Enable();
    }
    public void DisableCharacterInput()
    {
        inputActions.Character.Disable();
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, MouseToWorldPoint(Mouse.current.position.ReadValue()));
    }
}
