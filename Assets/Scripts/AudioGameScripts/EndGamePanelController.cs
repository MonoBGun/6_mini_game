using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGamePanelController : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject countdown;
    public GameObject[] buttons;
    public GameObject answerText;
    public TMP_Text scoreText;

    public void Setup(){
        scoreText.text="Skor: "+gameManager.GetComponent<AudioGameController>().TotalCorrectAnswers.ToString();
        gameManager.SetActive(false);
        countdown.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            buttons[i].SetActive(false);
        }
        answerText.SetActive(false);
        this.gameObject.SetActive(true);
    }
    public void Restart(){
        SceneManager.LoadScene("AudioGame");
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
