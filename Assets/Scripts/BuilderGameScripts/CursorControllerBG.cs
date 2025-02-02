using UnityEngine;

public class CursorControllerBG : MonoBehaviour
{
    public float speed = 5.0f; // Speed of cursor movement
    private Camera mainCamera;
    private GameObject grabbedObject = null;
    private PlankController grabbedPlankController = null;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get joystick input
        float moveHorizontal = Input.GetAxis("JoystickHorizontal");
        float moveVertical = Input.GetAxis("JoystickVertical");

        // Calculate new position
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        Vector3 newPos = transform.position + movement * Time.deltaTime * speed;

        // Clamp the cursor position to the screen bounds
        newPos = ClampPositionToScreenBounds(newPos);

        // Update the cursor position
        transform.position = newPos;

        // Handle grabbing and moving planks
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            TryGrabPlank();
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button0) && grabbedPlankController != null)
        {
            ReleasePlank();
        }

        if (grabbedPlankController != null)
        {
            grabbedPlankController.Drag(transform.position); // Pass cursor position here
        }

        // Handle rotation
        HandleRotation();
    }

    Vector3 ClampPositionToScreenBounds(Vector3 position)
    {
        Vector3 clampedPosition = position;

        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, mainCamera.transform.position.x - horizontalExtent, mainCamera.transform.position.x + horizontalExtent);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, mainCamera.transform.position.y - verticalExtent, mainCamera.transform.position.y + verticalExtent);

        return clampedPosition;
    }

    void TryGrabPlank()
    {
        // Raycast to detect plank
        Vector2 cursorPosition = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Plank"))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedPlankController = grabbedObject.GetComponent<PlankController>();
            if (grabbedPlankController != null)
            {
                grabbedPlankController.StartDragging(transform.position);
                Debug.Log("Plank grabbed");
            }
        }
    }

    void ReleasePlank()
    {
        if (grabbedPlankController != null)
        {
            grabbedPlankController.StopDragging();
            grabbedPlankController = null;
            Debug.Log("Plank released");
        }
        grabbedObject = null;
    }

    void HandleRotation()
    {
        if (grabbedPlankController != null)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                grabbedPlankController.transform.Rotate(Vector3.forward, GameManager.rotationAmount);
                Debug.Log($"Rotated clockwise by {GameManager.rotationAmount} degrees: " + grabbedPlankController.transform.rotation.eulerAngles.z);
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                grabbedPlankController.transform.Rotate(Vector3.forward, -GameManager.rotationAmount);
                Debug.Log($"Rotated counterclockwise by {GameManager.rotationAmount} degrees: " + grabbedPlankController.transform.rotation.eulerAngles.z);
            }
        }
    }
}
