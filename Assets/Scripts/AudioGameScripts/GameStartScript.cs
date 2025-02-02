using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public GameObject answerText;
    public GameObject gameManager;
    public GameObject countdown;

    public void OnButtonClickStart(){
        answerText.SetActive(true);
        gameManager.SetActive(true);
        countdown.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
