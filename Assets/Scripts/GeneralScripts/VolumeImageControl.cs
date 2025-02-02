using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeImageControl : MonoBehaviour
{
    public enum VolumeType { Master, Music, SFX }
    public VolumeType volumeType;

    private Image fillImage;

    private void Start()
    {
        fillImage = GetComponent<Image>();

        // Load initial value from PlayerPrefs
        float initialVolume = 1.0f;
        switch (volumeType)
        {
            case VolumeType.Master:
                initialVolume = PlayerPrefs.HasKey(AudioManager.Instance.masterVolumeParameter) ?
                                PlayerPrefs.GetFloat(AudioManager.Instance.masterVolumeParameter) : 1.0f;
                break;
            case VolumeType.Music:
                initialVolume = PlayerPrefs.HasKey(AudioManager.Instance.musicVolumeParameter) ?
                                PlayerPrefs.GetFloat(AudioManager.Instance.musicVolumeParameter) : 1.0f;
                break;
            case VolumeType.SFX:
                initialVolume = PlayerPrefs.HasKey(AudioManager.Instance.sfxVolumeParameter) ?
                                PlayerPrefs.GetFloat(AudioManager.Instance.sfxVolumeParameter) : 1.0f;
                break;
        }
        fillImage.fillAmount = initialVolume;

        // Add listeners for click and drag events to adjust the fill amount
        AddEventListeners();
    }

    private void AddEventListeners()
    {
        // Add event listeners to handle user interaction with the fill image
        // For example, use EventTrigger to handle drag events
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Drag
        };
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDown(PointerEventData data)
    {
        UpdateFillAmount(data);
    }

    public void OnDrag(PointerEventData data)
    {
        UpdateFillAmount(data);
    }

    private void UpdateFillAmount(PointerEventData data)
    {
        RectTransform rectTransform = fillImage.GetComponent<RectTransform>();
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out localPoint))
        {
            float newFillAmount = Mathf.InverseLerp(rectTransform.rect.xMin, rectTransform.rect.xMax, localPoint.x);
            fillImage.fillAmount = newFillAmount;
            OnFillAmountChanged(newFillAmount);
        }
    }

    public void IncreaseFillAmount()
    {
        UpdateFillAmountByAmount(0.1f);
    }

    public void DecreaseFillAmount()
    {
        UpdateFillAmountByAmount(-0.1f);
    }

    private void UpdateFillAmountByAmount(float amount)
    {
        fillImage.fillAmount = Mathf.Clamp(fillImage.fillAmount + amount, 0f, 1f);
        OnFillAmountChanged(fillImage.fillAmount);
    }

    private void OnFillAmountChanged(float newFillAmount)
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioManager.Instance.SetMasterVolume(newFillAmount);
                break;
            case VolumeType.Music:
                AudioManager.Instance.SetMusicVolume(newFillAmount);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SetSFXVolume(newFillAmount);
                break;
        }
    }
}
