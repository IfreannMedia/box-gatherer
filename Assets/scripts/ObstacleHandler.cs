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
            StartCoroutine(handleObstableCollision());
        }
    }

    private IEnumerator handleObstableCollision()
    {
        // TODO
        // set destruction timer
        // Destroy after X time
        GetComponent<BoxCollider>().enabled = false;
        StackManager stackManager = GetComponentInParent<StackManager>();
        stackManager.Remove(GetComponent<BoxPickup>());
        StartCoroutine(MoveAlongCurveY());
        yield return StartCoroutine(ImpulseForceFromCol());

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxPickup>().enabled = true;
        GetComponent<Rotate>().enabled = true;
    }

    private IEnumerator ImpulseForceFromCol()
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
        bool hasFinishedCurve = false;
        while (!hasFinishedCurve)
        {
            current = animCurve.Evaluate(time);
            hasFinishedCurve = time > 0.0f && current == last;

            transform.position = new Vector3(transform.position.x, originalY + current, transform.position.z);

            time += Time.deltaTime;
            last = current;
            yield return new WaitForEndOfFrame();
        }
        time = 0.0f;
        last = 0.0f;
        current = 0.0f;
    }
}
