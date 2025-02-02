using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionBallGame : MonoBehaviour
{
   public GameObject player;
   public GameObject backgroundImage;
   public GameObject virtualMouse;

   public void OnStartButtonClick(){
    player.SetActive(true);
    backgroundImage.SetActive(true);
    virtualMouse.SetActive(false);
    this.gameObject.SetActive(false);
   }
}
