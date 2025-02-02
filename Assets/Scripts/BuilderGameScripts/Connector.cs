using UnityEngine;

public class Connector : MonoBehaviour
{
    public bool isConnected = false;
    private GameManager gameManager;
    public AudioClip connectionSound;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log($"Connector {name} initialized. Initial isConnected: {isConnected}. GameManager found: {gameManager != null}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D called for {name} with {other.name}");

        if (other.CompareTag("PlankSide"))
        {
            if (!isConnected)
            {
                GameObject sound=GameObject.FindGameObjectWithTag("SoundEffectObject");
                if (sound==null)
                {
                    SoundFXManager.instance.PlaySoundFXClip(connectionSound,transform,1f);
                }
                isConnected = true;
                gameManager?.IncrementConnectionCount();
                Debug.Log($"Connected: {name} with {other.name}. Connection Count: {gameManager?.connectionint}");
            }
            else
            {
                Debug.Log($"{name} is already connected.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"OnTriggerExit2D called for {name} with {other.name}");

        if (other.CompareTag("PlankSide"))
        {
            if (isConnected)
            {
                isConnected = false;
                gameManager?.DecrementConnectionCount();
                Debug.Log($"Disconnected: {name} from {other.name}. Connection Count: {gameManager?.connectionint}");
            }
            else
            {
                Debug.Log($"{name} is already disconnected.");
            }
        }
    }
}
