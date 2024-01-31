using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runway : MonoBehaviour
{
    public int score = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.OverlapPoint(collision.transform.position))
        {
            collision.GetComponent<Plane>().land = true;
            score += 1;
        }
    }
}
