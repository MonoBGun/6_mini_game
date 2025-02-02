using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingMouseData : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput=Input.GetAxis("JoystickHorizontal");
        float verticalInput=Input.GetAxis("JoystickVertical");

        Vector3 movementDirection=new Vector3(horizontalInput,verticalInput,0);

        transform.Translate(movementDirection*speed*Time.deltaTime);
    }
}
