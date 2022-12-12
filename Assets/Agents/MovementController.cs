using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Range(1, 10)] public float movementSpeed;
    [Range(0, 3)] public float movementRadius;
    [Range(0, 1)] public float forwards;
    [Range(-1, 1)] public float direction;
    CharacterController controller;
    public bool isDead = false;

    Animator animator;
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
        Vector3 move;
        animator.SetFloat("MovementSpeed", isMoving(forwards, direction));
        move = transform.forward * forwards;
        gameObject.transform.Rotate(Vector3.up * direction * movementRadius);
        controller.Move(move * movementSpeed * Time.deltaTime);
    }


    float isMoving(float a, float b)
    {
        if (Mathf.Abs(a) >= Mathf.Abs(b)) return Mathf.Abs(a);
        else return Mathf.Abs(b);
    }
}
