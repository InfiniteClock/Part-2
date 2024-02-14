using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knight : MonoBehaviour
{
    public float speed = 3;
    public Animator animator;
    public Rigidbody2D rb;
    public float maxHealth = 5;
    public float health;

    private bool isDead = false;
    private bool selfClick = false;
    private bool attacking = false;
    private Vector2 destination;
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = PlayerPrefs.GetFloat("currentHealth", maxHealth);
        SendMessage("SetSliderMax", maxHealth, SendMessageOptions.DontRequireReceiver);
        SendMessage("SetSliderCurrent", health, SendMessageOptions.DontRequireReceiver);
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
        if (Input.GetMouseButtonDown(0) && !selfClick && !EventSystem.current.IsPointerOverGameObject())
        {
            destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1) && !attacking)
        {
            Attack();
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
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        animator.SetTrigger("takeDamage");
        PlayerPrefs.SetFloat("currentHealth", health);
    }
    public void Heal()
    {
        health = maxHealth;
        if (isDead) { 
            animator.SetTrigger("takeDamage");
            isDead = false;
        }
        PlayerPrefs.SetFloat("currentHealth", health);
    }
    private void Death()
    {
        isDead = true;
        animator.SetTrigger("dead");
    }
    private void Attack()
    {
        animator.SetTrigger("attack");
    }
}
