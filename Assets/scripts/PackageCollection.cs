using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCollection : MonoBehaviour
{
    public AnimationCurve boxMove;
    [SerializeField] private float speed = .5f;
    [SerializeField] private ScoreManager scoreManager;
    private Transform firstTargetPos;

    private void OnTriggerEnter(Collider other)
    {
        Transform childTransform = GetComponentInChildren<Transform>();
        firstTargetPos = childTransform;
        if (other.tag == "Player")
        {
            scoreManager.GetComponent<PauseManager>().enabled = false;
            scoreManager.EnlargeScoreText(2f);
            // deactivate main cam and player controls, activate child cam
            other.GetComponent<PlayerMovement>().enabled = false;
            other.GetComponentInChildren<Animator>().enabled = false;
            Camera scoreCam = GetComponentInChildren<Camera>(true);
            scoreCam.enabled = true;
            Camera.main.enabled = false;
            StackManager stackManager = other.GetComponent<StackManager>();
            List<BoxPickup> boxPickups = stackManager.getStack();
            StartCoroutine(AdjustCamPosition(scoreCam, boxPickups.Count));
            Vector3 targetPos = firstTargetPos.position + new Vector3(0, 0, -2f);
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
                    targetPos = targetPos + new Vector3(0, 1, 4);
                else
                    targetPos = targetPos + new Vector3(0, 0, -2f);
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

        IEnumerator AdjustCamPosition(Camera scoreCam, int boxCount)
        {
            float adjustTime = 2f;
            float timer = 0.0f;
            float heightAdjustment = .25f * boxCount;
            float backAdjustment = .1f * boxCount;
            Vector3 targetVector = scoreCam.transform.position + new Vector3(0, heightAdjustment, 0);
            targetVector += -scoreCam.transform.forward * backAdjustment;
            while (timer <= adjustTime)
            {
                scoreCam.transform.position = Vector3.MoveTowards(scoreCam.transform.position, targetVector, 15 * Time.deltaTime);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
