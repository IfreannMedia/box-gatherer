using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 targetMoveDir;
    [SerializeField] private float duration = 2.0f, waitTime = 2.0f;
    private float speed = 0.5f, time = 0.0f;
    private Vector3 startPos, endPos, direction, currentTarget;
    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + targetMoveDir;
        direction = endPos - startPos;
        speed = direction.magnitude / duration;
        currentTarget = endPos;
        StartCoroutine(moveObstacle());
    }

    //private void Update()
    //{
    //    bool atStart = transform.position == startPos;
    //    bool atEnd = transform.position == endPos;
    //    if ((atStart || atEnd) && time < waitTime)
    //    {
    //        currentTarget = atEnd ? startPos : endPos;
    //        time += Time.deltaTime;
    //        return;
    //    }
    //    transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    //}

    IEnumerator moveObstacle()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            if (transform.position == endPos || transform.position == startPos)
            {
                currentTarget = currentTarget == endPos ? startPos : endPos;
                yield return new WaitForSeconds(waitTime);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
