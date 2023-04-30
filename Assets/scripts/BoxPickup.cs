using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{

    public int score = 25;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StackManager stackManager = other.GetComponent<StackManager>();
            if (stackManager.getStack().Contains(this))
                return;
            GetComponent<AudioSource>().pitch = Random.Range(.9f, 1.1f);
            GetComponent<AudioSource>().Play();
            GetComponent<ObstacleHandler>().ResetAfterPickup();
            GetComponent<Rotate>().enabled = false;
            enabled = false;
            stackManager.Push(this);
        }
    }
}
