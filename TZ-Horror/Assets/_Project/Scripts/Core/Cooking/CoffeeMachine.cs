using System;
using System.Collections;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private ItemPlacementZone _cupPlacement;
    [SerializeField] private ItemPlacementZone _capsulePlacement;
    [SerializeField] private ItemPlacementZone _lidPlacement;
    [SerializeField] private GameObject _effect;
    [SerializeField] private InteractabeButton _button;
    [SerializeField] private float _pourDuration = 5f;
    [SerializeField] private GameObject _coffeePrefab;

    private bool _isPouring;
    private bool _isReady;
    private Item _cup;
    private Item _capsule;
    private Item _lid;

    public event Action OnPour;
    public event Action OnComplete;

    private void Awake()
    {
        _lidPlacement.SetInteractAble(false);
    }

    private void OnEnable()
    {
        _cupPlacement.OnItemPlaced += HandleCupPlaced;
        _capsulePlacement.OnItemPlaced += HandleCapsulePlaced;
        _lidPlacement.OnItemPlaced += HandleLidPlaced;
        _button.OnPress += HandleBrewButtonPress;
    }

    private void OnDisable()
    {
        _cupPlacement.OnItemPlaced -= HandleCupPlaced;
        _capsulePlacement.OnItemPlaced -= HandleCapsulePlaced;
        _lidPlacement.OnItemPlaced -= HandleLidPlaced;
        _button.OnPress -= HandleBrewButtonPress;
    }

    private void HandleCupPlaced(Item item)
    {
        _cup = item;
    }

    private void HandleCapsulePlaced(Item item)
    {
        _capsule = item;
    }

    private void HandleLidPlaced(Item item)
    {
        _lid = item;

        GameObject coffee = Instantiate(_coffeePrefab, _cupPlacement.transform.position, Quaternion.identity);
        coffee.transform.rotation = _cup.transform.rotation;

        if (coffee.TryGetComponent(out Rigidbody rigidbody))
            rigidbody.useGravity = false;

        _effect.SetActive(false);


        _cupPlacement.ResetZone();
        _capsulePlacement.ResetZone();
        StartCoroutine(DelayedReset(_lidPlacement, false));

        _cup = null;
        _capsule = null;
        _lid = null;

        _lidPlacement.SetInteractAble(false);
        _isReady = false;


        OnComplete?.Invoke();
    }

    private IEnumerator DelayedReset(ItemPlacementZone zone, bool resetInteraction)
    {
        yield return null;
        zone.ResetZone(resetInteraction);
    }

    private void HandleBrewButtonPress()
    {
        if (_cup == null || _capsule == null || _isPouring || _isReady)
            return;

        StartCoroutine(PourRoutine());
    }

    private IEnumerator PourRoutine()
    {
        OnPour?.Invoke();

        _isPouring = true;
        _effect.SetActive(true);

        yield return new WaitForSeconds(_pourDuration);

        _isPouring = false;
        _isReady = true;

        _lidPlacement.SetInteractAble(true);
    }

    private void DestroyIfValid(Item item)
    {
        if (item != null && item.gameObject != null)
        {
            item.gameObject.SetActive(false);
        }
    }
}
