using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractabeButton : MonoBehaviour, IInteractable, IOutlinable
{
    private Outline _outline;

    public event Action OnPress;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        Debug.Log($"Interaction {gameObject.name}");
        OnPress?.Invoke();
    }

    public void DisableOutline()
    {
        _outline.SetOutlineWidth(0);
    }

    public void EnableOutline()
    {
        _outline.SetOutlineWidth(3);
    }
}
