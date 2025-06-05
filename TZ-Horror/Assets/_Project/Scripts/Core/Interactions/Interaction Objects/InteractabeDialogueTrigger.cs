using QuickOutline;
using System;
using System.Collections;
using UnityEngine;

public class InteractabeDialogueTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline _outline;
    [SerializeField] private string _dialogueKey = "greeting";
    [SerializeField] private Transform _lookAt;
    [SerializeField] private bool _outlineable = false;

    private float _cooldownAfterDialogue = 0.75f;
    private bool _interactAble = true;
    private bool _onCooldown;
    private Collider _collider;
    private bool _isOneShot;

    public string Name => _interactAble ? "Talk" : "";
    public bool IsInteractable => _interactAble;

    public event Action OnInteract;

    private void Awake()
    {
        if (_outline == null && _outlineable)
        {
            _outline = GetComponent<Outline>();
        }

        if (_outline != null)
        {
            _outline.enabled = false;
        }

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

        NewDialogueSystem.DialogueSystem.StartDialogue(_dialogueKey, _lookAt);
        DisableOutline();
        SetInteractAble(false);
        OnInteract?.Invoke();
    }

    private void HandleDialogueEnd(string key)
    {
        if (_isOneShot)
        {
            return;
        }

        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(_cooldownAfterDialogue);
        _onCooldown = false;
        SetInteractAble(true);
    }

    public void SetOneShot(bool value)
    {
        _isOneShot = value;
    }

    public void SetInteractAble(bool value)
    {
        _interactAble = value;

        if (_collider == null)
            _collider = GetComponent<Collider>();

        _collider.enabled = value;
        DisableOutline();
    }

    public void DisableOutline()
    {
        if (!_outlineable)
        {
            return;
        }

        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }

    public void EnableOutline()
    {
        if (!_outlineable)
        {
            return;
        }

        if (_interactAble)
        {
            _outline.enabled = true;
        }
        else
        {
            DisableOutline();
        }
    }
}
