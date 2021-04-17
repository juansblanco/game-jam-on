using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShield(float shield)
    {
        slider.value = (int)shield;
        Debug.Log("Cambio vida a " + (int)shield);
    }

    public void SetMaxShield(int maxShield)
    {
        slider.maxValue = maxShield;
    }
}
