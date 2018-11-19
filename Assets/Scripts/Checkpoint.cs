using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour 
{

    [SerializeField]
    private Sprite inactiveSprite, activeSprite;

    private bool isActivated;
    private SpriteRenderer spriteRenderer;

    private void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Sprite sprite = inactiveSprite;

        if (isActivated)
        {
            sprite = activeSprite;
        }

        spriteRenderer.sprite = sprite;
    }

    public void SetIsActivated(bool value)
    {
        isActivated = value;
        UpdateSprite();
    }
}
