using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float sensitivity = 2.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * sensitivity;
        y -= Input.GetAxis("Mouse Y") * sensitivity;

        y = Mathf.Clamp(y, -80f, 80f);

        transform.rotation = Quaternion.Euler(y, x, 0);
        //
        // Summary:
        //     Rotates the transform about axis passing through point in world coordinates by
        //     angle degrees.
        //
        // Parameters:
        //   point:
        //
        //   axis:
        //
        ////   angle:
        //public void RotateAround(Vector3 point, Vector3 axis, float angle);
        ////
        //// Parameters:
        ////   axis:
        ////
        ////   angle:
        //[Obsolete("warning use Transform.Rotate instead.")]
        //public void RotateAround(Vector3 axis, float angle);
        transform.RotateAround(target.position, Vector3.up, x);
        transform.position = target.position - (transform.rotation * Vector3.forward * distance);
    }
}
