using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour 
{

    [SerializeField]
    private Sprite inactiveSprite, activeSprite;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private bool isActivated;

    private void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Sprite sprite = inactiveSprite;

        if (isActivated)
        {            
            sprite = activeSprite;
            audioSource.Play();
        }

        spriteRenderer.sprite = sprite;
    }

    public void SetIsActivated(bool value)
    {
        isActivated = value;
        UpdateSprite();
    }
}
