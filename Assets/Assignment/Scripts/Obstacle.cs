using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Obstacle : MonoBehaviour
{
    public float speed;
    public float damage;
    public float lifetime;
    public float rotationSpeed;

    public Transform sprite;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Sets random direction and magnitude of spin to obstacle
        rotationSpeed *= Random.Range(-1f, 1f);

        // Set the spawn point to be along the borders outside of the camera view
        Vector2 spawn = new Vector2(-10f,-10f); // Default outside of map in far bottom left corner
        int side = Random.Range(1, 4);
        if (side == 1)  //Top side
        {
            spawn = new Vector2(Random.Range(-10f, 10f), 6f);
        }
        if (side == 2)  //Bot side
        {
            spawn = new Vector2(Random.Range(-10f, 10f), -6f);
        }
        if (side == 3)  //Left side
        {
            spawn = new Vector2(-10f, Random.Range(-6f, 6f));
        }
        if (side == 4)  //Right side
        {
            spawn = new Vector2(10f, Random.Range(-6f, 6f));
        }
        rb.position = spawn;

        // Sets direction towards center screen, +/- 30 degrees rotation
        float angle = Mathf.Atan2(-spawn.x, -spawn.y) * Mathf.Rad2Deg;
        angle += Random.Range(-30f, 30f);
        rb.SetRotation(-angle);
    }
    private void FixedUpdate()
    {
        // Set the obstacle to 
        rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.deltaTime);
        Vector3 angle = new Vector3(0,0,transform.rotation.z * rotationSpeed * Time.deltaTime);
        sprite.Rotate(angle);
    }
    
    private void OnBecameInvisible()
    {
        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score")+1);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
