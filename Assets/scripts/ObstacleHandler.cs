using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    private float time = 0.0f;
    private float current = 0.0f;
    private float last = 0.0f;
    float originalY;


    private void Start()
    {
        originalY = transform.position.y;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Obstacle"))
        {
            // TODO
            // remove from stack
            // knock onto floor
            // set destruction timer
            // Destroy after X time
            StartCoroutine(MoveAlongCurveY());
        }
    }

    IEnumerator MoveAlongCurveY()
    {
        bool finishedCurve = time > 0.0f && current == last;
        while (!finishedCurve)
        {
            current = animCurve.Evaluate(time);

            transform.position = new Vector3(transform.position.x, originalY + current, transform.position.z);

            time += Time.deltaTime;
            last = current;
            yield return new WaitForEndOfFrame();
        }

    }
}
