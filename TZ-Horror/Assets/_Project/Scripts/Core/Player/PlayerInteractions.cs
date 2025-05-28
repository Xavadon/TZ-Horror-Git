using UnityEngine;

public class PlayerInteractions : MonoBehaviour 
{
    [SerializeField] private float _pickupRange = 5f;
    [SerializeField] private LayerMask _interactionLayer;

    private Camera _mainCamera;

    public void Construct()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
