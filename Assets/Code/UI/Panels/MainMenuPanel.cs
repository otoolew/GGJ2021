using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MenuPanel
{
    #region Buttons
    [SerializeField] private Button startGameButton;
    public Button StartGameButton { get => startGameButton; set => startGameButton = value; }

    [SerializeField] private Button quitButton;
    public Button QuitButton { get => quitButton; set => quitButton = value; }
    #endregion

    [SerializeField] private string startSceneName;
    public string StartSceneName { get => startSceneName; set => startSceneName = value; }

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(StartButton_Clicked);
        quitButton.onClick.AddListener(QuitButton_Clicked);
    }

    public override void Open()
    {
       
    }

    public override void Close()
    {
    }
    public void StartButton_Clicked()
    {
        Debug.Log("Start Clicked");
        GameManager.Instance.LoadScene(startSceneName);
    }
    public void QuitButton_Clicked()
    {
        Debug.Log("Quit Selected!");
        Application.Quit();
    }

}
