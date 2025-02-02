using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class HandsScript : MonoBehaviour
{
    public VirtualMouseInput.CursorMode cursorMode {get;set;}
    Vector2 Direction;

    public float force;

    public GameObject PointsPrefab;

    public GameObject[] Points;

    public int numberOfPoints;
    public GameObject virtualmouse;
    // Start is called before the first frame update
    void Start()
    {
        Points=new GameObject[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            Points[i]=Instantiate(PointsPrefab,transform.position,Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseposset=new Vector2(virtualmouse.transform.position.x*100,virtualmouse.transform.position.y*100);

        Vector2 MousePos= Camera.main.ScreenToWorldPoint(mouseposset);

        Vector2 handsPos= transform.position;

        Direction=MousePos-handsPos;

        faceMouse();

        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position=PointsPosition(i*0.1f);
        }
    }

    void faceMouse(){
        transform.right=Direction;
    }

    Vector2 PointsPosition(float t){
        Vector2 currentPos=(Vector2)transform.position+(Direction.normalized*force*t)+0.5f*Physics2D.gravity*(t*t);
        return currentPos;
    }
}
