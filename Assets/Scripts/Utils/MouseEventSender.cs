using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This component send Unity mouse Message to parent of this gameObject. Require to have Collider in this gameObject
/// </summary>
public class MouseEventSender : MonoBehaviour
{
    [Header("This Component with attached Collider will send Mouse Event", order = 0)]
    [Space(-10, order = 1)]
    [Header("To its parent/child Object that doens't have collider attached", order = 2)]
    [Space(10, order = 3)]

    [SerializeField] UnityEvent onMouseUp;
    [SerializeField] UnityEvent onMouseDrag;
    [SerializeField] UnityEvent onMouseDown;
    [SerializeField] UnityEvent onMouseEnter;
    [SerializeField] UnityEvent onMouseExit;
    [SerializeField] UnityEvent onMouseUpAsButton;

    private void OnMouseUp()
    {
        onMouseUp?.Invoke();
    }

    private void OnMouseDown()
    {
        onMouseDown?.Invoke();
    }

    private void OnMouseDrag()
    {
        onMouseDrag?.Invoke();
    }

    private void OnMouseEnter()
    {
        onMouseEnter?.Invoke();
    }

    private void OnMouseExit()
    {
        onMouseExit?.Invoke();
    }
    private void OnMouseUpAsButton()
    {
        if (BitBenderGames.TouchInputHelper.IsClick)
            onMouseUpAsButton?.Invoke();
    }
}
