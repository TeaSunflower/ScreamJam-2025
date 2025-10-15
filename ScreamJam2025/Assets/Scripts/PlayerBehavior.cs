using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    enum Movement
    {
        Hidden,
        Walking,
        Sprinting
    }
    
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    Vector2 moveDirection;

    [SerializeField]
    Vector2 position;

    [SerializeField]
    Vector2 velocity;

    [SerializeField]
    Movement movePhase;

    float stamina;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePhase = Movement.Walking;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (movePhase)
        {
            case Movement.Walking:
                moveSpeed = 5.0f;
                break;

            case Movement.Sprinting:
                moveSpeed = 8.0f;
                break;

            case Movement.Hidden:
                moveSpeed = 0;
                break;
        }

        velocity = moveSpeed * moveDirection * Time.fixedDeltaTime;
        position = rb.position + velocity;

        rb.MovePosition(rb.position + velocity);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void StartSprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movePhase = Movement.Sprinting;
        }
        else
        {
            movePhase = Movement.Walking;
        }
    }
}
