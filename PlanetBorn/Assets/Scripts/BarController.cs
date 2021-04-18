using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    public BarType barType;
    

    public enum BarType
    {
        HEALTH,
        SHIELD,
        PUSH,
        PULL,
        HOOK,
        BARRIER
    }
    public void SetValue(float val)
    {
        slider.value = (int)val;
        Debug.Log("Cambio valor a " + (int)val);
    }

    public void SetMaxValue(int maxVal)
    {
        slider.maxValue = maxVal;
    }

    public BarType getType()
    {
        return barType;
    }

    public void setType(BarType type)
    {
        barType = type;
    }
    
    
    
}
