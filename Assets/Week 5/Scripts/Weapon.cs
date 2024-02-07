using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float speed = 5;
    public float damage = 1;

    float lifeTime = 5;
    Vector2 direction;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector2(-9, Random.Range(-4f, 4f));
        direction = Vector2.right;
        rb.SetRotation(-Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
    }
    private void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
