using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCollection : MonoBehaviour
{
    public AnimationCurve boxMove;
    [SerializeField] private float speed = .5f;
    [SerializeField] private ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StackManager stackManager = other.GetComponent<StackManager>();
            List<BoxPickup> boxPickups = stackManager.getStack();
            for (int i = boxPickups.Count - 1; i > -1; i--)
            {
                boxPickups[i].transform.SetParent(null);
                boxPickups[i].GetComponent<BoxCollider>().enabled = false;
                boxPickups[i].GetComponent<Rotate>().enabled = false;
                StartCoroutine(moveBoxToCollection(boxPickups[i]));
                scoreManager.add(boxPickups[i].score);
                scoreManager.RenderScoreText();
                stackManager.Remove(boxPickups[i]);
            }
        }

        IEnumerator moveBoxToCollection(BoxPickup box)
        {
            bool hasFinishedCurve = false;
            float time = 0.0f;
            Vector3 direction;
            float distance;
            Vector3 normalizedDirection;
            Vector3 translationVector;
            while (!hasFinishedCurve)
            {
                direction = transform.position - box.transform.position;
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
