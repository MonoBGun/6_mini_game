using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public float speed = 5.0f; // Speed of cursor movement
    private Camera mainCamera;
    private GameObject grabbedObject = null;
    private int uiLayer;
    private int interactableUILayer;

    private void Awake()
    {
        mainCamera = Camera.main;
        uiLayer = LayerMask.NameToLayer("UI");
        interactableUILayer = LayerMask.NameToLayer("InteractableUI");
    }

    private void Update()
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

        // Handle grabbing the Shampoo object
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            TryInteract();
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button0) && grabbedObject != null)
        {
            ReleaseObject();
        }

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = transform.position;
        }
    }

    private Vector3 ClampPositionToScreenBounds(Vector3 position)
    {
        Vector3 clampedPosition = position;

        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, mainCamera.transform.position.x - horizontalExtent, mainCamera.transform.position.x + horizontalExtent);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, mainCamera.transform.position.y - verticalExtent, mainCamera.transform.position.y + verticalExtent);

        return clampedPosition;
    }

    private void TryInteract()
    {
        // Check if cursor is over an interactable UI element
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = mainCamera.WorldToScreenPoint(transform.position)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == interactableUILayer)
            {
                // Check if the result game object has a Button component
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null && result.gameObject.CompareTag("Soap"))
                {
                    button.onClick.Invoke();
                    return;
                }
            }
        }

        // If no interactable UI element, try to grab a game object
        Ray ray = mainCamera.ScreenPointToRay(mainCamera.WorldToScreenPoint(transform.position));
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, ~(1 << uiLayer));

        if (hit.collider != null && hit.collider.CompareTag("Shampoo"))
        {
            grabbedObject = hit.collider.gameObject;
        }
    }

    private void ReleaseObject()
    {
        grabbedObject = null;
    }
}
