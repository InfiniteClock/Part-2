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

        Vector3 spawn = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
        spawn.z = 0;
        float angle = Mathf.Atan2(-spawn.x, -spawn.y) * Mathf.Rad2Deg;
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
