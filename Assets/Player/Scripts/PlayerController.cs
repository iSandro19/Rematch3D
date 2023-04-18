using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Vector3 velocity;
    public float maxVelocity = 5f;
    public float gravityScale = 0.01f;
    public float angle = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocity = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        if (controller.isGrounded)
            velocity.y = 0;
        else
            velocity.y += Physics.gravity.y * gravityScale;

        velocity.x = Input.GetAxis("Horizontal") * maxVelocity;
        velocity.z = Input.GetAxis("Vertical") * maxVelocity;
        angle = 90f - Mathf.Rad2Deg * Mathf.Atan2(velocity.z, velocity.x);
        transform.rotation = Quaternion.AngleAxis(
            angle,
            Vector3.up
        );

        animator.SetFloat(
            "velocity",
            Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))
        );

        controller.Move(velocity * Time.deltaTime);
    }
}
