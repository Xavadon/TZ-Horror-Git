using QuickOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractabeButton : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionName = "Complete Coffee";

    private Outline _outline;

    public string Name => _interactionName;

    public bool IsInteractable => true;

    public event Action OnPress;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Interact()
    {
        Debug.Log($"Interaction {gameObject.name}");
        OnPress?.Invoke();
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
    }
}
