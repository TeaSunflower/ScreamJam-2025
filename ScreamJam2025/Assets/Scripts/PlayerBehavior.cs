using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerBehavior : MonoBehaviour
{
    public enum Movement
    {
        Hidden,
        Walking,
        Sprinting,
        Stalling,
        Revealed
    }

    [SerializeField]
    Slider staminaBar;

    [SerializeField]
    Slider susBar;
    
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    Vector2 moveDirection;

    [SerializeField]
    Vector2 position;

    [SerializeField]
    Vector2 velocity;

    [SerializeField]
    public Movement movePhase;

    [SerializeField]
    bool canHide;

    // Maybe add a slider for this?
    public float suspicion;

    public float stamina;

    float timeCounter;

    List<Vector2> hideList;

    Renderer spriteControls;
    BoxCollider2D hitBox;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stamina = 200;
        suspicion = 0;
        timeCounter = 0;
        movePhase = Movement.Walking;

        rb = GetComponent<Rigidbody2D>();
        spriteControls = GetComponent<Renderer>();
        hitBox = GetComponent<BoxCollider2D>();

        spriteControls.enabled = true;
        hitBox.enabled = true;

        hideList = new List<Vector2>();
        hideList.Add(new Vector2(-4.5f, 0.5f));
        hideList.Add(new Vector2(14.5f, 7.5f));
        hideList.Add(new Vector2(7.5f, 16.5f));
        hideList.Add(new Vector2(-30.5f, 16.5f));
        hideList.Add(new Vector2(-11.5f, 10.5f));
        hideList.Add(new Vector2(-25.5f, -12.5f));
        hideList.Add(new Vector2(-2.5f, -7.5f));
        hideList.Add(new Vector2(6.5f, -17.5f));
        hideList.Add(new Vector2(23.5f, -13f));
    }

    void Update()
    {
        if (movePhase == Movement.Sprinting && stamina == 0) // Checks if player is stalling out
        {
            movePhase = Movement.Stalling;
        }

        CheckMovePhase();

        if (stamina > 200) // Caps stamina upper and lower limit
        {
            stamina = 200;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }

        if (suspicion > 100) // Caps suspicion upper and lower limit
        {
            suspicion = 100;
        }
        else if (suspicion < 0)
        {
            suspicion = 0;
        }

        staminaBar.value = stamina / 200; // Sets stamina bar
        susBar.value = suspicion / 100; // Sets suspicion bar

        for (int i = 0; i < hideList.Count; i++)
        {
            if (Vector2.Distance(position, hideList[i]) <= 1)
            {
                canHide = true;
                break;
            }
            else
            {
                canHide = false;
            }
        }

        if (suspicion >= 90 && movePhase == Movement.Hidden)
        {
            movePhase = Movement.Revealed;
            hitBox.enabled = true;
        }

        timeCounter += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = moveSpeed * moveDirection * Time.fixedDeltaTime;
        position = rb.position + velocity;

        rb.MovePosition(position); // Moves player
    }

    private void CheckMovePhase()
    {
        switch (movePhase)
        {
            case Movement.Walking: // Player is walking
                moveSpeed = 5.0f;
                    stamina += 0.5f;
                if (timeCounter >= 0.5)
                {
                    suspicion -= 1;
                    timeCounter = 0;
                }
                break;

            case Movement.Stalling: // Player is trying to sprint with no stamina
                moveSpeed = 5.0f;
                if (timeCounter >= 0.5)
                {
                    suspicion -= 1;
                    timeCounter = 0;
                }
                break;

            case Movement.Sprinting: // Player is sprinting
                moveSpeed = 8.0f;
                if (moveDirection != Vector2.zero)
                {
                    stamina -= 1;
                }
                if (timeCounter >= 0.5)
                {
                    suspicion -= 5;
                    timeCounter = 0;
                }
                break;
                 
            case Movement.Hidden: // Player is hiding
                moveSpeed = 0;
                    stamina += 1;
                if (timeCounter >= 0.5)
                {
                    suspicion += 2;
                    timeCounter = 0;
                }
                break;

            case Movement.Revealed: // Player is revealed/hiding too long
                moveSpeed = 0;
                    stamina += 1;
                if (timeCounter >= 0.5)
                {
                    suspicion += 2;
                    timeCounter = 0;
                }
                break;
        }
    }

    public void OnMove(InputAction.CallbackContext context) // Movement vector
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void StartSprinting(InputAction.CallbackContext context) // Sprinting input
    {
        if (context.performed && movePhase != Movement.Hidden && movePhase != Movement.Revealed)
        {
            movePhase = Movement.Sprinting;
        }

        if (context.canceled && movePhase != Movement.Hidden && movePhase != Movement.Revealed)
        {
            movePhase = Movement.Walking;
        }
    }

    public void Hide(InputAction.CallbackContext context)
    {
        if (canHide)
        {
            if (movePhase == Movement.Hidden || movePhase == Movement.Revealed)
            {
                movePhase = Movement.Walking;
                spriteControls.enabled = true;
                hitBox.enabled = true;
            }
            else
            {
                movePhase = Movement.Hidden;
                spriteControls.enabled = false;
                hitBox.enabled = false;
            }
        }
    }
}
