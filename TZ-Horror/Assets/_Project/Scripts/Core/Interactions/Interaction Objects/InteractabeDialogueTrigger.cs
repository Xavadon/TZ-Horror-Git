using System;
using UnityEngine;

public class InteractabeDialogueTrigger : MonoBehaviour, IInteractable, IOutlinable
{
    [SerializeField] private Outline _outline;
    [SerializeField] private string _dialogueKey = "greeting";
    [SerializeField] private Transform _lookAt;

    private bool _interactAble = true;
    private Collider _collider;

    public event Action OnInteract;

    private void OnEnable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += OnDialogueEnd;
    }

    private void OnDisable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished -= OnDialogueEnd;
    }

    public void Construct(int dialogueIndex)
    {
        _collider = GetComponent<Collider>();

        string dialogueKey = $"order{dialogueIndex}";
        _dialogueKey = dialogueKey;
        _collider.enabled = false;
    }

    private void OnDialogueEnd(string obj)
    {
        SetInteractAble(true);
    }

    private void Awake()
    {
        if (_outline == null)
        {
            _outline = GetComponent<Outline>();
        }
    }

    public void Interact()
    {
        if (!_interactAble)
        {
            return;
        }

        Debug.Log($"Interaction {gameObject.name}");

        if (_lookAt == null)
        {
            _lookAt = transform;
        }

        NewDialogueSystem.DialogueSystem.StartDialogue(_dialogueKey, _lookAt);
        DisableOutline();
        SetInteractAble(false);

        OnInteract?.Invoke();
    }

    public void DisableOutline()
    {
        if (_interactAble)
        {
            _outline.SetOutlineWidth(0);
        }
    }

    public void EnableOutline()
    {
        if (_interactAble)
        {
            _outline.SetOutlineWidth(3);
        }
        else
        {
            DisableOutline();
        }
    }

    public void SetInteractAble(bool value)
    {
        _interactAble = value;

        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }

        _collider.enabled = value;
    }
}