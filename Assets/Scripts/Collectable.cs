using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    public static float collectables = 0f;

    private AudioSource audioSource;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate ()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collectables++;
            audioSource.Play();
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
