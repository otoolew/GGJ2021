using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuPanel : MenuPanel
{
    [SerializeField] private Button resumeButton;
    public Button ResumeButton { get => resumeButton; set => resumeButton = value; }

    [SerializeField] private Button restartButton;
    public Button RestartButton { get => restartButton; set => restartButton = value; }

    [SerializeField] private Button quitButton;
    public Button QuitButton { get => quitButton; set => quitButton = value; }

    public override void Open()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }

}
