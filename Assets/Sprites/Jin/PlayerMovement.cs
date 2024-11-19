using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float maxYPosition = 0f; // Maximum Y position the player can reach
    public float minYPosition = -2f; // Minimum Y position (ground level)
    public float maxXPosition = 10f; // Maximum X position the player can reach
    public float minXPosition = -10f; // Minimum X position (leftmost boundary)

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get input for horizontal (left/right) and vertical (up/down) movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Combine into a Vector2
        movement = new Vector2(moveX, moveY).normalized; // Normalized for consistent speed

        // Set Animator parameter
        animator.SetBool("isMoving", movement.magnitude > 0);

        // Flip sprite based on movement direction
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0; // Flip the sprite when moving left
        }
    }

    void FixedUpdate()
    {
        // Calculate the new velocity
        Vector2 newVelocity = movement * speed;

        // Get the current position
        Vector2 currentPosition = rb.position;

        // Clamp the X and Y positions
        float clampedX = Mathf.Clamp(currentPosition.x + newVelocity.x * Time.fixedDeltaTime, minXPosition, maxXPosition);
        float clampedY = Mathf.Clamp(currentPosition.y + newVelocity.y * Time.fixedDeltaTime, minYPosition, maxYPosition);

        // Apply the clamped velocity to the Rigidbody
        rb.velocity = new Vector2((clampedX - currentPosition.x) / Time.fixedDeltaTime, (clampedY - currentPosition.y) / Time.fixedDeltaTime);
    }
}
