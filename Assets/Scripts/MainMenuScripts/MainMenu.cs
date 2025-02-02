using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject gameSelectionPanel;
    public GameObject mainMenuPanel;
    public void Options(){
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void SelectAGame(){
        gameSelectionPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void Quit(){
        Application.Quit();
    }
    public void ReturnMainMenu(){
        optionsPanel.SetActive(false);
        gameSelectionPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ReflexGame(){
        SceneManager.LoadScene("SampleScene");
    }
    public void RoutineGame(){
        SceneManager.LoadScene("RoutineGame");
    }
    public void AudioGame(){
        SceneManager.LoadScene("AudioGame");
    }
    public void ImageGame(){
        SceneManager.LoadScene("OddImagePickerGame");
    }
    public void BallThrowingGame(){
        SceneManager.LoadScene("BallGame");
    }
    public void ConstructionGame(){
        SceneManager.LoadScene("BuilderGame");
    }
}
