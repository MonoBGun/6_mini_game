using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private AudioClip swissSound;
    BallGameManager ballGameManager;
    PointSpawner spawn;
    void Start(){
        spawn=GameObject.FindGameObjectWithTag("Spawner").GetComponent<PointSpawner>();
    }
        
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Ball"){
        SoundFXManager.instance.PlaySoundFXClip(swissSound,transform,1f);
        spawn.checkIfHit=false;
        Destroy(this.gameObject);
    }
    }
    
}