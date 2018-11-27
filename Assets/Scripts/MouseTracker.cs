using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be attached to the FirePoint of the Player in order to track
/// the position of the Mouse and rotate the FirePoint towards it
/// </summary>
public class MouseTracker : MonoBehaviour
{
    [SerializeField]
    private Vector3 firePoint;
    [SerializeField]
    private float angle;

    private Vector3 mousePosition;
    private Vector3 distanceToMouse;

    private void FixedUpdate()
    {
        mousePosition = Input.mousePosition;
        firePoint = Camera.main.WorldToScreenPoint(transform.position);
        //check position of mouse and firepoint in world and adjust distance values to match position
        distanceToMouse.x = mousePosition.x - firePoint.x;
        distanceToMouse.y = mousePosition.y - firePoint.y;
        //measure the Tangent and convert from Radians to Degrees to input into a rotation
        angle = Mathf.Atan2(distanceToMouse.y, distanceToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
