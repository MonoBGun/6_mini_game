using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ImagePickerGameGameOver : MonoBehaviour
{
    public GameObject[] closer;
    public GameObject ImagePickerGameController;
    public void Setup(){
        gameObject.SetActive(true);
        for (int i = 0; i < closer.Length; i++)
        {
            closer[i].SetActive(false);
        }
    }
    public void RestartButton(){
        SceneManager.LoadScene("OddImagePickerGame");
    }
    public void MainMenuButton(){
        SceneManager.LoadScene("MainMenu");
    }
}
