using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Serialized Floats
    [SerializeField]
    [Tooltip("Max Speed that the Player can Move")]
    private float maxSpeed;
    [SerializeField]
    [Tooltip("How much Forward force is applied to the Player on Jump")]
    private float jumpForceForward;
    [SerializeField]
    [Tooltip("How much Upwards force is applied to the Player on Jump")]
    private float jumpForceUp;
    [SerializeField]
    [Tooltip("How fast the Bullets move (pos value)")]
    private float bulletSpeed;
    [SerializeField]
    [Tooltip("How much Backwards force is applied to the Player when they Shoot (pos value)")]
    private float recoilSpeed;
    [SerializeField]
    [Tooltip("How much Overheat is added on each Shot (pos value)")]
    private float overheatAdd;
    [SerializeField]
    [Tooltip("How much Overheat is subtracted each Physics Frame (neg value)")]
    private float overheatSubtract;
    #endregion

    #region Serialized Objects
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
    private Slider overheatSlider;
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip deathSound;
    #endregion

    #region Private Variables
    private Rigidbody2D rb2d;
    private Animator anim;
    private AudioSource audioSource;
    private Checkpoint currentCheckpoint;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private bool grounded;
    private bool isDead = false;
    private bool isFacingRight = true;
    private float overheat = 0f;
    #endregion

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        isFacingRight = true;
        isDead = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            GroundCheck();
            Shoot();
        }
        anim.SetBool("isDead", isDead);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Jump();
        }
        UpdateOverheat();
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
            audioSource.clip = jumpSound;
            audioSource.Play();

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
            anim.SetBool("isShooting", true);
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
            Collectable.collectables = 0;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            transform.position = currentCheckpoint.transform.position;
        }
        isDead = false;
    }    

    private void UpdateOverheat()
    {
        if(overheat > 0)
        {
            overheat -= overheatSubtract;
        }        
        overheatSlider.value = overheat / 100;
    }

    private void ResetIsShooting()
    {
        anim.SetBool("isShooting", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            isDead = true;
            audioSource.clip = deathSound;
            audioSource.Play();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            SetCurrentCheckpoint(other.GetComponent<Checkpoint>());
        }
    }
}
