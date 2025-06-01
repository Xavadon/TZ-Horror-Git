using QuickOutline;
using System;
using UnityEngine;

public class InteractabeDialogueTrigger : MonoBehaviour, IInteractable, IOutlinable
{
    [SerializeField] private Outline _outline;
    [SerializeField] private string _dialogueKey = "greeting";
    [SerializeField] private Transform _lookAt;
    
    private float _cooldownAfterDialogue = 0.75f;
    private bool _interactAble = true;
    private bool _onCooldown;
    private Collider _collider;

    public event Action OnInteract;

    private void Awake()
    {
        if (_outline == null)
            _outline = GetComponent<Outline>();

        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished -= HandleDialogueEnd;
    }

    public void Construct(int dialogueIndex)
    {
        _dialogueKey = $"order{dialogueIndex}";
        if (_collider != null)
            _collider.enabled = false;
    }

    public void Interact()
    {
        if (!_interactAble || _onCooldown)
            return;

        if (_lookAt == null)
            _lookAt = transform;

        NewDialogueSystem.DialogueSystem.StartDialogue(_dialogueKey, _lookAt);
        DisableOutline();
        SetInteractAble(false);
        OnInteract?.Invoke();
    }

    private void HandleDialogueEnd(string _)
    {
        StartCooldown();
    }

    private async void StartCooldown()
    {
        _onCooldown = true;
        await System.Threading.Tasks.Task.Delay((int)(_cooldownAfterDialogue * 1000f));
        _onCooldown = false;
        SetInteractAble(true);
    }

    public void SetInteractAble(bool value)
    {
        _interactAble = value;

        if (_collider == null)
            _collider = GetComponent<Collider>();

        _collider.enabled = value;
    }

    public void DisableOutline()
    {
        if (_outline != null)
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
}
