using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerSpeed, jumpForceForward, jumpForceUp;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float playerJump = Input.GetAxis("Jump");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        Vector3 jump = new Vector3(jumpForceForward, playerJump * jumpForceUp, 0.0f);

        transform.position = transform.position + (movement * playerSpeed);
        rb.AddForce(jump);

    }
}
