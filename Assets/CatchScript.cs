using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchscript : MonoBehaviour
{
    public LogicScript logic;
    public GameObject leftBorder, rightBorder;
    public CircleCollider2D catcher;
    public Rigidbody2D hardbody;
    public float baseSpeed = 1f * 10 ;  // Initial speed
    private float currentSpeed;  // Tracks the current speed
    private float speedIncreaseRate = 0.1f;

    private float minX, maxX;

    void Start()
    {
        // Initialize the current speed to the base speed
        currentSpeed = baseSpeed;
        //get logic
        logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();

        //set border boundary
        GameObject borders = GameObject.FindWithTag("Borders");
        if (borders != null)
        {
            BoxCollider2D borderCollider = borders.GetComponent<BoxCollider2D>();
            minX = borders.transform.position.x - borderCollider.bounds.size.x / 2f;
            maxX = borders.transform.position.x + borderCollider.bounds.size.x / 2f;
        }
    }

    float horizontalInput;
    void Update()
    {
        currentSpeed = baseSpeed + (Time.time * speedIncreaseRate);


        horizontalInput = 0f;

        if (!logic.isGameOver)
        {
            if (Input.GetKey(KeyCode.A))
            {
                    horizontalInput = -2f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                    horizontalInput = 2f;
 
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                horizontalInput *= 2.7f; // Move Faster
            }
        }
        else
        {
            hardbody.bodyType = RigidbodyType2D.Dynamic;
        }

        hardbody.velocity = new Vector2(horizontalInput * currentSpeed, hardbody.velocity.y);

        //clamping to borders
        float clampedX = Mathf.Clamp(hardbody.position.x, minX, maxX);
        hardbody.position = new Vector2(clampedX, hardbody.position.y);

        // Debug log for testing
        //if (horizontalInput != 0)
        //{
        //    Debug.Log(horizontalInput > 0 ? "RIGHT" : "LEFT");
        //}


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter triggered");

        if (collision.gameObject.name == "LeftBorder")
        {
            Debug.Log("HITLEFTBORDER");
            horizontalInput = 2f;
        }
        if(collision.gameObject.name == "RightBorder")
        {
            Debug.Log("HITRIGHTBORDER");
            horizontalInput = -2f;
        }
    }

}
