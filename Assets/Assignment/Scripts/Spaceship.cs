using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public float maxHealth = 5f;            // Maximum health of ship
    public float health;                    // Current health of ship
    public float speed;                     // Speed limit of ship (Orthogonally only)
    public float distThreshold = 0.1f;      // Threshold distance to target
    public float hurtTimer;                 // Maximum time to be invincible after hurt
    public AnimationCurve curve;

    private bool thrust;                    // determines if moving towards target or not
    private bool isDead;                    // determines if health above 0
    private bool isHurt;                    // determines if recently hurt 
    private float timer;                    // Timer for invincibility when hurt
    private float yVelocity;                // Vertical momentum
    private float xVelocity;                // Horizontal momentum
    public float rotation;                 // Rotation of ship's movement
    private float interpolationTimer;       // Timer for interpolation
    public Vector2 movement;               // Momentum of ship
    private Vector2 target;                 // Position of target destination
    private Animator animator;
    private Rigidbody2D rb;
    private LineRenderer lr;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        // Set line renderer to only have one point at self
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);

        if (PlayerPrefs.GetFloat("health", maxHealth) > 0)
        {
            health = PlayerPrefs.GetFloat("health", maxHealth);
        }
        else { health = maxHealth; }
        transform.position = new Vector3(PlayerPrefs.GetFloat("playerX", 0), (PlayerPrefs.GetFloat("playerY", 0)), 0);

        SendMessage("StartHealth",maxHealth, SendMessageOptions.DontRequireReceiver);
        SendMessage("SetHealth", health, SendMessageOptions.DontRequireReceiver);
        SendMessage("StartSpeed", (Mathf.Sqrt(speed*speed+speed*speed)), SendMessageOptions.DontRequireReceiver);
        SendMessage("SetSpeed", 0, SendMessageOptions.DontRequireReceiver);

    }

    private void FixedUpdate()
    {
        // If target is outside threshold for distance, interpolate the current velocity of the ship to the target direction
        if (Vector2.Distance(target, transform.position) > distThreshold)
        {
            // Timer for the Lerp
            if (interpolationTimer < 1.0f) 
            {
                interpolationTimer += 0.05f * Time.deltaTime;
            }
            // Collect the interpolation from the animation curve, then get a direction to target.
            // Lerp the current x and y velocities of the ship between existing velocity and direction by interpolation
            float interpolation = curve.Evaluate(interpolationTimer);
            Vector2 direction = target - (Vector2)transform.position;
            xVelocity = Mathf.Lerp(xVelocity, direction.x, interpolation);
            yVelocity = Mathf.Lerp(yVelocity, direction.y, interpolation);
            thrust = true;
        }
        // Else reduce the x and y velocities very slowly 
        else
        {
            if (xVelocity > 0.1f) xVelocity -= 0.1f * Time.deltaTime;
            else if (xVelocity < -0.1f) xVelocity += 0.1f * Time.deltaTime;
            else xVelocity = 0f;

            if (yVelocity > 0.1f) yVelocity -= 0.1f * Time.deltaTime;
            else if (yVelocity < -0.1f) yVelocity += 0.1f * Time.deltaTime;
            else yVelocity = 0f;
            thrust = false;
        }
        // Clamp the velocities to fit within speed limits
        // Diagonal speed can exceed this limit currently
        Mathf.Clamp(xVelocity, -speed, speed);
        Mathf.Clamp(-yVelocity, -speed, speed);

        // Movement vector holds current velocity of the ship
        movement = new Vector2(xVelocity, yVelocity);
        rb.MovePosition(rb.position + movement * Time.deltaTime);

        // Rotation holds current angle of motion. Only changes while thrusting
        if (thrust)
        {
            rotation = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            rb.SetRotation(-rotation); 
        }
    }

    private void Update()
    {
        if (isDead)
        {
            lr.positionCount = 0;
            transform.localScale = new Vector3(transform.localScale.x - 0.1f * Time.deltaTime, 
                transform.localScale.y - 0.1f * Time.deltaTime, transform.localScale.z);
            return;
        }

        // Controls the invincibility timer
        if (timer > 0f) timer -= Time.deltaTime;
        else isHurt = false;

        SendMessage("SetSpeed", movement.magnitude, SendMessageOptions.DontRequireReceiver);

        // Collects mouse inputs on screen and gets a target vector from that. Also resets timer for Lerp in speed changes
        if (Input.GetMouseButtonDown(0) /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            interpolationTimer = 0;
        }

        // Sets the line renderer to display line between self and destination, or not at all if reached destination already
        lr.SetPosition(0, transform.position);
        if (Vector2.Distance(transform.position,target) < distThreshold)
        {
            target = transform.position;
            lr.positionCount = 1;
        }
        else
        {
            lr.positionCount = 2;
            lr.SetPosition(1, target);
        }

        // Controls the animations used for thrust or idle
        if (thrust) animator.SetFloat("thrust", movement.magnitude);
        else animator.SetFloat("thrust", 0);

        // Controls the Blend Tree for direction of Thrust using angle of direction and angle of self
        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float angleSelf = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        float rot = angleSelf - angle;
        // Loops the rotation over the 180/-180 axis
        if (rot > 180) rot -= 360;
        if (rot < -180) rot += 360;
        animator.SetFloat("rotation", -rot);

        // Kills the ship at 0 health
        if (health <= 0 )
        {
            Death();
        }

        // Temporary damage dealer
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isHurt) return;
        isHurt = true;
        timer = hurtTimer;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health > 0) animator.SetTrigger("takeDamage");
        PlayerPrefs.SetFloat("health", health);
        SendMessage("Damage", 1, SendMessageOptions.DontRequireReceiver);
    }
    public void Death()
    {
        isDead = true;
        animator.SetTrigger("death");
        PlayerPrefs.SetFloat("health", maxHealth);
        Destroy(gameObject, 3f);
    }

}
