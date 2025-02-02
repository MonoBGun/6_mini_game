using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartImagePicker : MonoBehaviour
{
  public GameObject gameController;
  public GameObject spawnPoints;
  public GameObject countdown;

  public void ButtonOnClick(){
    gameController.SetActive(true);
    spawnPoints.SetActive(true);
    countdown.SetActive(true);
    this.gameObject.SetActive(false);
  }
}
