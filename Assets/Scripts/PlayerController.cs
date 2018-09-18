using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerSpeed, maxSpeed, jumpForceForward, jumpForceUp;

    private Rigidbody2D rb;
    private bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        Vector3 jump = new Vector3(0.0f, jumpForceUp, 0.0f);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (grounded)
        {
            rb.AddForce(Vector2.right * movement, ForceMode2D.Impulse);

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * jumpForceForward, ForceMode2D.Impulse);
            }
        }
              
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
