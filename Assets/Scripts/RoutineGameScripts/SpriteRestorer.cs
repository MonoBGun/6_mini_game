using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRestorer : MonoBehaviour
{
    public GameObject endGamePanel;
    public GameObject virtualMouse;
    public SpriteRenderer dirtySpriteRenderer;  // Renderer for the dirty sprite
    public SpriteRenderer cleanSpriteRenderer;  // Renderer for the clean sprite
    public Image progressImage; // Image component to act as the progress slider
    public int brushSize = 5; // Define brush size
    public Sprite indicatorSprite; // Public variable for the custom indicator sprite
    public ParticleSystem paintParticleSystem; // Particle system for painting effect
    public GameObject infoPanel; // Info panel game object
    public Button startButton; // Button to start the game
    public GameObject otherCursor; // Reference to the other cursor

    // Joystick parameters
    public float joystickSpeed = 5f;
    private Vector2 joystickPosition;
    private Texture2D dirtyTexture;
    private Texture2D cleanTexture;
    private int totalDirtyPixels = 0;
    private const int totalCleanPixels = 80066; // Fixed total clean pixels
    private int restoredPixels = 0;
    private bool isProgressDirty = false;
    private bool isInteractable = false;

    private GameObject cursor;  // Visual indicator for joystick position
    private SpriteRenderer cursorRenderer;
    public AudioClip brushSound;
    int soundThreashold=100;

    private void Start()
    {
        if (dirtySpriteRenderer == null || cleanSpriteRenderer == null || progressImage == null)
        {
            Debug.LogError("SpriteRenderers or ProgressImage are not assigned.");
            return;
        }

        // Initialize joystick position at the center of the screen
        joystickPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        // Create a new texture for the dirty sprite that is readable and writable
        Texture2D dirtyOriginalTexture = dirtySpriteRenderer.sprite.texture;
        dirtyTexture = new Texture2D(dirtyOriginalTexture.width, dirtyOriginalTexture.height, TextureFormat.RGBA32, false);
        dirtyTexture.SetPixels(dirtyOriginalTexture.GetPixels());
        dirtyTexture.Apply();
        dirtySpriteRenderer.sprite = Sprite.Create(dirtyTexture, dirtySpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));

        // Create a new texture for the clean sprite that is readable
        cleanTexture = cleanSpriteRenderer.sprite.texture;

        Debug.Log($"Dirty Sprite Texture Format: {dirtyTexture.format}");
        Debug.Log($"Dirty Sprite Texture Size: {dirtyTexture.width}x{dirtyTexture.height}");

        // Create a visual cursor for feedback
        cursor = new GameObject("Cursor");
        cursorRenderer = cursor.AddComponent<SpriteRenderer>();
        cursorRenderer.sprite = indicatorSprite != null ? indicatorSprite : dirtySpriteRenderer.sprite;
        cursor.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Scale to 0.2 of its size

        // Adjust sorting order to ensure cursor is in front
        cursorRenderer.sortingOrder = dirtySpriteRenderer.sortingOrder + 1;

        // Adjust Z position to ensure cursor is in front
        cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, dirtySpriteRenderer.transform.position.z - 1);

        // Add a collider if not present
        if (GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // Ensure the GameObject has the correct tag
        gameObject.tag = "Paintable";

        // Set the progress image fill method to Filled if not already set
        if (progressImage.type != Image.Type.Filled)
        {
            progressImage.type = Image.Type.Filled;
        }

        // Set the fill method to Horizontal (or any other direction you prefer)
        progressImage.fillMethod = Image.FillMethod.Horizontal;
        progressImage.fillAmount = 0f; // Initialize the fill amount to 0

        // Calculate the total number of pixels within the collider area of the dirty texture
        StartCoroutine(CalculateTotalDirtyPixels());

        // Set up the start button click listener
        startButton.onClick.AddListener(StartGame);

        // Find the button with the tag "Soap" and add a listener to set otherCursor inactive
        Button soapButton = GameObject.FindGameObjectWithTag("Soap").GetComponent<Button>();
        if (soapButton != null)
        {
            soapButton.onClick.AddListener(DeactivateOtherCursor);
        }
    }

    private void Update()
    {
        if (!isInteractable)
        {
            return;
        }

        // Get joystick input
        float moveHorizontal = Input.GetAxis("JoystickHorizontal");
        float moveVertical = Input.GetAxis("JoystickVertical");

        // Debug logs for joystick input
        Debug.Log($"Joystick Input - Horizontal: {moveHorizontal}, Vertical: {moveVertical}");

        // Update joystick position
        joystickPosition += new Vector2(moveHorizontal, moveVertical) * joystickSpeed * Time.deltaTime;
        joystickPosition.x = Mathf.Clamp(joystickPosition.x, 0, Screen.width);
        joystickPosition.y = Mathf.Clamp(joystickPosition.y, 0, Screen.height);

        // Convert joystick position to world point
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(joystickPosition);

        // Debug log for cursor world position
        Debug.Log($"Cursor World Position: {worldPos}");

        // Update cursor position for visual feedback
        cursor.transform.position = new Vector3(worldPos.x, worldPos.y, dirtySpriteRenderer.transform.position.z - 1);

        // Adjust cursor position to make the left side of the sprite paint
        Vector2 cursorSize = cursorRenderer.bounds.size;
        Vector2 paintPosition = worldPos + new Vector2(-cursorSize.x / 3, cursorSize.y / 3); // Adjust the offset to move cursor up

        // Check if the painting button is held down
        if (Input.GetKey(KeyCode.Joystick1Button0))
        {
            Debug.Log("Joystick Button Pressed");

            // Perform the raycast
            RaycastHit2D hit = Physics2D.Raycast(paintPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Paintable"))
            {
                Debug.Log($"Hit detected on: {hit.collider.name}");
                Restore(hit.point, hit.collider);
            }
        }
    }

    private void Restore(Vector2 worldPoint, Collider2D collider)
    {
        // Convert world point to local point in the sprite's space
        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);

        // Convert local point to texture coordinates
        float pixelsPerUnit = dirtySpriteRenderer.sprite.pixelsPerUnit;
        Vector2 pivot = dirtySpriteRenderer.sprite.pivot;
        float x = (localPoint.x * pixelsPerUnit) + pivot.x;
        float y = (localPoint.y * pixelsPerUnit) + pivot.y;

        Vector2 textureCoord = new Vector2(x, y);
        Vector2Int textureCoordInt = new Vector2Int(Mathf.FloorToInt(textureCoord.x), Mathf.FloorToInt(textureCoord.y));

        // Track whether any pixels were actually restored
        bool restoredAnyPixels = false;

        // Restore multiple pixels within the brush area
        List<Color> pixelsToRestore = new List<Color>();
        List<Vector2Int> coordinatesToRestore = new List<Vector2Int>();

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                Vector2Int brushCoord = new Vector2Int(textureCoordInt.x + i, textureCoordInt.y + j);

                if (brushCoord.x >= 0 && brushCoord.x < dirtyTexture.width && brushCoord.y >= 0 && brushCoord.y < dirtyTexture.height)
                {
                    Vector2 worldBrushCoord = dirtySpriteRenderer.transform.TransformPoint(new Vector3((brushCoord.x - pivot.x) / pixelsPerUnit, (brushCoord.y - pivot.y) / pixelsPerUnit, 0));
                    if (collider.OverlapPoint(worldBrushCoord))
                    {
                        Color currentColor = dirtyTexture.GetPixel(brushCoord.x, brushCoord.y);
                        Color cleanColor = cleanTexture.GetPixel(brushCoord.x, brushCoord.y);
                        if (currentColor != cleanColor)
                        {
                            pixelsToRestore.Add(cleanColor);
                            coordinatesToRestore.Add(brushCoord);
                            restoredAnyPixels = true;
                            restoredPixels++;
                            GameObject soundfxobject=GameObject.FindGameObjectWithTag("SoundEffectObject");
                            if (restoredPixels==soundThreashold)
                            {
                                soundThreashold=soundThreashold+2000;
                            }
                            if (soundThreashold>restoredPixels+400&&soundfxobject==null)
                            {
                                SoundFXManager.instance.PlaySoundFXClip(brushSound,transform,1f);
                            }
                        }
                    }
                }
            }
        }

        if (restoredAnyPixels)
        {
            // Apply pixel changes in a batch
            for (int i = 0; i < pixelsToRestore.Count; i++)
            {
                dirtyTexture.SetPixel(coordinatesToRestore[i].x, coordinatesToRestore[i].y, pixelsToRestore[i]);
            }
            dirtyTexture.Apply();
            dirtySpriteRenderer.sprite = Sprite.Create(dirtyTexture, dirtySpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));

            // Trigger particle effect
            if (paintParticleSystem != null)
            {
                paintParticleSystem.transform.position = worldPoint;
                paintParticleSystem.Play();
            }

            // Mark progress as dirty to trigger an update
            isProgressDirty = true;
        }
    }

    private void UpdateProgress()
    {
        if (totalCleanPixels > 0)
        {
            float currentFillAmount = (float)restoredPixels / totalCleanPixels;
            progressImage.fillAmount = currentFillAmount;

            Debug.Log($"Current Fill Amount: {currentFillAmount * 100}%");
            Debug.Log($"Restored Pixels: {restoredPixels}, Total Clean Pixels: {totalCleanPixels}");

            // Check if the progress image is filled at the calculated threshold
            if (currentFillAmount >= 1.0f)
            {
                FinishGame();
            }
        }
    }

    private IEnumerator UpdateProgressRoutine()
    {
        while (true)
        {
            if (isProgressDirty)
            {
                UpdateProgress();
                isProgressDirty = false;
            }
            yield return new WaitForSeconds(0.1f); // Update every 0.1 seconds
        }
    }

    private IEnumerator CalculateTotalDirtyPixels()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        for (int x = 0; x < dirtyTexture.width; x++)
        {
            for (int y = 0; y < dirtyTexture.height; y++)
            {
                Vector2 worldPoint = dirtySpriteRenderer.transform.TransformPoint(new Vector2((x - dirtyTexture.width / 2f) / dirtyTexture.width, (y - dirtyTexture.height / 2f) / dirtyTexture.height));
                if (collider.OverlapPoint(worldPoint))
                {
                    totalDirtyPixels++;
                }
            }
            yield return null; // Yield control back to the main thread to avoid frame drops
        }

        Debug.Log($"Total Dirty Pixels: {totalDirtyPixels}");

        StartCoroutine(UpdateProgressRoutine());
    }

    private void StartGame()
    {
        // Disable the info panel
        infoPanel.SetActive(false);
        virtualMouse.SetActive(false);

        // Enable interaction
        isInteractable = true;
    }

    private void DeactivateOtherCursor()
    {
        if (otherCursor != null)
        {
            otherCursor.SetActive(false);
        }
    }

    private void FinishGame()
    {
       endGamePanel.SetActive(true);
       progressImage.fillAmount=0;
       virtualMouse.SetActive(true);
       this.gameObject.SetActive(false);
    }
}
