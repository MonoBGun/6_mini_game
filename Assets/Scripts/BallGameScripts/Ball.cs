using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    [SerializeField] private AudioClip bounce;

    [HideInInspector]public Vector3 pos {get{return transform.position;}}
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Push(Vector3 force){
        rb.AddForce(force,ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag=="Ground")
        {
            SoundFXManager.instance.PlaySoundFXClip(bounce,col.gameObject.transform,1f);
        }
    }
}
