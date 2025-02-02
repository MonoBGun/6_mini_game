using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInScreen : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    public GameObject backGroundImage;
    void Start()
    {
        Vector2 deviceScreenResolution=new Vector2(Screen.width,Screen.height);
        float srcHeight=Screen.height;
        float srcWidth=Screen.width;
        float DEVICE_SCREEN_ASPECT=srcWidth/srcHeight;
        mainCamera.aspect=DEVICE_SCREEN_ASPECT;
        float camHeight=100.0f*mainCamera.orthographicSize*2.0f;
        float camWidth=camHeight*DEVICE_SCREEN_ASPECT;
        SpriteRenderer backGroundImageSR=backGroundImage.GetComponent<SpriteRenderer>();
        float bgImgH=backGroundImageSR.sprite.rect.height;
        float bgImgW=backGroundImageSR.sprite.rect.width;
        float bgImg_scale_ratio_height=camHeight/bgImgH;
        float bgImg_scale_ratio_Width=camWidth/bgImgW;
        backGroundImage.transform.localScale=new Vector3(bgImg_scale_ratio_Width,bgImg_scale_ratio_height,1);
    }
}
