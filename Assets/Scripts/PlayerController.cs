using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float playerAcceleration, maxSpeed, jumpForceForward, jumpForceUp, bulletSpeed, recoilSpeed, overheatAdd, overheatSubtract;
    [SerializeField]
    private PhysicsMaterial2D playerMoving, playerStopping;
    [SerializeField]
    private CapsuleCollider2D playerCollider;
    [SerializeField]
    private Rigidbody2D bullet;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private Collider2D groundDetectTrigger;
    [SerializeField]
    private ContactFilter2D groundContactFilter;
    [SerializeField]
    private Text collectableText;
    [SerializeField]
    private Slider overheatSlider;


    private Rigidbody2D rb2d;
    private Animator anim;
    private Checkpoint currentCheckpoint;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private bool grounded;
    private bool isDead = false;
    private bool isFacingRight = true;
    private float collectables = 0f;
    private float overheat = 0f;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isFacingRight = true;
        isDead = false;
        UpdateCollectables();
    }

    private void Update()
    {
        if (!isDead)
        {
            GroundCheck();
            Shoot();
        }
        anim.SetBool("isDead", isDead);
        UpdateOverheat();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Jump();
        }                                   
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            playerCollider.sharedMaterial = playerMoving;
        }
        else
        {
            playerCollider.sharedMaterial = playerStopping;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        anim.SetFloat("playerSpeed", Mathf.Abs(moveHorizontal));

        if (grounded)
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

        if (Input.GetButtonDown("Jump") && grounded && !isDead)
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
        if (Input.GetButtonDown("Fire1") && overheat < 100)
        {
            overheat += overheatAdd;
            rb2d.velocity = Vector3.zero;
            anim.SetTrigger("playerShoot");
            Vector2 recoil = new Vector2();
            recoil = transform.TransformDirection(-firePoint.right * recoilSpeed);

            Rigidbody2D bulletClone;

            bulletClone = Instantiate(bullet, transform.position + firePoint.right, firePoint.transform.rotation) as Rigidbody2D;
            bulletClone.velocity = transform.TransformDirection(firePoint.right * bulletSpeed);

            rb2d.AddForce(recoil, ForceMode2D.Impulse);

            if (isFacingRight && (firePoint.rotation.eulerAngles.z > 90 && firePoint.rotation.eulerAngles.z < 270))
            {
                Flip();
            }
            else if (!isFacingRight && (firePoint.rotation.eulerAngles.z < 90 || firePoint.rotation.eulerAngles.z > 270))
            {
                Flip();
            }
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

    private void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {
        if (currentCheckpoint != null)
        {
            currentCheckpoint.SetIsActivated(false);
        }

        currentCheckpoint = newCurrentCheckpoint;
        currentCheckpoint.SetIsActivated(true);
    }

    private void Respawn()
    {
        rb2d.velocity = Vector2.zero;
        anim.ResetTrigger("playerShoot");
        grounded = true;
        overheat = 0;

        if (currentCheckpoint == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            transform.position = currentCheckpoint.transform.position;
        }
        isDead = false;
    }

    private void UpdateCollectables()
    {
        collectableText.text = "Collectables: " + collectables;
    }

    private void UpdateOverheat()
    {
        if(overheat > 0)
        {
            overheat -= overheatSubtract;
        }        
        overheatSlider.value = overheat / 100;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            isDead = true;
        }
        else if (other.CompareTag("Checkpoint"))
        {
            SetCurrentCheckpoint(other.GetComponent<Checkpoint>());
        }
        else if (other.CompareTag("Collectable"))
        {
            collectables++;
            UpdateCollectables();
        }
    }
}
