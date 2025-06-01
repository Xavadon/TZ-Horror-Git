using DG.Tweening;
using QuickOutline;
using System;
using UnityEngine;

public class InteractableDoor : MonoBehaviour, IInteractable, IOutlinable
{
    [SerializeField] private Vector3 _openRotationOffset = new Vector3(0f, -90f, 0f);
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private bool _isOpen = false;

    private Outline _outline;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    public event Action OnOpen;
    public event Action OnClose;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _closedRotation = transform.rotation;
        Vector3 targetEuler = _closedRotation.eulerAngles + _openRotationOffset;
        _openRotation = Quaternion.Euler(targetEuler);
        DisableOutline();
    }

    public void Interact()
    {
        if (_isOpen)
        {
            transform.DORotateQuaternion(_closedRotation, _duration);
            OnClose?.Invoke();
        }
        else
        {
            transform.DORotateQuaternion(_openRotation, _duration);
            OnOpen?.Invoke();
        }

        _isOpen = !_isOpen;
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }
}
