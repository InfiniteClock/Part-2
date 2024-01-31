using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject plane;
    public float timer = 0;
    public float timerTarget = 0;


    // Update is called once per frame
    void Update()
    {
        if (timer < timerTarget)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Instantiate(plane);
            timer = 0;
            timerTarget = Random.Range(1, 5);
        }
    }
}
