using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovementGravity : MonoBehaviour
{
    [Range(0.1f, 10)] public float horizontalLookSpeedMultiplier;
    [Range(0.1f, 10)] public float movementSpeedMultiplier;

    [Range(-1, 1)] public float rotation;
    [Range(0, 1)] public float walkSpeed;

    CharacterController controller;
    Animator animator;

    [Space(10)]
    [Header("Gravity")]
    public Transform groundCheck;
    public float gravity = -20;
    Vector3 gravityVelocity;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;
    bool isGrounded;

    [Space(10)]
    public bool isDead = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }

        animator.SetFloat("MovementSpeed", walkSpeed);

        Vector3 move = transform.forward * walkSpeed;
        controller.Move(move * movementSpeedMultiplier * Time.deltaTime);
        transform.Rotate(Vector3.up * Remap(rotation,-1,1,-180,180) * horizontalLookSpeedMultiplier * Time.deltaTime);

        gravityVelocity.y += gravity * Time.deltaTime;
        controller.Move(gravityVelocity * Time.deltaTime);
    }

    float isMoving(float a, float b)
    {
        if (Mathf.Abs(a) >= Mathf.Abs(b)) return Mathf.Abs(a);
        else return Mathf.Abs(b);
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
