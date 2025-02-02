using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameOverScreen : MonoBehaviour
{
    public Animator animator;
    public TMP_Text scoreText;
    public GameObject virtualmouse;
    public GameObject inGameVirtualMouse;
    public PointSpawner pointspawn;
    public void Setup(){
        gameObject.SetActive(true);
        virtualmouse.SetActive(true);
        inGameVirtualMouse.SetActive(false);
        pointspawn._timerActive=false;
        animator.SetTrigger("Change");
    }
    public void RestartButton(){
        SceneManager.LoadScene("BallGame");
    }
    public void ExitButton(){
    SceneManager.LoadScene("MainMenu");
    }
    
}
