using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    List<BoxPickup> stack = new List<BoxPickup>();
    [SerializeField] private Transform stackOrigin;
    private float ySpacing = 0.10f;
    private OrbitalCamera orbitalCam;
    private Animator animator;

    private void Start()
    {
        orbitalCam = Camera.main.GetComponent<OrbitalCamera>();
        animator = GetComponentInChildren<Animator>();
    }
    public void Push(BoxPickup newBox)
    {
        newBox.transform.SetParent(stackOrigin);
        PositionBoxInStack(newBox);
        stack.Add(newBox);
        orbitalCam.IncreaseDistance(newBox);
        animator.SetBool("hasBoxes", stack.Count > 0);
    }

    private void PositionBoxInStack(BoxPickup newBox)
    {
        float highestYPosInStack = stackOrigin.position.y;
        float previousBoxHeight = 0.0f;
        for (int i = 0; i < stack.Count; i++)
        {
            if (stack[i] != null && stack[i].transform.position.y > highestYPosInStack)
                highestYPosInStack = stack[i].transform.position.y;
            if (i+1 >= stack.Count)
                previousBoxHeight = stack[i].GetComponent<BoxCollider>().size.y;
        }
        float nextYPos = highestYPosInStack + previousBoxHeight + ySpacing;
        newBox.transform.position = new Vector3(stackOrigin.position.x, stack.Count > 0 ? nextYPos : stackOrigin.position.y, stackOrigin.position.z);
    }

    public void Remove(BoxPickup boxPickup)
    {
        orbitalCam.DecreaseDistance(boxPickup);
        int index = stack.IndexOf(boxPickup);
        if (index >= 0)
        {
            stack.RemoveAt(index);
            boxPickup.transform.SetParent(null);
            StartCoroutine(CloseGapInStack());
        }
        animator.SetBool("hasBoxes", stack.Count > 0);
    }

    public List<BoxPickup> getStack()
    {
        return stack;
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
        if(stack.Count > 0 && stack[0].transform.position != stackOrigin.position)
        {
            stack[0].transform.Translate(new Vector3(0, Vector3.Distance(stack[0].transform.position, stackOrigin.position), 0));
        }

        for (int i = 0; i < stack.Count; i++)
        {
            BoxPickup current = stack[i];
            BoxPickup previous = i > 0 ? stack[i - 1] : null;
            if (previous != null && current != null)
            {
                float previousBoxHeight = previous.GetComponent<BoxCollider>().size.y;
                float verticalDistanceBetween = Vector3.Distance(previous.transform.position, current.transform.position);
                if(verticalDistanceBetween > previousBoxHeight + ySpacing)
                {
                    float distanceToShift = -(verticalDistanceBetween - (previousBoxHeight + ySpacing));
                    current.transform.Translate(new Vector3(0, distanceToShift, 0));
                }
            }
            else if(current != null && current.transform.position != stackOrigin.position)
            {
                current.transform.position = stackOrigin.position;
            }
        }
    }
}
