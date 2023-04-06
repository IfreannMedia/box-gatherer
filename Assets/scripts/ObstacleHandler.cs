using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
            GetComponent<BoxCollider>().enabled = false;
            StackManager stackManager = GetComponentInParent<StackManager>();
            stackManager.Remove(GetComponent<BoxPickup>());
            StartCoroutine(MoveAlongCurveY());
            StartCoroutine(ImpulseForceFromCol(other));
        }
    }

    private IEnumerator ImpulseForceFromCol(Collider other)
    {
        float duration = 0.0f;
        Vector3 randDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1f, 1f));
        while (duration <= 1.5f)
        {
            duration += Time.deltaTime;
            transform.Translate(randDir * Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveAlongCurveY()
    {
        bool finishedCurve = time > 0.0f && current == last;
        if (finishedCurve)
        {
            // WON'T EXECUTE FOR SOME REASON
            Debug.Log("FINISHED CURVE");
        }
        while (!finishedCurve)
        {
            current = animCurve.Evaluate(time);

            transform.position = new Vector3(transform.position.x, originalY + current, transform.position.z);

            time += Time.deltaTime;
            last = current;
            yield return new WaitForEndOfFrame();
        }
        // WON'T EXECUTE FOR SOME REASON
        GetComponent<BoxCollider>().enabled = true;
        Debug.Log("BoxCollider enabled: " + GetComponent<BoxCollider>().enabled);
        GetComponent<BoxPickup>().enabled = true;
        Debug.Log("BoxPickup enabled: " + GetComponent<BoxPickup>().enabled);
    }
}
