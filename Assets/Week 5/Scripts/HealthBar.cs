using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void TakeDamage(float damage)
    {
        slider.value -= damage;
    }
    public void SetSliderMax(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }
    public void SetSliderCurrent(float value)
    {
        slider.value = value;
    }
    public void Heal()
    {
        slider.value = slider.maxValue;
    }
}
