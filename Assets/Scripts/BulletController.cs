using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Background" && collision.tag != "Collectable" && collision.tag != "Checkpoint")
        {
            Debug.Log("Hit " + collision.name);
            Destroy(gameObject);
        }
        
    }
}
