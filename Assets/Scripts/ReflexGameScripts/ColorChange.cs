using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    GameController gameController;
    public int a;
    Color color1= new Color(1,0,0,1);//red
    Color color2= new Color(0,1,0,1);//green
    Color color3= new Color(0,0,1,1);//blue
    Color color4= new Color(1,0.92f,0.016f,1);//yellow
    Color color5=new Color(1,0,1,1);//magenta
    Color[] colors;

    void Awake(){
        colors=new Color[] {Color.red,Color.blue,Color.magenta,Color.yellow,Color.green};
    }

    void Start()
    {
        _spriteRenderer=GetComponent<SpriteRenderer>();
        gameController=GetComponent<GameController>();
    }

    public void ChangeColor(){
        a=UnityEngine.Random.Range(0,5);
        _spriteRenderer.color=colors[a];
    }
    public void ResetColor(){
        _spriteRenderer.color=new Color(1,1,1,1);
    }
}
