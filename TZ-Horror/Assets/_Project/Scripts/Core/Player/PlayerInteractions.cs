using UnityEngine;

public class PlayerInteractions : MonoBehaviour 
{
    [SerializeField] private float _pickupRange = 5f;
    [SerializeField] private LayerMask _interactionLayer;

    private Camera _mainCamera;
    private ItemManipulator _itemManipulator;

    public void Construct(ItemManipulator itemManipulator)
    {
        _mainCamera = GetComponent<Camera>();
        _itemManipulator = itemManipulator;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_itemManipulator.IsHoldingItem)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, _pickupRange))
        {
            return;
        }

        if (hit.transform.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }
}
