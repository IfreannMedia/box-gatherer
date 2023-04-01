using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.name + enabled);
        if (other.tag.Equals("Player"))
        {
            GetComponent<Rotate>().enabled = false;
            enabled = false;
            other.GetComponent<StackManager>().Push(this);
        }
    }
}
