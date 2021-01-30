using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PauseMenuPanel pauseMenuPanel;
    public PauseMenuPanel PauseMenuPanel { get => pauseMenuPanel; set => pauseMenuPanel = value; }

    public void OpenPauseMenu()
    {
        pauseMenuPanel.Open();
        GameManager.Instance.GameMode.PauseGame();
    }

    public void ClosePauseMenu()
    {
        GameManager.Instance.GameMode.ResumeGame();
        pauseMenuPanel.Close();
    }
}
