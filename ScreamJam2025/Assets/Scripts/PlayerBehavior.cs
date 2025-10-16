using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerBehavior : MonoBehaviour
{
    enum Movement
    {
        Hidden,
        Walking,
        Sprinting,
        Stalling
    }

    [SerializeField]
    Slider staminaBar;
    
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

    public float stamina;

    bool secondPassed;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stamina = 200;
        movePhase = Movement.Walking;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movePhase == Movement.Sprinting && stamina == 0) // Checks if player is stalling out
        {
            movePhase = Movement.Stalling;
        }

        switch (movePhase)
        {
            case Movement.Walking: // Player is walking
                moveSpeed = 5.0f;
                stamina += 1;
                break;

            case Movement.Stalling: // Player is trying to sprint with no stamina
                moveSpeed = 5.0f;
                break;

            case Movement.Sprinting: // Player is sprinting
                moveSpeed = 8.0f;
                if (moveDirection != Vector2.zero)
                {
                    stamina -= 2;
                }
                break;

            case Movement.Hidden: // Player is hiding
                moveSpeed = 0;
                if (secondPassed)
                stamina += 5;
                break;
        }

        velocity = moveSpeed * moveDirection * Time.fixedDeltaTime;
        position = rb.position + velocity;

        staminaBar.value = stamina / 200; // Sets stamina bar

        if (stamina > 200) // Caps stamina upper and lower limit
        {
            stamina = 200;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }

        rb.MovePosition(position); // Moves player
    }

    public void OnMove(InputAction.CallbackContext context) // Movement vector
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void StartSprinting(InputAction.CallbackContext context) // Sprinting input
    {
        if (context.performed)
        {
            movePhase = Movement.Sprinting;
        }

        if (context.canceled)
        {
            movePhase = Movement.Walking;
        }
    }
}
