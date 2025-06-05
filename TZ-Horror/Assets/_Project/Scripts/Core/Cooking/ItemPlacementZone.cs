using System;
using System.Collections;
using UnityEngine;

public class ItemPlacementZone : MonoBehaviour
{
    [SerializeField] private ItemType _targetType;

    private float _moveSpeed = 30f;
    private float _rotateSpeed = 10f;
    private bool _isOccupied;
    private bool _interactable = true;
    private Item _item;

    public event Action<Item> OnItemStartPlacing;
    public event Action<Item> OnItemPlaced;

    private void OnTriggerStay(Collider other)
    {
        if (_isOccupied || !_interactable)
            return;

        if (other.TryGetComponent(out Item item) && item.GetItemType() == _targetType)
        {
            _interactable = false;
            _isOccupied = true;
            StartCoroutine(SmoothSnapItem(item.gameObject));
            _item = item;
            PlayerHandsDropper.DropItem();
            OnItemStartPlacing?.Invoke(item);
        }
    }

    private IEnumerator SmoothSnapItem(GameObject item)
    {
        if (item.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }

        if (item.TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }

        Transform itemTransform = item.transform;
        Vector3 targetPosition = transform.position;
        Quaternion targetRotation = transform.rotation;

        float threshold = 0.01f;

        while (Vector3.Distance(itemTransform.position, targetPosition) > threshold || Quaternion.Angle(itemTransform.rotation, targetRotation) > threshold)
        {
            itemTransform.position = Vector3.Lerp(itemTransform.position, targetPosition, Time.deltaTime * _moveSpeed);
            itemTransform.rotation = Quaternion.Lerp(itemTransform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
            yield return null;
        }

        itemTransform.position = targetPosition;
        itemTransform.rotation = targetRotation;

        OnItemPlaced?.Invoke(_item);
    }

    public void ResetZone(bool resetInteraction = true)
    {
        Debug.Log($"[DESTROY] Item name: {_item.gameObject.name} null:{_item == null}");

        if (_item != null)
        {
            Destroy(_item.gameObject);
            _item = null;
        }

        if (resetInteraction)
        {
            _interactable = true;
        }

        _isOccupied = false;
    }

    public void SetInteractAble(bool value)
    {
        _interactable = value;
    }
}
