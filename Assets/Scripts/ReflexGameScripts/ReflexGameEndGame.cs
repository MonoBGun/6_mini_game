using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReflexGameEndGame : MonoBehaviour
{
   public GameObject colorBall;
    public GameObject gameManager;
    public GameObject virtualMouse;
    public void Setup(){
        this.gameObject.SetActive(true);
        colorBall.SetActive(false);
        gameManager.SetActive(false);
        virtualMouse.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("SampleScene");
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
