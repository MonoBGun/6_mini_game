using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeButtonSound : MonoBehaviour
{
    public AudioClip buttonFX;
    public void ButtonSound(){
        SoundFXManager.instance.PlaySoundFXClip(buttonFX,transform,1f);
    }
}