using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerSpeed, maxSpeed, jumpForceForward, jumpForceUp, bulletSpeed, recoilSpeed;
    [SerializeField]
    private Rigidbody2D bullet;
    [SerializeField]
    private Transform firePoint;

    private Rigidbody2D rb;
    private bool grounded;
    private bool isFacingRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        //Shoot();                     
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (grounded)
        {
            //rb.velocity = new Vector2(moveHorizontal * playerSpeed, 0.0f);
            rb.AddForce(Vector2.right * movement, ForceMode2D.Impulse);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (moveHorizontal > 0)
        {
            isFacingRight = true;
        }
        else if (moveHorizontal < 0)
        {
            isFacingRight = false;
        }        
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb.AddForce(Vector2.up * jumpForceUp, ForceMode2D.Impulse);

                if (isFacingRight)
                {
                    rb.AddForce(Vector2.right * jumpForceForward, ForceMode2D.Impulse);
                }
                else if (!isFacingRight)
                {
                    rb.AddForce(Vector2.left * jumpForceForward, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody2D bulletClone;
            bulletClone = Instantiate(bullet, firePoint.position, transform.rotation) as Rigidbody2D;
            bulletClone.velocity = transform.TransformDirection(Vector2.right * bulletSpeed);

            if (grounded)
            {
                rb.velocity = transform.TransformDirection(Vector2.left * recoilSpeed);
            }
            else
            {
                rb.velocity = transform.TransformDirection(Vector2.left * recoilSpeed * 2);
            }
        }
    }
}
