using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5.0f;

    [SerializeField]
    Vector2 moveDirection;

    [SerializeField]
    Vector2 position;

    [SerializeField]
    Vector2 velocity;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = moveSpeed * moveDirection * Time.fixedDeltaTime;
        position = rb.position + velocity;

        rb.MovePosition(rb.position + velocity);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
}
