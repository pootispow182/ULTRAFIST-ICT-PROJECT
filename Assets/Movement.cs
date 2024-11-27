using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float maxYPosition = 0f; // Maximum Y position the player can reach
    public float minYPosition = -2f; // Minimum Y position (ground level)

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isPunching = false; // Check if the player is currently punching

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

        // Check for punch input (e.g., spacebar or mouse click)
        if (Input.GetKeyDown(KeyCode.Space) && !isPunching)
        {
            Punch();
        }
    }

    void FixedUpdate()
    {
        // Stop movement while punching
        if (isPunching)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Calculate the new velocity
        Vector2 newVelocity = movement * speed;

        // Get the current position
        Vector2 currentPosition = rb.position;

        // Clamp the Y position
        float clampedY = Mathf.Clamp(currentPosition.y + newVelocity.y * Time.fixedDeltaTime, minYPosition, maxYPosition);

        // Apply the clamped velocity to the Rigidbody
        rb.velocity = new Vector2(newVelocity.x, (clampedY - currentPosition.y) / Time.fixedDeltaTime);
    }

    void Punch()
    {
        // Trigger the punch animation
        animator.SetTrigger("Punch");
        isPunching = true;

        // Optional: Delay ending the punch (until the animation finishes)
        StartCoroutine(EndPunch());
    }

    IEnumerator EndPunch()
    {
        // Wait for the animation to finish (adjust the time to match your animation)
        yield return new WaitForSeconds(0.5f);

        isPunching = false;
    }
}
