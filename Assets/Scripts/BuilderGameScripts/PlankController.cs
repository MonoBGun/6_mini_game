using UnityEngine;

public class PlankController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isDragging)
        {
            Drag(transform.position);
        }
    }

    public void StartDragging(Vector3 cursorPosition)
    {
        offset = transform.position - cursorPosition;
        isDragging = true;
        Debug.Log("Plank grabbed");
    }

    public void StopDragging()
    {
        isDragging = false;
        Debug.Log("Plank released");
    }

    public void Drag(Vector3 cursorPosition)
    {
        transform.position = cursorPosition + offset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            RegisterConnection(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            UnregisterConnection(other);
        }
    }

    void RegisterConnection(Collider2D other)
    {
        Debug.Log($"Connected: {name} with {other.name}");
    }

    void UnregisterConnection(Collider2D other)
    {
        Debug.Log($"Disconnected: {name} from {other.name}");
    }
}
