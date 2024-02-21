using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform kickOff;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (kickOff == null)
        {
            kickOff = transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Controller.score++;
        rb.MovePosition(kickOff.position);
        rb.velocity = Vector2.zero;
    }
}
