using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class AudioGameController : MonoBehaviour
{
    [SerializeField] public AudioClip[] audioClipArray;
    [SerializeField] public Sprite[] spriteArray;
    [SerializeField] public Sprite[] wrongSprites;
    private float countdownFloat=0f;
    public TMP_Text answerText;
    public TMP_Text countdownText;
    [SerializeField] public Button[] buttons;
    [SerializeField] public GameObject[] objectButtons;
    private Button tempButton;
    public bool timerIsRunning = false;
    List<int> array1=new List<int>{0,1,2,3,4,5,6,7,8,9,10,11,12};
    private int correctNumber;
    public int TotalCorrectAnswers=0;
    public EndGamePanelController audioGameOver;
    public Image fill;
    public float max; 
    void Start()
    {
        Shuffle(array1);
        StartCoroutine(Spawnerc());
    }
    IEnumerator Spawnerc(){
        for (int i = 0; i < 10; i++)
        {
            for (int c = 0; c < 2; c++)
            {
                objectButtons[c].SetActive(false);
            }
            answerText.text="Hazirlan!";
            countdownFloat=3f;
            yield return new WaitForSeconds(3);
            countdownFloat=15;
            timerIsRunning=true;
            answerText.text="";
            SoundFXManager.instance.PlaySoundFXClip(audioClipArray[array1[i]],transform,1f);
            int rnd=UnityEngine.Random.Range(0,3);
            correctNumber=rnd;
            buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
            tempButton=buttons[rnd];
            Debug.Log(array1[i].ToString());
            if(array1[i]==13){
                if(correctNumber==0){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[1].GetComponent<Image>().sprite=spriteArray[array1[rnd]];
                buttons[2].GetComponent<Image>().sprite=wrongSprites[array1[i]];
                }
                if(correctNumber==1){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[0].GetComponent<Image>().sprite=wrongSprites[array1[i]];
                buttons[2].GetComponent<Image>().sprite=spriteArray[array1[rnd]];
                }
                if(correctNumber==2){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[0].GetComponent<Image>().sprite=wrongSprites[array1[i]];
                buttons[1].GetComponent<Image>().sprite=spriteArray[array1[rnd]];
                }
            }
            else{
                
            if(correctNumber==0){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[1].GetComponent<Image>().sprite=spriteArray[array1[i]+1];
                buttons[2].GetComponent<Image>().sprite=wrongSprites[array1[i]+1];
            }
            if(correctNumber==1){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[0].GetComponent<Image>().sprite=wrongSprites[array1[i]];
                buttons[2].GetComponent<Image>().sprite=spriteArray[array1[i]+1];
            }
            if(correctNumber==2){
                buttons[correctNumber].GetComponent<Image>().sprite=spriteArray[array1[i]];
                buttons[0].GetComponent<Image>().sprite=wrongSprites[array1[i]];
                buttons[1].GetComponent<Image>().sprite=spriteArray[array1[i]+1];
            }
            }
            for (int n = 0; n < 3; n++)
            {
                objectButtons[n].SetActive(true);
            }
            while(timerIsRunning){
                yield return null;
            }
            for (int b = 0; b < 3; b++)
            {
                objectButtons[b].SetActive(false);
            }
            yield return new WaitForSeconds(2);
        }
        countdownText.text="";
        GameOver();
    }
    private void GameOver(){
        audioGameOver.Setup();
    }
    void Update()
    {
        if(timerIsRunning){
            if(countdownFloat>0){
            countdownFloat-=Time.deltaTime;
            DisplayTime(countdownFloat);
            }else{
                countdownFloat=0;
            timerIsRunning=false;
            }
        }
        
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay/60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        fill.fillAmount=countdownFloat/max;
        countdownText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
    void Shuffle<T>(List<T>inputList){
        for (int i = 0; i < inputList.Count-1; i++)
        {
            T temp =inputList[i];
            int rand=UnityEngine.Random.Range(1,inputList.Count);
            inputList[i]=inputList[rand];
            inputList[rand]=temp;
        }
        Debug.Log(String.Join(", ",array1));
    }
    public void OnClickButton(){
        int a =Array.IndexOf(buttons,tempButton);
        GameObject gameObject=GameObject.FindGameObjectWithTag("SoundEffectObject");
        Destroy(gameObject);
        UnityEngine.Vector3 thisPosition=EventSystem.current.currentSelectedGameObject.GetComponent<Transform>().localPosition;
        if(thisPosition==buttons[correctNumber].GetComponent<Transform>().localPosition){
            
            for (int i = 0; i < 3; i++)
            {
                objectButtons[i].SetActive(false);
            }
            answerText.text="Doğru Cevap!";
            timerIsRunning=false;
            countdownFloat=0;
            TotalCorrectAnswers++;
        }
        else{
            for (int i = 0; i < 3; i++)
            {
                objectButtons[i].SetActive(false);
                answerText.text="Yanlış Cevap!";
                timerIsRunning=false;
                countdownFloat=0;
            }
        }
    }
    
}


