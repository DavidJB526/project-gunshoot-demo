using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour 
{
    private AudioSource audioSource;
    private CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isNotPlayerOrBackground = collision.tag != "Player" && collision.tag != "Background";
        bool isNotCollectableOrCheckpoint = collision.tag != "Collectable" && collision.tag != "Checkpoint";

        if (isNotCollectableOrCheckpoint && isNotPlayerOrBackground)
        {
            circleCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
        
    }
}
