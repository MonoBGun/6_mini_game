using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{

    public GameObject Ball;
    public float LaunchForce;
    public bool shootcooldown=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button0)&&shootcooldown==true){
            StartCoroutine(Shoot());
            gameObject.GetComponent<SpriteRenderer>().enabled=false;
        }
    }
    IEnumerator Shoot(){
        GameObject BallIns=Instantiate(Ball,transform.position,transform.rotation);
        BallIns.GetComponent<Rigidbody2D>().velocity=transform.right*LaunchForce;
        shootcooldown=false;
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<SpriteRenderer>().enabled=true;
        yield return new WaitForSeconds(1);
        shootcooldown=true;
    }
}
