using System;
using UnityEngine;

public class ItemManipulator : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private float _pickupRange = 5f;
    [SerializeField] private float _moveForce = 15f;
    [SerializeField] private LayerMask _pickupLayer;

    private Camera _mainCamera;
    private Rigidbody _heldObject;
    private float _throwForce = 500f;
    private Quaternion _heldObjectRotation;
    public bool IsHoldingItem => _heldObject != null;

    private void OnEnable()
    {
        PlayerHandsDropper.OnDrop += DropObject;
    }

    private void OnDisable()
    {
        PlayerHandsDropper.OnDrop -= DropObject;
    }

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleInput();

        if (_heldObject != null)
        {
            MoveHeldObject();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                ThrowObject();
            }
        }

        if (Input.GetMouseButtonDown(1) && _heldObject != null)
        {
            DropObject();
        }
    }

    private void MoveHeldObject()
    {
        Vector3 direction = _holdPoint.position - _heldObject.position;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(_heldObject.position, direction.normalized, out hit, distance, _pickupLayer, QueryTriggerInteraction.Ignore))
        {
            _heldObject.position = hit.point - direction.normalized * 0.1f;
        }
        else
        {
            Vector3 targetPosition = _holdPoint.position;
            _heldObject.transform.position = Vector3.Lerp(_heldObject.transform.position, targetPosition, Time.deltaTime * _moveForce);
        }

        _heldObject.rotation = Quaternion.Slerp(_heldObject.rotation, _heldObjectRotation, Time.deltaTime * _moveForce);
    }

    private void TryPickupObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, _pickupRange, _pickupLayer))
        {
            return;
        }

        if (hit.transform.TryGetComponent(out Rigidbody rb))
        {
            _heldObject = rb;
            _heldObject.useGravity = false;
            _heldObject.drag = 10f;
            _heldObjectRotation = _heldObject.rotation;
        }
    }

    private void DropObject()
    {
        if (_heldObject != null)
        {
            _heldObject.useGravity = true;
            _heldObject.drag = 0f;
            _heldObject = null;
        }
    }

    private void ThrowObject()
    {
        _heldObject.useGravity = true;
        _heldObject.drag = 0f;
        _heldObject.AddForce(_mainCamera.transform.forward * _throwForce);
        _heldObject = null;
    }
}

public static class PlayerHandsDropper
{
    public static event Action OnDrop;

    public static void DropItem()
    {
        OnDrop?.Invoke();
    }
}