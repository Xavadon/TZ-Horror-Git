using UnityEngine;

public class InteractionHighlighter : MonoBehaviour
{
    [SerializeField] private float _highlightDistance = 5f;
    [SerializeField] private LayerMask _highlightLayer;

    private Camera _mainCamera;
    private ItemInfo _itemInfo;
    private GameObject _currentTarget;
    private IInteractable _lastOutlinable;
    private ItemManipulator _itemManipulator;
    private bool _canDetect = true;

    private void OnEnable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueStart += (_, _, _) => _canDetect = false;
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += (_) => _canDetect = true;
    }

    private void OnDisable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueStart -= (_, _, _) => _canDetect = false;
        NewDialogueSystem.DialogueSystem.OnDialogueFinished -= (_) => _canDetect = true;
    }

    public void Construct(ItemInfo itemInfo, ItemManipulator itemManipulator)
    {
        _itemInfo = itemInfo;
        _itemManipulator = itemManipulator;
        _mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!_itemManipulator.IsHoldingItem && _canDetect)
        {
            DetectOutline();
        }
        else if (_currentTarget != null)
        {
            DisableOutline();
        }
    }

    private void DetectOutline()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, _highlightDistance, _highlightLayer))
        {
            if (_lastOutlinable != null)
            {
                DisableOutline();
            }

            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        if (_currentTarget == hitObject)
        {
            return;
        }

        if (_lastOutlinable != null)
        {
            DisableOutline();
        }

        _currentTarget = hitObject;

        if (_currentTarget.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
        {
            interactable.EnableOutline();
            _lastOutlinable = interactable;
            _itemInfo.UpdateText(interactable.Name);
        }
    }

    public void DisableOutline()
    {
        if (_lastOutlinable != null)
        {
            _lastOutlinable.DisableOutline();
        }

        _lastOutlinable = null;
        _currentTarget = null;
        _itemInfo.DisableText();
    }
}
