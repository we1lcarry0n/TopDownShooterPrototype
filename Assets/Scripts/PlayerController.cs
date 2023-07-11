using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Controls controls;
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Shooter shooter;
    private Camera mainCamera;
    private Health health;

    [SerializeField] private float speed;

    private Vector2 movementVector;

    private void Awake()
    {
        controls = new Controls();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shooter = GetComponent<Shooter>();
        mainCamera = Camera.main;
        health = GetComponent<Health>();
        controls.Player.Shoot.performed += Shoot;
    }

    private void OnEnable()
    {
        controls.Enable();
        health.Death += Die;
    }

    private void OnDisable()
    {
        controls.Disable();
        health.Death -= Die;
    }

    private void Update()
    {
        movementVector = CalculateMovementVector();
        rb2d.velocity = ConvertToVector3(movementVector);
        AdjustAnimation(movementVector);
        AdjustFlipping();
    }

    private Vector2 CalculateMovementVector()
    {
        return controls.Player.Movement.ReadValue<Vector2>().normalized * (speed);
    }

    private Vector3 ConvertToVector3(Vector2 input)
    {
        return new Vector3(input.x, input.y, 0);
    }

    private void AdjustAnimation(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }

    private void AdjustFlipping()
    {
        if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }

        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Vector3 cursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.z = 0;
        shooter.Shoot((cursor - transform.position).normalized);
    }

    private void Die(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
