using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    Stack<BoxPickup> stack = new Stack<BoxPickup>();
    [SerializeField] private Transform stackOrigin;
    private float ySpacing = 0.10f;

    public void Push(BoxPickup newBox)
    {
        newBox.transform.SetParent(stackOrigin);
        float nextYPos = stack.Count == 0 ? stackOrigin.position.y : stack.Peek().transform.position.y + newBox.transform.localScale.y + ySpacing;
        newBox.transform.position = new Vector3(stackOrigin.position.x, nextYPos, stackOrigin.position.z);
        
        stack.Push(newBox);
    }
}