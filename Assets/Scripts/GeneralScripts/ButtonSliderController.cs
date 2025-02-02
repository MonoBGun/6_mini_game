using UnityEngine;
using UnityEngine.UI;

public class ButtonSliderController : MonoBehaviour
{
    public Slider slider; // Reference to the slider you want to control
    public float incrementAmount = 0.1f; // Amount to increment the slider value by

    // Call this method when the increment button is clicked
    public void OnIncrementButtonClick()
    {
        if (slider != null)
        {
            slider.value += incrementAmount;
        }
    }

    // Call this method when the decrement button is clicked
    public void OnDecrementButtonClick()
    {
        if (slider != null)
        {
            slider.value -= incrementAmount;
        }
    }
}
