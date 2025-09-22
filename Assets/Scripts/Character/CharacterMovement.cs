using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    public Joystick joystick;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        joystick = FindAnyObjectByType<Joystick>();
    }

    void Update()
    {
        // Joystick
        if (joystick != null)
        {
            movement.x = joystick.Horizontal();
            movement.y = joystick.Vertical();
        }
        else
        {
            // PC
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        movement = movement.normalized;

        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isMoving", isMoving);

        if (movement.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
