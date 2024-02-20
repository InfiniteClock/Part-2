using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statbars : MonoBehaviour
{
    public Slider health;
    public Slider speed;

    public void StartHealth(float value)
    {
        health.maxValue = value;
    }
    public void SetHealth(float value)
    {
        health.value = value;
    }
    public void Damage(float value)
    {
        health.value -= value;
    }

    public void StartSpeed(float value)
    {
        speed.maxValue = value;
    }
    public void SetSpeed(float value)
    {
        speed.value = value;
    }
}
