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
        SetUpPlayerInput();
        inputActions.Character.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //LookAtMouse();
        //SteerInDirection(deviation);
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

    private void SetUpPlayerInput()
    {
        if(inputActions != null)
        {
            inputActions.Character.Steer.performed += ctx => Steer_performed(ctx);
        }
        //if (character)
        //{
        //    // CharacterInput
        //    character.PlayerController = this;
        //    playerCharacter = character;

        //    #region Character Input
        //    inputActions.Character..started += playerCharacter.OnLeftPullTrigger;
        //    inputActions.Character.Left_PullTrigger.canceled += playerCharacter.OnLeftPullTrigger;
        //    inputActions.Character.Left_Reload.started += playerCharacter.OnLeftReload;

        //    inputActions.Character.Right_PullTrigger.started += playerCharacter.OnRightPullTrigger;
        //    inputActions.Character.Right_PullTrigger.canceled += playerCharacter.OnRightPullTrigger;
        //    inputActions.Character.Right_Reload.started += playerCharacter.OnRightReload;

        //    inputActions.Character.UseInteraction.started += playerCharacter.OnUseInteraction;

        //    inputActions.Character.Dash.started += playerCharacter.OnDashPullTrigger;
        //    inputActions.Character.Dash.canceled += playerCharacter.OnDashPullTrigger;

        //    inputActions.Character.Enable();
        //    #endregion
        //    playerCharacter.SetUpPlayerUI(PlayerUI);
        //}
    }

    private void Steer_performed(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {           
            Type inputType = callbackContext.valueType;
            Debug.Log("InputType: " + inputType.FullName);
            //if (inputType == typeof(Vector2))
            //{

            //}
            Vector2 dir = callbackContext.ReadValue<Vector2>();
            SteerInDirection(dir);
            Debug.Log("Steering: " + callbackContext.ReadValue<Vector2>());
            Debug.Log("Steering Normalized: " + callbackContext.ReadValue<Vector2>().normalized);
            //Screen.width
        }
    }

    private void OnPause(InputAction.CallbackContext callbackContext)
    {
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
    #endregion

    #region Character Methods
    public void SteerInDirection(Vector2 direction)
    {
        float dirX = direction.x;
        float halfScreen = Screen.width;

        Debug.Log("");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(direction.x,0.0f, 0.0f) , Time.deltaTime * moveSpeed);
    }
    public void SteerInDirection(Vector3 direction)
    {       
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed);
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
