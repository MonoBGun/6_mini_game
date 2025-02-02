using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallGameEndgameScript : MonoBehaviour
{
    public GameObject player;
    public GameObject backgroundImage;
    public GameObject virtualMouse;

    public void Setup(){
        this.gameObject.SetActive(true);
        player.SetActive(false);
        backgroundImage.SetActive(false);
        virtualMouse.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("BallGame");
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
