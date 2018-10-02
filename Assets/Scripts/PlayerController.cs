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
    [SerializeField]
    private Collider2D groundDetectTrigger;
    [SerializeField]
    private ContactFilter2D groundContactFilter;


    private Rigidbody2D rb2d;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private bool grounded;
    private bool isFacingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GroundCheck();
        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();                           
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (grounded == true)
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
        if (Input.GetButtonDown("Jump") && grounded)
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

    private void Shoot()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 recoil = new Vector2();
            recoil = transform.TransformDirection(-firePoint.right * recoilSpeed);

            Rigidbody2D bulletClone;

            bulletClone = Instantiate(bullet, transform.position + firePoint.right, firePoint.transform.rotation) as Rigidbody2D;
            bulletClone.velocity = transform.TransformDirection(firePoint.right * bulletSpeed);

            rb2d.AddForce(recoil, ForceMode2D.Impulse);
        }
    }

    private void GroundCheck()
    {
        grounded = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;
    }
}
