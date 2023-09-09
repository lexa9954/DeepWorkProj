using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            velocity.y = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * -gravity * controller.stepOffset);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        moveDirection.y = velocity.y;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
