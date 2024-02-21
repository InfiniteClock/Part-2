using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color selectedCol;
    public float speed = 100;
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
        Controller.SelectPlayer(this);
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
    public void Move(Vector2 direction)
    {
        float angle = Mathf.Atan2 (direction.x, direction.y) * Mathf.Rad2Deg;
        rb.SetRotation(-angle);
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
