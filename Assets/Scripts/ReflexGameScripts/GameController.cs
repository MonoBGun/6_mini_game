using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int qteGen;
    public AudioClip succsess_audio;
    public AudioClip fail_audio;
    int waitingForKey=2;
    int correctKey;
    GameObject colorBall;
    ColorChange colorChange;
    private float sessionTimer;
    private float totalTimer;
    private int counter=0;
    private int totalsuc;
    private bool timeractive;
    private SpriteRenderer _spriteRenderer;
    private int countingDown;
    public TMP_Text bilgilendirme;
    public TMP_Text displayBox;
    public TMP_Text Timer;
    public ReflexGameEndGame endGame;
    void Start()
    {
        colorChange=GameObject.FindGameObjectWithTag("ColorBall").GetComponent<ColorChange>();
        colorBall=GameObject.FindGameObjectWithTag("ColorBall");
        _spriteRenderer=colorBall.GetComponent<SpriteRenderer>();
        displayBox.text="Başlamak için ateşleyiciye basınız!";
        timeractive=false;
    }

    void Update(){
        if(waitingForKey==2){
            if(Input.GetKeyDown(KeyCode.Joystick1Button0)||Input.GetKeyDown(KeyCode.Space)){
                waitingForKey=0;
                displayBox.text="";
                bilgilendirme.text="";
                totalTimer=0;
                totalsuc=0;
                counter=0;
            }
        }
        if(timeractive){
            sessionTimer=sessionTimer+Time.deltaTime;
        }
        if (waitingForKey==0&&counter<10)
            {
                countingDown=1;
                StartCoroutine(CountDown());
                timeractive=true;
                colorChange.ChangeColor();
                if (_spriteRenderer.color==Color.red)
                {
                qteGen=1;
                bilgilendirme.text="[2]";
                }
                if (_spriteRenderer.color==Color.green)
                {
                qteGen=2;
                bilgilendirme.text="[3]";
                }
                if (_spriteRenderer.color==Color.blue)
                {
                qteGen=3;
                bilgilendirme.text="[4]";
                }
                if (_spriteRenderer.color==Color.yellow)
                {
                qteGen=4;
                bilgilendirme.text="[5]";
                }
                if(_spriteRenderer.color==Color.magenta){
                    qteGen=5;
                    bilgilendirme.text="[6]";
                }
                waitingForKey=3;
            }
            if(waitingForKey==1){
                if(qteGen==1){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button1)||Input.GetKeyDown(KeyCode.Keypad2))
                    {
                        correctKey=1;
                        Debug.Log(sessionTimer);
                        StartCoroutine(ColorGame());
                    }
                    else
                    {
                        correctKey=2;
                        Debug.Log(sessionTimer);
                        StartCoroutine(ColorGame());
                    }
                }
            }
            if(qteGen==2){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button2)||Input.GetKeyDown(KeyCode.Keypad3))
                    {
                        correctKey=1;
                        Debug.Log(sessionTimer);
                        StartCoroutine(ColorGame());
                    }
                    else
                    {
                        correctKey=2;
                        Debug.Log(sessionTimer);
                        StartCoroutine(ColorGame());
                    }
                }
            }
            if(qteGen==3){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button3)||Input.GetKeyDown(KeyCode.Keypad4))
                    {
                        correctKey=1;
                        StartCoroutine(ColorGame());
                    }
                    else
                    {
                        correctKey=2;
                        StartCoroutine(ColorGame());
                    }
                }
            }
            if(qteGen==4){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button4)||Input.GetKeyDown(KeyCode.Keypad5))
                    {
                        correctKey=1;
                        StartCoroutine(ColorGame());
                    }
                    else
                    {
                        correctKey=2;
                        StartCoroutine(ColorGame());
                    }
                }
            }
            if(qteGen==5){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button5)||Input.GetKeyDown(KeyCode.Keypad6))
                    {
                        correctKey=1;
                        StartCoroutine(ColorGame());
                    }
                    else
                    {
                        correctKey=2;
                        StartCoroutine(ColorGame());
                    }
                }
            }
            if(counter==10){
                EndGame();
            }
            }
    }
    IEnumerator ColorGame(){
        qteGen=6;
        counter++;
        if(correctKey==1){
            countingDown=2;
            SoundFXManager.instance.PlaySoundFXClip(succsess_audio,transform,1f);
            totalsuc++;
            Debug.Log(totalsuc);
            timeractive=false;
            totalTimer=totalTimer+sessionTimer;
            sessionTimer=0;
            colorChange.ResetColor();
            displayBox.text="Bekleyin!";
            yield return new WaitForSeconds(4f);
            correctKey=0;
            yield return new WaitForSeconds(2f);
            waitingForKey=0;
            countingDown=1;
            displayBox.text="";
        }
        if(correctKey==2){
            countingDown=2;
            timeractive=false;
            SoundFXManager.instance.PlaySoundFXClip(fail_audio,transform,1f);
            sessionTimer=0;
            colorChange.ResetColor();
            displayBox.text="Bekleyin!";
            yield return new WaitForSeconds(4f);
            correctKey=0;
            yield return new WaitForSeconds(2f);
            waitingForKey=0;
            countingDown=1;
            displayBox.text="";
        }
    }
    private void EndGame(){
        if (totalsuc>0)
        {
            float alfa=totalTimer/totalsuc;
            TimeSpan time=TimeSpan.FromSeconds(alfa);
            Timer.text="Ortalama: "+time.Seconds.ToString()+":"+time.Milliseconds.ToString()+" "+"Saniye";
            displayBox.text="Toplam Doğru Sayısı: "+totalsuc;
            bilgilendirme.text="";
            endGame.Setup();
        }
        else
        {
            bilgilendirme.text="Yeniden İçin Ateşleyiciye Basınız!";
            displayBox.text="Tekrar Dene!";
            bilgilendirme.text="";
            endGame.Setup();
        }
    }
    IEnumerator CountDown(){
        yield return new WaitForSeconds(0.5f);
        waitingForKey=1;
        yield return new WaitForSeconds(6f);
        if(countingDown==1){
            qteGen=6;
            countingDown=2;
            timeractive=false;
            sessionTimer=0;
            colorChange.ResetColor();
            displayBox.text="Bekleyin!";
            yield return new WaitForSeconds(4f);
            correctKey=0;
            yield return new WaitForSeconds(2f);
            waitingForKey=0;
            countingDown=1;
            displayBox.text="";
        }
    }

}
