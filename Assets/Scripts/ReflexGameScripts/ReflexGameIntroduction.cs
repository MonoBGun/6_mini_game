using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflexGameIntroduction : MonoBehaviour
{
    public GameObject colorBall;
    public GameObject gameManager;
    public GameObject displayBox;
    public GameObject virtualMouse;

    public void OnButtonClickStart(){
        colorBall.SetActive(true);
        gameManager.SetActive(true);
        displayBox.SetActive(true);
        virtualMouse.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
