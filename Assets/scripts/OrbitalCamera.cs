using System;
using System.Collections;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float sensitivity = 2.0f;
    public bool yInvert = false;
    private float x = 0.0f;
    private float y = 0.0f;

    // as stack groes, increase both the yOffset and the distance (by a fraction of the yOffset)
    [SerializeField] public float yOffset = 1f;
    [SerializeField] private float maxYRotation = 80f;
    [SerializeField] private float minYRotation = -20f;
    float yRotated = 0.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.position = (target.position + new Vector3(0, yOffset, 0));
        transform.rotation = target.rotation;
    }

    void LateUpdate()
    {
        x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        y = Input.GetAxis("Mouse Y") * (yInvert ? -1: 1) * sensitivity * Time.deltaTime;

        if (yRotated + y > maxYRotation)
            y = maxYRotation - yRotated;
        if (yRotated + y < minYRotation)
            y = minYRotation - yRotated;

        transform.RotateAround(target.position, Vector3.up, x);
        transform.RotateAround(target.position, transform.right, y); // changes X rotation value
        transform.position = (target.position + new Vector3(0, yOffset, 0)) - (transform.rotation * Vector3.forward * distance);
        yRotated += y;
    }

    public void IncreaseDistance(BoxPickup box)
    {
        // TODO see if I can find a way to make this smoother
        //StartCoroutine(SmoothlyAdjustCamDistance(distance+box.transform.localScale.y, yOffset + box.transform.localScale.y));
        distance += box.transform.localScale.y;
        yOffset += box.transform.localScale.y;
    }

    //private IEnumerator SmoothlyAdjustCamDistance(float targetDistance, float targetYOffset)
    //{
    //    float timer = 0.0f;
    //    while(timer < 1)
    //    {
    //        distance = Mathf.Lerp(distance, targetDistance, timer);
    //        yOffset = Mathf.Lerp(yOffset, targetYOffset, timer);
    //        timer += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    public void DecreaseDistance(BoxPickup box)
    {
        //StartCoroutine(SmoothlyAdjustCamDistance(distance - box.transform.localScale.y, yOffset - box.transform.localScale.y));

        distance -= box.transform.localScale.y;
        yOffset -= box.transform.localScale.y;
    }
}
