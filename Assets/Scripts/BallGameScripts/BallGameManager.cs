using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallGameManager : MonoBehaviour
{
    public int puan;
    public bool puancheck=false;

    public TMP_Text puantext;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(puancheck==true){
            puantext.text="Puan: "+puan.ToString();
            puancheck=false;
        }
    }
    void MainMenu(){

    }
}
