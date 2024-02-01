using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runway : MonoBehaviour
{
    public int score = 0;

    private void OnTriggerExit2D(Collider2D collision)
    {
        score += 1;
    }
}
