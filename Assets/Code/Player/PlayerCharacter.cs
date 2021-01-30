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
    [SerializeField] private PlayerCamera playerCamera;
    public PlayerCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }

    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI { get => playerUI; set => playerUI = value; }
    #endregion

    #region Values
    [SerializeField] private Vector3 currentDirection;
    public Vector3 CurrentDirection { get => currentDirection; set => currentDirection = value; }

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
        PlayerCamera.AssignFollowTarget(transform);
    }

    // Update is called once per frame
    void Update()
    {
        SteerInDirection(InputActions.Character.Move.ReadValue<Vector2>());
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

    #region Camera

    #endregion
    #region Character Methods

    public void SteerInDirection(Vector3 direction)
    {

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


}
