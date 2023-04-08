using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] private AnimationCurve knockDownCurve;
    [SerializeField] [Range(0.5f, 8f)] private float remainActive = 4f;
    float animDuration;


    private void Start()
    {
        animDuration = knockDownCurve.keys[knockDownCurve.keys.Length - 1].time;
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
        GetComponent<BoxCollider>().enabled = false;
        StackManager stackManager = GetComponentInParent<StackManager>();
        stackManager.Remove(GetComponent<BoxPickup>());
        StartCoroutine(MoveAlongCurveY(stackManager.GetStackOriginY()));
        yield return StartCoroutine(ImpulseForceFromCol());
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxPickup>().enabled = true;
        GetComponent<Rotate>().enabled = true;
        yield return StartCoroutine(Blink());
        Destroy(this.gameObject);
        
    }

    private IEnumerator Blink()
    {
        float timer = 0.0f;
        float timeBetweenBlinks = .5f;
        Renderer rend = GetComponent<Renderer>();
        while (timer<= remainActive)
        {
            // set rend off
            rend.enabled = false;
            // wait fraction of second
            yield return new WaitForSeconds(.05f);
            // set rend on
            rend.enabled = true;
            // leave rendered for timeBetweenBlinks
            yield return new WaitForSeconds(timeBetweenBlinks);
            // decrease timeBetweenBlinks until .1f
            timeBetweenBlinks = timeBetweenBlinks <= .1f ? timeBetweenBlinks : timeBetweenBlinks / 1.5f;
            // add fraction of second and timeBetweenBlinks to timer
            timer += .05f + timeBetweenBlinks;
        }
    }

    private IEnumerator ImpulseForceFromCol()
    {
        float duration = 0.0f;
        Vector3 randDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1f, 1f));
        while (duration <= animDuration)
        {
            duration += Time.deltaTime;
            transform.Translate(randDir * Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveAlongCurveY(float groundYPos)
    {
        bool hasFinishedCurve = false;
        //float originalY = transform.position.y;
        float originalY = groundYPos;
        float time = 0.0f;
        float current;
        float last = 0.0f;
        while (!hasFinishedCurve)
        {
            current = knockDownCurve.Evaluate(time);
            hasFinishedCurve = time > 0.0f && current == last;

            transform.position = new Vector3(transform.position.x, originalY + current, transform.position.z);

            time += Time.deltaTime;
            last = current;
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetAfterPickup()
    {
        StopAllCoroutines();
        GetComponent<Renderer>().enabled = true;
    }
}
