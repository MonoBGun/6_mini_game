using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuButtonController : MonoBehaviour
{
    UnityEngine.UI.Image image;
    Button button;
    int a=0;
    void Start(){
        image=this.gameObject.GetComponent<UnityEngine.UI.Image>();
        button=this.gameObject.GetComponent<Button>();
    }
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&&a==0){
            image.enabled=false;
            button.enabled=false;
            foreach(Transform child in transform){
                child.gameObject.SetActive(false);  
            }
            a=1;
        }
        if(a==1&&PauseMenu.GameIsPaused==false){
            image.enabled=true;
            button.enabled=true;
            foreach(Transform child in transform){
                child.gameObject.SetActive(true);  
            }
            a=0;
            Debug.Log(a);
        }

    }
}
