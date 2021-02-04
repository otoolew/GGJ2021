using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable] public enum GameState { RUNNING, PAUSED, LOADING, GAME_OVER }
public class GameMode : MonoBehaviour
{
    #region Variables
    [Header("Game State")]
    [SerializeField] private GameState currentGameState;
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }

    [Header("Player Components")]
    [SerializeField] private PlayerUI playerUI;
    public PlayerUI PlayerUI { get => playerUI; set => playerUI = value; }

    [SerializeField] private GameObject endScreen;
    [SerializeField] private Text endTime;
    [SerializeField] private Text endSouls;

    [SerializeField] private PlayerCharacter playerCharacter;
    public PlayerCharacter PlayerCharacter { get => playerCharacter; set => playerCharacter = value; }

    [SerializeField] private Transform playerSpawnPoint;
    public Transform PlayerSpawnPoint { get => playerSpawnPoint; set => playerSpawnPoint = value; }
    #endregion

    private bool restartLevel = false;

    private void Awake()
    {
        CurrentGameState = GameState.LOADING;
        InitGame();
    }
    // Start is called before the first frame update
    private void Start()
    {
        //GameManager.Instance.AssignGameMode(this);
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
            playerUI.PauseMenuPanel.RestartButton.onClick.AddListener(ResetGame);
            playerUI.PauseMenuPanel.QuitButton.onClick.AddListener(QuitGame);
        }
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Start Game!");
        CurrentGameState = GameState.RUNNING;

        if(restartLevel == true)
        {
            Debug.Log("Restarting level");
            GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
            endScreen.gameObject.SetActive(false);

        }
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
        CurrentGameState = GameState.GAME_OVER;

        //stop player
        var player = FindObjectOfType<PlayerCharacter>();
        player.DisableCharacterInput();

        //show game over panel
        endScreen.gameObject.SetActive(true);
        restartLevel = true;

        //show timer
        string endGameTime = Timer.instance.timePlayingStr;
        Timer.instance.EndTimer();
        endTime.text = endGameTime;

        //show souls
        endSouls.text = player.soulsCollected.ToString();
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.LoadScene("MainMenu");
    }
    #endregion
}
