using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PauseMenuPanel pauseMenuPanel;
    public PauseMenuPanel PauseMenuPanel { get => pauseMenuPanel; set => pauseMenuPanel = value; }
    
    [SerializeField] private TMP_Text soulsCollected_Text;
    public TMP_Text SoulsCollected_Text { get => soulsCollected_Text; set => soulsCollected_Text = value; }
    
    public void ChangeSoulsCollectedText(int value)
    {
        soulsCollected_Text.text = "Souls Collected : " + value;
    }
    public void OpenPauseMenu()
    {
        Debug.Log("[PAUSE]");
        pauseMenuPanel.Open();
        GameManager.Instance.GameMode.PauseGame();
    }

    public void ClosePauseMenu()
    {
        Debug.Log("[UNPAUSE]");
        GameManager.Instance.GameMode.ResumeGame();
        pauseMenuPanel.Close();
    }
}
