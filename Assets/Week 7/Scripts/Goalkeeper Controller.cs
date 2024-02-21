using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class GoalkeeperController : MonoBehaviour
{
    public float goalKeeperSpeed = 0.05f;
    public Rigidbody2D goalkeeper;
    private Transform centerLine;
    public float goalkeeperDistance;

    private void Start()
    {
        centerLine = transform;
    }

    public void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        if (Controller.selectedPlayer != null) direction = Controller.selectedPlayer.transform.position - centerLine.position;
        if (direction.magnitude > goalkeeperDistance*2) 
        {
            goalkeeper.position = Vector2.MoveTowards
                (goalkeeper.position,(Vector2)centerLine.position + direction.normalized * goalkeeperDistance, goalKeeperSpeed);
        }
        else
        {
            goalkeeper.position = Vector2.MoveTowards
                (goalkeeper.position, (Vector2)centerLine.position + direction / 2, goalKeeperSpeed);
        }
        
    }
}
