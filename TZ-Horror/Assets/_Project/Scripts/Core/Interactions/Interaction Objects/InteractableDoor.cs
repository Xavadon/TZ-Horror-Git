using DG.Tweening;
using QuickOutline;
using System;
using System.Collections;
using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable
{
    private readonly string Open = "Open";
    private readonly string Close = "Close";

    [SerializeField] private Vector3 _openRotationOffset = new Vector3(0f, -90f, 0f);
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private bool _isOpen = false;
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private bool _npcInteractable = false;
    [SerializeField] private PlayerCheckerCollider _playerChecker;

    private Outline _outline;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    public string Name { get; private set; }

    public bool IsInteractable => _isInteractable;

    public event Action OnOpen;
    public event Action OnClose;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _closedRotation = transform.rotation;
        Vector3 targetEuler = _closedRotation.eulerAngles + _openRotationOffset;
        _openRotation = Quaternion.Euler(targetEuler);
        _outline.enabled = false;
        Name = Open;
    }

    public void Interact()
    {
        if (!_isInteractable)
        {
            return;
        }

        if (_playerChecker != null && _playerChecker.IsPlayerHere && _isOpen)
        {
            Quaternion targetRotation = Quaternion.Slerp(_openRotation, _closedRotation, 0.2f);
            transform.DORotateQuaternion(targetRotation, _duration).SetLoops(2, LoopType.Yoyo);
            return;
        }

        if (_isOpen)
        {
            transform.DORotateQuaternion(_closedRotation, _duration);
            OnClose?.Invoke();
            Name = Open;
        }
        else
        {
            transform.DORotateQuaternion(_openRotation, _duration);
            OnOpen?.Invoke();
            Name = Close;
        }

        _isOpen = !_isOpen;
    }

    public void InteractNPC()
    {
        if (!_npcInteractable)
        {
            return;
        }

        _npcInteractable = false;
        StartCoroutine(InteractNPCRoutine());
    }

    private IEnumerator InteractNPCRoutine()
    {
        if (!_isOpen)
        {
            transform.DORotateQuaternion(_openRotation, _duration);
            OnOpen?.Invoke();
            Name = Close;
            _isOpen = true;
        }

        yield return new WaitForSeconds(_duration + 1);

        if (_isOpen)
        {
            transform.DORotateQuaternion(_closedRotation, _duration);
            OnClose?.Invoke();
            Name = Open;
            _isOpen = false;
        }

        yield return new WaitForSeconds(_duration + 1);

        _npcInteractable = true;
    }

    public void SetInteractable(bool value)
    {
        _isInteractable = value;
        _outline.enabled = false;
    }

    public void EnableOutline()
    {
        if (!_isInteractable)
        {
            return;
        }

        _outline.enabled = true;
    }

    public void DisableOutline()
    {
        if (!_isInteractable)
        {
            return;
        }

        _outline.enabled = false;
    }
}
