using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused=false;
    public GameObject PauseMenuUI;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }
    public void Resume(){
        if(SceneManager.GetActiveScene().name!="MainMenu"){
        PauseMenuUI.SetActive(false);
        Time.timeScale=1f;
        GameIsPaused=false;
        AudioSource[] audios=FindObjectsOfType<AudioSource>();
        foreach(AudioSource a in audios){
            a.Play();
        }
        if(SceneManager.GetActiveScene().name=="AudioGame"){
            GameObject a=GameObject.FindWithTag("DontDestroyOnLoad");
            a.GetComponent<AudioSource>().Pause();
        }
        }
    }
    public void Pause(){
        if(SceneManager.GetActiveScene().name!="MainMenu"){
            PauseMenuUI.SetActive(true);
            Time.timeScale=0f;
            GameIsPaused=true;
            AudioSource[] audios=FindObjectsOfType<AudioSource>();
            foreach(AudioSource a in audios){
            a.Pause();
            }
        }
    }
    public void LoadMenu(){
        if(SceneManager.GetActiveScene().name!="MainMenu"){
            PauseMenuUI.SetActive(false);
            GameIsPaused=false;
            Time.timeScale=1f;
            AudioSource[] audios=FindObjectsOfType<AudioSource>();
            foreach(AudioSource a in audios){
                a.Play();
            }
            SceneManager.LoadScene("MainMenu");
        }
    }

}
