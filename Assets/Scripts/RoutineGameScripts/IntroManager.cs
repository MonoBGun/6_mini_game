using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel; // Reference to the intro panel
    public GameObject[] items;
    public GameObject virtualMouse;
    public GameObject cursor;
    public GameObject brushIntroPanel;

    private void Start()
    {
        // Ensure the intro panel is active at the start
        items[0].SetActive(false);
        items[1].SetActive(false);
    }

    // This method will be called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        // Deactivate the intro panel
        introPanel.SetActive(false);
        items[0].SetActive(true);
        items[1].SetActive(true);
        virtualMouse.SetActive(false);
        cursor.SetActive(true);

    }

    public void OnContinueButtonClicked(){
        brushIntroPanel.SetActive(false);
        virtualMouse.SetActive(false);
    }
        // Optionally, you can add any other code here to start the game
}

