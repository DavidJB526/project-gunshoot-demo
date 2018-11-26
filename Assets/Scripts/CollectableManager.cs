using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    [SerializeField]
    private Text collectableText;

    private void Start()
    {
        UpdateCollectableText();
    }

    private void Update()
    {
        UpdateCollectableText();
    }

    private void UpdateCollectableText()
    {
        collectableText.text = "Collectables: " + Collectable.collectables;
    }
}
