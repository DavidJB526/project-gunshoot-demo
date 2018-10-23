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
    private Animator anim;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private bool grounded;
    private bool isFacingRight = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isFacingRight = true;
    }

    private void Update()
    {
        GroundCheck();
        Shoot();
    }

    private void FixedUpdate()
    {
        //Move();
        Jump();                           
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        anim.SetFloat("playerSpeed", Mathf.Abs(moveHorizontal));

        if (grounded == true)
        {
            rb2d.AddForce(Vector2.right * movement, ForceMode2D.Impulse);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
        }

        if (moveHorizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        anim.SetFloat("playerVSpeed", rb2d.velocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb2d.AddForce(Vector2.up * jumpForceUp);

            if (isFacingRight)
            {
                rb2d.AddForce(Vector2.right * jumpForceForward);
            }
            else if (!isFacingRight)
            {
                rb2d.AddForce(Vector2.left * jumpForceForward);
            }
        }
    }

    private void Shoot()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("playerShoot");
            Vector2 recoil = new Vector2();
            recoil = transform.TransformDirection(-firePoint.right * recoilSpeed);

            Rigidbody2D bulletClone;

            bulletClone = Instantiate(bullet, transform.position + firePoint.right, firePoint.transform.rotation) as Rigidbody2D;
            bulletClone.velocity = transform.TransformDirection(firePoint.right * bulletSpeed);

            rb2d.AddForce(recoil, ForceMode2D.Impulse);

            if ((firePoint.rotation.eulerAngles.z > 90 && isFacingRight) || (firePoint.rotation.eulerAngles.z < -90 && isFacingRight))
            {
                Flip();
            }
            else if ((firePoint.rotation.eulerAngles.z < 90 && !isFacingRight) || (firePoint.rotation.eulerAngles.z > -90 && !isFacingRight))
            {
                Flip();
            }


            //if (!grounded)
            //{
            //    Flip();
            //}
        }
    }

    private void GroundCheck()
    {
        grounded = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;
        anim.SetBool("isGrounded", grounded);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
