using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject plankPrefab;
    private List<GameObject> placedPlanks = new List<GameObject>();
    private Camera mainCamera;
    private int currentLevel = 1;
    private int maxLevel = 2;
    public static int rotationAmount = 60; // Default rotation amount for level 1
    private int currentAttempt = 0;
    public PanelControllerBG panelControllerBG; // Reference to PanelControllerBG script
    public AudioClip failsound;
    public AudioClip sucsound;
    public SpriteRenderer quber;
    public Sprite qube;
    public int connectionint = 0; // Connection count

    void Start()
    {
        mainCamera = Camera.main;
        StartLevel(currentLevel);
        Debug.Log($"GameManager started. Initial Connection Count: {connectionint}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)) // Button for checking connections
        {
            Debug.Log($"Checking connections at Level {currentLevel}. Current Connection Count: {connectionint}");
            if (CheckAllPlankConnections())
            {
                Debug.Log(currentLevel);
                currentLevel++;
                if (currentLevel <= maxLevel)
                {
                    StartLevel(currentLevel);
                }
                else
                {
                    EndGame(true);
                }
            }
            else
            {
                Debug.Log("Connections not valid. Try again.");
                RetryLevel();
            }
        }
    }

    public void IncrementConnectionCount()
    {
        connectionint++;
        Debug.Log($"IncrementConnectionCount called. New Connection Count: {connectionint}");
    }

    public void DecrementConnectionCount()
    {
        connectionint--;
        Debug.Log($"DecrementConnectionCount called. New Connection Count: {connectionint}");
    }

    void StartLevel(int level)
    {
        ClearPlanks();
        int numberOfPlanks =level+2;
        rotationAmount = (level == 1) ? 60 : 90;
        currentAttempt = 0; // Reset attempts for the new level
        connectionint = 0; // Reset connection count for the new level
        if (currentLevel != 1)
        {
            quber.sprite = qube;
        }
        for (int i = 0; i < numberOfPlanks; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinCamera();
            GameObject newPlank = Instantiate(plankPrefab, randomPosition, Quaternion.identity);
            newPlank.tag = "Plank";
            placedPlanks.Add(newPlank);
        }

        Debug.Log($"Level {level} started with {numberOfPlanks} planks and rotation amount {rotationAmount}. Connection Count reset to {connectionint}");
    }

    void RetryLevel()
    {
        ClearPlanks();
        int numberOfPlanks = currentLevel+2;
        SoundFXManager.instance.PlaySoundFXClip(failsound, transform, 1f);
        if (currentLevel != 1)
        {
            quber.sprite = qube;
        }
        for (int i = 0; i < numberOfPlanks; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinCamera();
            GameObject newPlank = Instantiate(plankPrefab, randomPosition, Quaternion.identity);
            newPlank.tag = "Plank";
            placedPlanks.Add(newPlank);
        }

        Debug.Log($"Retrying level {currentLevel} with {numberOfPlanks} planks.");
    }

    Vector3 GetRandomPositionWithinCamera()
    {
        float minX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        float maxX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;
        float minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;
        float maxY = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane)).y;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector3(randomX, randomY, 0);
    }

    void ClearPlanks()
    {
        foreach (GameObject plank in placedPlanks)
        {
            Destroy(plank);
        }
        placedPlanks.Clear();
    }

    bool CheckAllPlankConnections()
    {
        int requiredConnections=2;
        requiredConnections*=(currentLevel+2);
        Debug.Log(requiredConnections);
        Connector[] connectors = FindObjectsOfType<Connector>();

        bool allConnected = true;
        foreach (var connector in connectors)
        {
            if (!connector.isConnected)
            {
                allConnected = false;
                break;
            }
        }

        if (connectionint >= requiredConnections && allConnected)
        {
            Debug.Log($"Level {currentLevel} completed! You win!");
            SoundFXManager.instance.PlaySoundFXClip(sucsound,transform,1f);
            return true;
        }

        Debug.Log("Not enough connections or not all connectors are connected.");
        return false;
    }

    void EndGame(bool success)
    {
        panelControllerBG.GameOverBG();
    }
}
