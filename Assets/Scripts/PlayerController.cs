using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerAcceleration, maxSpeed, jumpForceForward, jumpForceUp, bulletSpeed, recoilSpeed;
    [SerializeField]
    private Rigidbody2D bullet;
    [SerializeField]
    private Transform firePoint;

    private Rigidbody2D rb2d;
    private bool grounded;
    private bool isFacingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Shoot();                     
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
            rb2d.AddForce(Vector2.right * movement, ForceMode2D.Impulse);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
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
                rb2d.AddForce(Vector2.up * jumpForceUp, ForceMode2D.Impulse);

                if (isFacingRight)
                {
                    rb2d.AddForce(Vector2.right * jumpForceForward, ForceMode2D.Impulse);
                }
                else if (!isFacingRight)
                {
                    rb2d.AddForce(Vector2.left * jumpForceForward, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void Shoot()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 recoil = new Vector2();
            recoil = transform.TransformDirection(-firePoint.right * recoilSpeed);

            Rigidbody2D bulletClone;

            bulletClone = Instantiate(bullet, transform.position + firePoint.right, firePoint.transform.rotation) as Rigidbody2D;
            bulletClone.velocity = transform.TransformDirection(firePoint.right * bulletSpeed);

            if (grounded)
            {
                rb2d.velocity = recoil;
            }
            else
            {
                rb2d.velocity = recoil * 2;
            }
        }
    }
}
