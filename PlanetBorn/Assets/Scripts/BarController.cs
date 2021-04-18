using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;

    public void SetValue(float val)
    {
        slider.value = (int)val;
        Debug.Log("Cambio valor a " + (int)val);
    }

    public void SetMaxValue(int maxVal)
    {
        slider.maxValue = maxVal;
    }
}
