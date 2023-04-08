using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    List<BoxPickup> stack = new List<BoxPickup>();
    [SerializeField] private Transform stackOrigin;
    private float ySpacing = 0.10f;

    public void Push(BoxPickup newBox)
    {
        newBox.transform.SetParent(stackOrigin);
        float highestYPosInStack = stackOrigin.position.y;
        for (int i = 0; i < stack.Count; i++)
        {
            if(stack[i].transform.position.y > highestYPosInStack)
                highestYPosInStack = stack[i].transform.position.y;
        }
        float nextYPos = highestYPosInStack + newBox.transform.localScale.y + ySpacing;
        newBox.transform.position = new Vector3(stackOrigin.position.x, nextYPos, stackOrigin.position.z);
        stack.Add(newBox);
    }

    public void Remove(BoxPickup boxPickup)
    {
        int index = stack.IndexOf(boxPickup);
        if (index >= 0)
        {
            stack.RemoveAt(index);
            boxPickup.transform.SetParent(null);
            StartCoroutine(CloseGapInStack());
        }
    }

    public float GetStackOriginY()
    {
        return stackOrigin.position.y;
    }

    private IEnumerator CloseGapInStack()
    {
        // TODO make stack invincible for X time after hit registered
        // but not to long to allow skipping obstacles
        // or maybe just make the trigger colliders on boxes that get shifted down disabled for a brief moment
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < stack.Count; i++)
        {
            BoxPickup current = stack[i];
            BoxPickup previous = i > 0 ? stack[i - 1] : null;
            if (previous != null && current != null)
            {
                float verticalDistanceBetween = Vector3.Distance(previous.transform.position, current.transform.position);
                float distanceToShift = -(verticalDistanceBetween - (previous.transform.localScale.y + ySpacing));
                if(verticalDistanceBetween > previous.transform.localScale.y + ySpacing)
                {
                    current.transform.Translate(new Vector3(0, distanceToShift, 0));
                }
            }
        }
    }
}
