using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour {

    private Vector3 mousePosition;

    [SerializeField]
    private Vector3 firePoint;
    [SerializeField]
    private float angle;

    private void FixedUpdate()
    {
        mousePosition = Input.mousePosition;
        firePoint = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x = mousePosition.x - firePoint.x;
        mousePosition.y = mousePosition.y - firePoint.y;
        angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
