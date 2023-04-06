using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(-400, 400)] public float xSpeed, ySpeed, zSpeed;

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.up, .45f);
        transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
    }
}
