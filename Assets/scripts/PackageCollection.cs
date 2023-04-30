using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCollection : MonoBehaviour
{
    public AnimationCurve boxMove;
    [SerializeField] private float speed = .5f;
    [SerializeField] private ScoreManager scoreManager;
    private Vector3 firstTargetPos;

    private void OnTriggerEnter(Collider other)
    {
        Transform childTransform = GetComponentInChildren<Transform>();
        firstTargetPos = childTransform.position;
        if (other.tag == "Player")
        {
            StackManager stackManager = other.GetComponent<StackManager>();
            List<BoxPickup> boxPickups = stackManager.getStack();
            Vector3 targetPos = firstTargetPos + new Vector3(0,0,2);
            for (int i = boxPickups.Count - 1; i > -1; i--)
            {
                boxPickups[i].transform.SetParent(transform);
                boxPickups[i].GetComponent<BoxCollider>().enabled = false;
                boxPickups[i].GetComponent<Rotate>().enabled = false;

                StartCoroutine(moveBoxToCollection(boxPickups[i], targetPos));
                scoreManager.add(boxPickups[i].score);
                scoreManager.RenderScoreText();
                stackManager.Remove(boxPickups[i]);

                if (i % 3 == 0)
                    targetPos = targetPos + new Vector3(0, 1, -4);
                else
                    targetPos = targetPos + new Vector3(0, 0, 2f);

            }
        }

        IEnumerator moveBoxToCollection(BoxPickup box, Vector3 targetPos)
        {
            bool hasFinishedCurve = false;
            float time = 0.0f;
            Vector3 direction;
            float distance;
            Vector3 normalizedDirection;
            Vector3 translationVector;
            while (!hasFinishedCurve)
            {
                direction = targetPos - box.transform.position;
                distance = direction.magnitude;
                normalizedDirection = direction / distance;
                translationVector = normalizedDirection * speed * boxMove.Evaluate(time) * Time.deltaTime;
                box.transform.Translate(translationVector, Space.World);

                time += Time.deltaTime;
                hasFinishedCurve = direction.magnitude < 0.1f;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
