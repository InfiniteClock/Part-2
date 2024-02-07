using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public float speed = 3;
    public Animator animator;
    public Rigidbody2D rb;
    public float maxHealth = 5;
    public float health;
    public float invinceTime = 1;

    private bool isDead = false;
    private float timer;
    private bool selfClick = false;
    private Vector2 destination;
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        SendMessage("SetSlider", maxHealth, SendMessageOptions.DontRequireReceiver);
    }

    private void FixedUpdate()
    {
        if(isDead) return;
        movement = destination - (Vector2)transform.position;
        if (movement.magnitude < 0.1)
        {
            movement = Vector2.zero;
        }
        rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        timer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !selfClick)
        {
            destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        animator.SetFloat("movement", movement.magnitude);
        if (health <= 0)
        {
            Death();
        }
    }
    private void OnMouseDown()
    {
        if (isDead) return;
        selfClick = true;
        SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
    }
    private void OnMouseUp() {
        selfClick = false;
    }

    private void TakeDamage(float damage)
    {
        if (isDead) return;
        if (timer <= 0) 
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            animator.SetTrigger("takeDamage");
            timer = invinceTime;
        }
    }
    public void Heal()
    {
        health = maxHealth;
    }
    private void Death()
    {
        isDead = true;
        animator.SetTrigger("dead");
    }
}
