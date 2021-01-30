using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable] public enum GameState { RUNNING, PAUSED, LOADING, GAME_OVER }
public class GameMode : MonoBehaviour
{
    #region Variables
    [Header("Game State")]
    [SerializeField] private GameState currentGameState;
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }

    [Header("Player Components")]
    [SerializeField] private PlayerCamera playerCamera;
    public PlayerCamera PlayerCamera { get => playerCamera; set => playerCamera = value; }

    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI { get => playerUI; set => playerUI = value; }

    [SerializeField] private PlayerCharacter playerCharacter;
    public PlayerCharacter PlayerCharacter { get => playerCharacter; set => playerCharacter = value; }

    [SerializeField] private Transform playerSpawnPoint;
    public Transform PlayerSpawnPoint { get => playerSpawnPoint; set => playerSpawnPoint = value; }
    #endregion

    private void Awake()
    {
        CurrentGameState = GameState.LOADING;
        GameManager.Instance.AssignGameMode(this);
        InitGame();
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    #region Level Win Lose Condition

    #endregion

    #region Level Enemies

    #endregion

    #region Game Time Start and Finish
    public void InitGame()
    {
        //Debug.Log("Init Game!");
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        if (playerCharacter == null)
        {
            Debug.Log("Player Character NULL");
        }
        playerCharacter.transform.position = playerSpawnPoint.transform.position;

        // Player UI
        playerUI = FindObjectOfType<PlayerUI>();
        if (playerUI == null)
        {
            Debug.Log("Player UI NULL");
        }
        else
        {
            playerUI.PauseMenuPanel.ResumeButton.onClick.AddListener(ResumeGame);
            playerUI.PauseMenuPanel.AbortMissionButton.onClick.AddListener(ResumeGame);
            playerUI.PauseMenuPanel.QuitButton.onClick.AddListener(ResumeGame);
        }

        // Camera 
        playerCamera = FindObjectOfType<PlayerCamera>();
        if (playerCamera == null)
        {      
            Debug.Log("Player Camera NULL");
        }

        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Start Game!");
        CurrentGameState = GameState.RUNNING;
    }

    public void PauseGame()
    {
        if (playerCharacter)
            playerCharacter.InputActions.Character.Disable();
        CurrentGameState = GameState.PAUSED;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        if (playerCharacter)
            playerCharacter.InputActions.Character.Enable();
        CurrentGameState = GameState.RUNNING;
        Time.timeScale = 1.0f;
    }

    public void ResetGame()
    {
        Debug.Log("Reset");
        Time.timeScale = 1.0f;
        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.LoadScene("MainMenu");
    }
    #endregion
}
