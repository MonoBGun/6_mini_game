using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoutineGameEndGame : MonoBehaviour
{
    public void RestartGame(){
        SceneManager.LoadScene("RoutineGame");
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
