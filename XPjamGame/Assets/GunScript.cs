using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Vector3 dir;

    private void Update()
    {
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        dir = dir.normalized;

        float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }
}
