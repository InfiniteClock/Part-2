using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color selectedCol;
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private Color baseColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        baseColor = spr.color;
        Selected(false);
    }
    private void OnMouseDown()
    {
        Selected(true);
    }
    private void OnMouseUp()
    {
        Selected(false);
    }
    public void Selected(bool isSelected)
    {
        if (isSelected)
        {
            spr.color = selectedCol;
        }
        else
        {
            spr.color = baseColor;
        }
    }
}
