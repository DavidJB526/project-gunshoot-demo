using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    [SerializeField]
    private Text collectableText;

    public float startingCollectables;

    private void Start()
    {
        startingCollectables = Collectable.collectables;
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
