using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ImagePickerGameController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject scoreTexta;
    public Transform[] spawnPoints;
    public List<RelatedImageSet> relatedImageSets;
    public List<Sprite> unrelatedImages;
    public Image countdownImage;
    public TMP_Text scoreText;
    public TMP_Text countdownText;
    public TMP_Text finalScoreText;
    public ImagePickerGameGameOver gameOver;
    private int score = 0;
    private int sessionCount = 0;
    private float sessionTime = 15f;
    private float delayTime = 3f;
    private bool isSessionActive = false;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    private List<int> usedRelatedSets = new List<int>();
    private List<int> usedUnrelatedImages = new List<int>();
    public AudioClip successSound;
    public AudioClip failSound;

    private void Start()
    {
        if (buttonPrefab == null || spawnPoints == null || relatedImageSets == null || unrelatedImages == null || countdownImage == null || scoreText == null || countdownText == null)
        {
            Debug.LogError("Some references are not set in the Inspector.");
            return;
        }
        scoreTexta.SetActive(true);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (sessionCount < 4)
        {
            yield return StartCoroutine(StartSession());
            yield return StartCoroutine(DelayWithCountdown());
        }

        EndGame();
    }

    private IEnumerator StartSession()
    {
        sessionCount++;
        Debug.Log($"Starting Session {sessionCount}");

        SpawnButtons();
        isSessionActive = true;
        countdownImage.fillAmount = 1f;
        float timer = sessionTime;

        while (timer > 0f)
        {
            if (!isSessionActive) break;

            timer -= Time.deltaTime;
            countdownImage.fillAmount = timer / sessionTime;
            countdownText.text = Mathf.CeilToInt(timer).ToString();
            yield return null;
        }
        if (isSessionActive)
        {
            SoundFXManager.instance.PlaySoundFXClip(failSound,transform,1f);
        }
        isSessionActive = false;
        RemoveButtons();
    }

    private IEnumerator DelayWithCountdown()
    {
        float timer = delayTime;
        while (timer > 0f)
        {
            countdownText.text = Mathf.CeilToInt(timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }
        countdownText.text = "";
    }

    private void SpawnButtons()
    {
        Debug.Log("Spawning buttons...");
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points are not set.");
            return;
        }

        if (usedRelatedSets.Count >= relatedImageSets.Count)
        {
            Debug.LogError("No more related image sets available.");
            return;
        }

        List<Transform> shuffledSpawnPoints = new List<Transform>(spawnPoints);
        for (int i = 0; i < shuffledSpawnPoints.Count; i++)
        {
            Transform temp = shuffledSpawnPoints[i];
            int randomIndex = Random.Range(i, shuffledSpawnPoints.Count);
            shuffledSpawnPoints[i] = shuffledSpawnPoints[randomIndex];
            shuffledSpawnPoints[randomIndex] = temp;
        }

        int relatedSetIndex = -1;
        if (usedRelatedSets.Count < relatedImageSets.Count)
        {
            do
            {
                relatedSetIndex = Random.Range(0, relatedImageSets.Count);
            } while (usedRelatedSets.Contains(relatedSetIndex));
            usedRelatedSets.Add(relatedSetIndex);
        }

        List<Sprite> relatedImages = new List<Sprite>(relatedImageSets[relatedSetIndex].images);

        int unrelatedImageIndex;
        do
        {
            unrelatedImageIndex = Random.Range(0, unrelatedImages.Count);
        } while (usedUnrelatedImages.Contains(unrelatedImageIndex));

        usedUnrelatedImages.Add(unrelatedImageIndex);
        Sprite unrelatedImage = unrelatedImages[unrelatedImageIndex];

        for (int i = 0; i < shuffledSpawnPoints.Count; i++)
        {
            GameObject button = Instantiate(buttonPrefab, shuffledSpawnPoints[i].position, Quaternion.identity, shuffledSpawnPoints[i]);
            Image buttonImage = button.GetComponent<Image>();

            if (i == 0)
            {
                buttonImage.sprite = unrelatedImage;
                button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(false));
                Debug.Log($"Unrelated button spawned at {shuffledSpawnPoints[i].position}");
            }
            else
            {
                if (relatedImages.Count == 0)
                {
                    Debug.LogError("No more related images available to assign.");
                    return;
                }

                int relatedImageIndex = Random.Range(0, relatedImages.Count);
                buttonImage.sprite = relatedImages[relatedImageIndex];
                relatedImages.RemoveAt(relatedImageIndex);
                button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(true));
                Debug.Log($"Related button spawned at {shuffledSpawnPoints[i].position} with image {relatedImageIndex}");
            }

            spawnedButtons.Add(button);
        }

        Debug.Log("Buttons spawned successfully.");
    }

    private void RemoveButtons()
    {
        Debug.Log("Removing buttons...");
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();
        Debug.Log("Buttons removed");
    }

    private void OnButtonClick(bool isRelated)
    {
        if (!isSessionActive) return;

        isSessionActive = false;
        if (!isRelated)
        {
            score++;
            SoundFXManager.instance.PlaySoundFXClip(successSound,transform,1f);
            if (scoreText != null)
            {
                scoreText.text = "Skor: " + score;
            }
            else
            {
                Debug.LogError("Skor Text is not assigned.");
            }
        }
        if(isRelated){
            SoundFXManager.instance.PlaySoundFXClip(failSound,transform,1f);
        }
        RemoveButtons();
    }

    private void EndGame()
    {
        StopAllCoroutines();
        finalScoreText.text = "Skor: " + score;
        scoreTexta.SetActive(false);
        gameOver.Setup();
    }
}
