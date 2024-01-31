using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public List<Sprite> sprites;
    public AnimationCurve landing;
    float landingTimer;
    public List<Vector2> points;
    Vector2 lastPosition;
    Vector2 currentPos;
    public float pointThreshold = 0.2f;
    public float speed = 1f;
    public float tooClose = 0.7f;
    LineRenderer lineRenderer;
    Rigidbody2D rb;
    SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = sprites[Random.Range(0, sprites.Count)];

        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);


        speed = Random.Range(1f, 3f);
        Vector2 spawn = new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
        float angle = Mathf.Atan2(-spawn.x, -spawn.y) * Mathf.Rad2Deg;

        // Gives plane a random direction within 45 degrees cone towards center screen
        // This ensures that the plane always moves onto the screen
        angle += Random.Range(-45, 45); 
        rb.SetRotation(-angle);
        transform.position = spawn;
    }
    
    private void FixedUpdate()
    {
        currentPos = transform.position;
        if(points.Count > 0)
        {
            Vector2 direction = points[0] - currentPos;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            rb.SetRotation(-angle);
        }
        rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.deltaTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            landingTimer += 0.2f * Time.deltaTime;
            float interpolation = landing.Evaluate(landingTimer);

            if(transform.localScale.z < 0.2)
            {
                Destroy(gameObject);
            }
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, interpolation);
        }

        lineRenderer.SetPosition(0, transform.position);
        if (points.Count > 0)
        {
            if (Vector2.Distance(currentPos, points[0]) < pointThreshold)
            {
                points.RemoveAt(0);

                for (int i = 0; i < lineRenderer.positionCount - 2; i++)
                {
                    lineRenderer.SetPosition(i, lineRenderer.GetPosition(i+1));
                }
                if(lineRenderer.positionCount != 0) lineRenderer.positionCount--;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        spr.color = Color.red;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        float distance = Vector3.Distance(transform.position, collision.transform.position);
        if (distance <= tooClose)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spr.color = Color.white;
    }
    private void OnMouseDown()
    {
        points = new List<Vector2>();
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        points.Add(newPosition);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
    }
    private void OnMouseDrag()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(lastPosition, newPosition) > pointThreshold)
        {
            points.Add(newPosition);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
            lastPosition = newPosition;
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
