using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar; // Reference to the UI Image with fill properties
    public float fillSpeed = 0.1f; // Adjust this value to control fill speed
    private float targetProgress = 0;
    public GameObject allitems;
    public GameObject[] objectsToOpen;
    public GameObject cursor;
    public GameObject virtualMouse;

    private void Start()
    {
        progressBar.fillAmount = 0;
        Debug.Log("ProgressBar initialized");
    }

    private void Update()
    {
        
        if (progressBar.fillAmount < targetProgress)
        {
                progressBar.fillAmount += fillSpeed * Time.deltaTime;
        }
        if(progressBar.fillAmount==1&&allitems.activeInHierarchy==true){
            SecondFaze();
        }
    }

    public void IncreaseProgress(float newProgress)
    {
        Debug.Log("Increasing progress to: " + newProgress);
        targetProgress = newProgress;
    }
    private void SecondFaze(){
        allitems.SetActive(false);
        objectsToOpen[0].SetActive(true);
        objectsToOpen[1].SetActive(true);
        progressBar.fillAmount=0;
        cursor.SetActive(false);
        virtualMouse.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
