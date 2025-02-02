using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class PointSpawner : MonoBehaviour
{
    public GameObject rndCube;
    public bool _timerActive;
    public float _currentTime;
    
    public TMP_Text bigPuantext;

    public TMP_Text puantext;

    [SerializeField] BoxCollider2D bc;

    Vector2 cubeSize;
    Vector2 cubeCenter;

    public bool checkIfHit;
    public Animator animator;
    public BallGameEndgameScript gameOverScreen;


    private void Start(){
        StartCoroutine(Spawner());
        _currentTime=0;
    }
    IEnumerator Spawner(){
        //Text
        yield return new WaitForSeconds(3f);
        _timerActive=true;
        //countdown
        for (int i = 0; i < 10; i++)
        {
            GameObject go=Instantiate(rndCube,new Vector2(Random.Range(-1.35f,7.62f),Random.Range(1.23f,4f)),Quaternion.identity);
            checkIfHit=true;
            while(checkIfHit==true){
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
        TimeSpan time=TimeSpan.FromSeconds(_currentTime);
        bigPuantext.text="Skor: "+time.Minutes.ToString()+":"+time.Seconds.ToString()+time.Milliseconds.ToString();
        GameOver();
        //ana menüye dönmek için buton
    }
    public void GameOver(){
        gameOverScreen.Setup();
    }
    void Update(){
        if(_timerActive){
            _currentTime=_currentTime+Time.deltaTime;
        }
        TimeSpan time=TimeSpan.FromSeconds(_currentTime);
        bigPuantext.text="Skor: "+time.Minutes.ToString()+":"+time.Seconds.ToString()+":"+time.Milliseconds.ToString();
        
    }
    public void StartTimer(){
        _timerActive=true;
    }
    public void StopTimer(){
        _timerActive=false;
    }
}
