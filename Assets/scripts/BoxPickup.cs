using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GetComponent<ObstacleHandler>().ResetAfterPickup();
            GetComponent<Rotate>().enabled = false;
            enabled = false;
            other.GetComponent<StackManager>().Push(this);
        }
    }
}
