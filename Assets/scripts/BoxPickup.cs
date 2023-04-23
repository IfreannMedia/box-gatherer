using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{

    public readonly int score = 25;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            GetComponent<AudioSource>().Play();
            GetComponent<ObstacleHandler>().ResetAfterPickup();
            GetComponent<Rotate>().enabled = false;
            enabled = false;
            other.GetComponent<StackManager>().Push(this);
        }
    }
}
