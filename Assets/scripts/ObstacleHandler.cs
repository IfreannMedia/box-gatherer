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
    [SerializeField] private AudioSource boxHit;


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
        boxHit.pitch = Random.Range(.6f, 1.1f);
        boxHit.Play();
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
        float originalY = transform.position.y;
        float time = 0.0f;
        float current;
        float last = 0.0f;
        // first key becomes current transform y
        knockDownCurve.MoveKey(0, new Keyframe(knockDownCurve.keys[0].time, originalY, knockDownCurve.keys[0].inTangent, knockDownCurve.keys[0].outTangent));
        // second key becomes current y plus 2
        knockDownCurve.MoveKey(1, new Keyframe(knockDownCurve.keys[1].time, originalY + 2, knockDownCurve.keys[1].inTangent, knockDownCurve.keys[1].outTangent));
        // third key becomes ground
        knockDownCurve.MoveKey(2, new Keyframe(knockDownCurve.keys[2].time, .75f, knockDownCurve.keys[2].inTangent, knockDownCurve.keys[2].outTangent));
        // fourth key becomes half of second
        knockDownCurve.MoveKey(3, new Keyframe(knockDownCurve.keys[3].time, knockDownCurve.keys[1].value / 2, knockDownCurve.keys[3].inTangent, knockDownCurve.keys[3].outTangent));
        // fifth key becomes ground
        knockDownCurve.MoveKey(4, new Keyframe(knockDownCurve.keys[4].time, .75f, knockDownCurve.keys[4].inTangent, knockDownCurve.keys[4].outTangent));

        while (!hasFinishedCurve)
        {
            current = knockDownCurve.Evaluate(time);
            hasFinishedCurve = time > 0.0f && current == last;

            transform.position = new Vector3(transform.position.x, current, transform.position.z);
            //transform.position = new Vector3(transform.position.x, current, transform.position.z);

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
