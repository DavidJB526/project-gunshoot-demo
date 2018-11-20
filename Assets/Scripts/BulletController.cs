using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isNotPlayerOrBackground = collision.tag != "Player" && collision.tag != "Background";
        bool isNotCollectableOrCheckpoint = collision.tag != "Collectable" && collision.tag != "Checkpoint";

        if (isNotCollectableOrCheckpoint && isNotPlayerOrBackground)
        {
            Destroy(gameObject);
        }
        
    }
}
