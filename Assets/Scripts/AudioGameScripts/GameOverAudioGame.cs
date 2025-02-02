using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverAudioGame : MonoBehaviour
{
    public Animator animator;
    public GameObject audioGameController;
    public void Setup(){
        gameObject.SetActive(true);
        animator.SetTrigger("Change");
    }
    
}
