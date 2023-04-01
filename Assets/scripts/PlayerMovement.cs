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
    Camera cam;

    public void Start()
    {
        cam = Camera.main;
        charController = GetComponent<CharacterController>();
    }

    public void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

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

        // face cam forward parallel to ground
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // cam forward parallel to flat groun
            // The cross product of two vectors results in a third vector which is perpendicular to the two input vectors.
            Vector3 camForwardToFlatGround = Vector3.Cross(cam.transform.right, Vector3.up);
            // Creates a rotation with the specified forward and upwards directions.
            Quaternion newRotation = Quaternion.LookRotation(camForwardToFlatGround, Vector3.up);
            transform.rotation = newRotation;
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
