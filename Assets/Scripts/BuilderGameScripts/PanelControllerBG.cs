using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelControllerBG : MonoBehaviour
{
    public GameObject bgGameManager;
    public GameObject IntroductionPanel;
    public GameObject inGameCursor;
    public GameObject uiCursor;
    public GameObject endGamePanel;
    public void EnableGame(){
        bgGameManager.SetActive(true);
        IntroductionPanel.SetActive(false);
        inGameCursor.SetActive(true);
        uiCursor.SetActive(false);
    }
    public void GameOverBG(){
        bgGameManager.SetActive(false);
        inGameCursor.SetActive(false);
        endGamePanel.SetActive(true);
        uiCursor.SetActive(true);
    }
    public void MainMenusc(){
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart(){
        SceneManager.LoadScene("BuilderGame");
    }
}
