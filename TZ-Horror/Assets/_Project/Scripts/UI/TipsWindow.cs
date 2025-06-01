using UnityEngine;

public class TipsWindow : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private ItemManipulator _itemManipulator;

    private void Awake()
    {
        _window.SetActive(false);
    }

    private void Update()
    {
        bool isActive = _itemManipulator != null && _itemManipulator.IsHoldingItem;
        if (_window.activeSelf != isActive)
        {
            _window.SetActive(isActive);
        }
    }
}
