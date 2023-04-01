using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;
    [SerializeField] [Range(0.5f, 10.0f)] private float speed = 5f;
    Vector3 movement;
    Vector3 playerVelocity;
    [SerializeField] [Range(.5f, 2f)] float jumpHeight = 1.0f;
    float horizontalRotation, verticalRotation, mouseSensitivity = 5f;
    float gravityValue = -9.81f;

    public void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // cam rotation
        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        //transform.localRotation = Quaternion.Euler(0.0f, horizontalRotation, 0.0f);

        // movement
        movement = new Vector3(horizontalInput * speed, 0.0f, verticalInput * speed);
        movement = transform.rotation * movement;
        charController.Move(movement * Time.deltaTime);

        // jumping
        if (isGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        //if(!isGrounded())
        playerVelocity.y += gravityValue * Time.deltaTime;
        charController.Move(playerVelocity * Time.deltaTime);
    }

    public bool isGrounded()
    {
        return charController == null ? true : charController.isGrounded;
    }

}
