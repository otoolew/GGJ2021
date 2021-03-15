using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//[Serializable] public enum PlayerStat { }
public class PlayerCharacter : MonoBehaviour
{
    public enum PlayerState
    {
        Playing,
        Done
    }

    public PlayerState currentState;

    #region Input
    [SerializeField] private PlayerControls inputActions;
    public PlayerControls InputActions { get => inputActions; set => inputActions = value; }
    #endregion

    #region Components
    [Header("Components")]
    // Connects to PlayerController Animator so it knows when to turn sprite.
    [SerializeField] private Animator playerAnimator;
    public Animator Animator { get => playerAnimator; set => playerAnimator = value; }

    [SerializeField] private CharacterController characterController;
    public CharacterController CharacterController { get => characterController; set => characterController = value; }

    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI { get => playerUI; set => playerUI = value; }

    #region VFX
    [SerializeField] private GameObject wakeEffect;
    public GameObject WakeEffect { get => wakeEffect; set => wakeEffect = value; }

    [SerializeField] private GameObject wakeSpray;
    public GameObject WakeSpray { get => wakeSpray; set => wakeSpray = value; }

    [SerializeField] private GameObject attackEffect;
    public GameObject AttackEffect { get => attackEffect; set => attackEffect = value; }


    [SerializeField] private GameObject activePlayer;
    public GameObject ActivePlayer { get => activePlayer; set => activePlayer = value; } 

    [SerializeField] private GameObject breakablePlayer;
    public GameObject BreakablePlayer { get => breakablePlayer; set => breakablePlayer = value; }

    [SerializeField] private float slowMotion = 0.2f;
    public float SlowMotionScale { get => slowMotion; set => slowMotion = value; }

    
    #endregion

    #region SFX
    [SerializeField] private AudioSource playerAttackSFX;
    public AudioSource PlayerAttackSFX { get => playerAttackSFX; set => playerAttackSFX = value; }

    [SerializeField] private AudioSource playerHitSFX;
    public AudioSource PlayerHitSFX { get => playerHitSFX; set => playerHitSFX = value; }

    [SerializeField] private AudioSource changeDirectionSFX;
    public AudioSource ChangeDirectionSFX { get => changeDirectionSFX; set => changeDirectionSFX = value; }

    [SerializeField] private AudioSource collectSoulSFX;
    public AudioSource CollectSoulSFX { get => collectSoulSFX; set => collectSoulSFX = value; }
    #endregion

    #endregion

    #region Values
    [Header("Values")]
    public static float CurrentPlayerSpeed;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField, Range(-1.0f, 1.0f)] private float deviation;
    public float Deviation { get => deviation; set => deviation = value; }

    [SerializeField] private float rotationSpeed;
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

    [SerializeField] private Vector3 currentDirection;
    public Vector3 CurrentDirection { get => currentDirection; set => currentDirection = value; }

    [SerializeField] private float jumpSpeed;
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }

    [SerializeField] private float gravity;
    public float Gravity { get => gravity; set => gravity = value; }

    [SerializeField] public int soulsCollected;
    public int SoulsCollected { get => soulsCollected; set => soulsCollected = value; }

    [SerializeField] private bool isAttacking;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    #endregion

    #region Mouse Values
    [SerializeField] private LayerMask layerMask;
    public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
    #endregion

    #region Monobehaviour 
    private void Awake()
    {
        inputActions = new PlayerControls();
        soulsCollected = 0;
        currentState = PlayerState.Playing;
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPause;

        inputActions.Character.Enable();
        inputActions.Character.Attack.performed += OnAttack;
    }

    // Start is called before the first frame update
    void Start()
    {
        soulsCollected = 0;
        if (playerUI == null)
        {
            playerUI = GetComponent<PlayerUI>();
        }
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState==PlayerState.Playing)
        {
            LookAtMouse();
            SteerInDirection(deviation);
        }
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

    private void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (!IsAttacking & currentState==PlayerState.Playing)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }
    IEnumerator AttackRoutine()
    {
        Debug.Log("Start Attack Routine");
        IsAttacking = true;
        if (playerAttackSFX.clip != null)
        {
            playerAttackSFX.Play();
        }

        if (playerAnimator)
        {
            playerAnimator.SetTrigger("Attack");
        }
        wakeEffect.SetActive(false);
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Stop Attack Routine");
        IsAttacking = false;
        wakeEffect.SetActive(true);
        attackEffect.SetActive(false);

        // Fix for bug where wakeEffect was being renabled when gameover while attacking.
        if (currentState == PlayerState.Done)
            wakeEffect.SetActive(false);
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

        float previousDeviation = deviation;
        deviation = Vector3.Dot(Vector3.right.normalized, playerToMouseDirection.normalized);
        ChangeDirection(previousDeviation, deviation);

        if(Quaternion.LookRotation(playerToMouseDirection).eulerAngles != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(playerToMouseDirection);
            if (lookRotation.eulerAngles != Vector3.zero)
            {
                lookRotation.x = 0f;
                lookRotation.z = 0f;
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, RotationSpeed * Time.deltaTime);
                playerAnimator.SetFloat("direction", lookRotation.w);
            }
        }
    }
    private void ChangeDirection(float previousDeviation, float updatedDeviation)
    {
        if((previousDeviation < 0 && updatedDeviation < 0) || (previousDeviation > 0 && updatedDeviation > 0))
        {

        }
        else
        {
            changeDirectionSFX.Play();
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

    public void SteerInDirection(float deviationValue)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(deviationValue, 0, 0), Time.deltaTime * moveSpeed);
    }

    public void Attack()
    {
        Debug.Log("[Player] Attack");
    }

    public void CollectSoul()
    {
        if (currentState == PlayerState.Playing)
        {
            soulsCollected += 1;

            if (collectSoulSFX.clip != null)
            {
                collectSoulSFX.Play();
            }

            playerUI.ChangeSoulsCollectedText(soulsCollected);
        }
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

    public void OnDeath()
    {
        if (currentState==PlayerState.Playing)
        {
            currentState = PlayerState.Done;

            // Stop wake effects
            if (wakeSpray != null)
                wakeSpray.SetActive(false);
            if (wakeEffect != null)
                wakeEffect.SetActive(false);

            // Add in breakable prefab
            if (breakablePlayer != null)
            {
                GameObject breakPlayer = Instantiate(breakablePlayer, transform.position, transform.rotation);
            }

            // Coroutine to deactive activePlayer and slow motion effect.
            StartCoroutine(WaitToShow());
        }
            
        
    }

    private IEnumerator WaitToShow()
    {
        activePlayer.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(SlowMotion());
    }

    private IEnumerator SlowMotion()
    {
        Time.timeScale = SlowMotionScale;
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = 1;
    }
}
