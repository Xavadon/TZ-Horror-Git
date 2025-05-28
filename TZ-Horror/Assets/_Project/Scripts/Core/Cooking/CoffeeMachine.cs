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

    private void Awake()
    {
        _lidPlacement.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _cupPlacement.OnItemPlaced += OnCupPlaced;
        _capsulePlacement.OnItemPlaced += OnCapsulePlaced;
        _lidPlacement.OnItemPlaced += OnLidPlaced;
        _button.OnPress += PressBrewButton;
    }

    private void OnDisable()
    {
        _cupPlacement.OnItemPlaced -= OnCupPlaced;
        _capsulePlacement.OnItemPlaced -= OnCapsulePlaced;
        _lidPlacement.OnItemPlaced -= OnLidPlaced;
        _button.OnPress -= PressBrewButton;
    }

    private void OnCupPlaced(Item item)
    {
        _cup = item;
        //_cupPlacement.gameObject.SetActive(false);
    }

    private void OnCapsulePlaced(Item item)
    {
        _capsule = item;
        //_capsulePlacement.gameObject.SetActive(false);
    }

    private void OnLidPlaced(Item item)
    {
        _lid = item;
        var coffee = Instantiate(_coffeePrefab, _cupPlacement.transform.position, Quaternion.identity);
        coffee.transform.rotation = _cup.transform.rotation;

        if (coffee.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.useGravity = false;
        }

        Destroy(_cup.gameObject);
        Destroy(_capsule.gameObject);
        Destroy(_lid.gameObject);

        _cupPlacement.ResetZone();
        _capsulePlacement.ResetZone();
        _lidPlacement.ResetZone();

        _lidPlacement.gameObject.SetActive(false);

        _isReady = false;
    }

    public void PressBrewButton()
    {
        if (_cup == null) return;
        if (_capsule == null) return;
        if (_isPouring) return;
        if (_isReady) return;

        StartCoroutine(PourCoffee());
    }

    private IEnumerator PourCoffee()
    {
        _isPouring = true;
        _effect.SetActive(true);

        yield return new WaitForSeconds(_pourDuration);

        _effect.SetActive(false);
        _isPouring = false;
        _isReady = true;
        _lidPlacement.gameObject.SetActive(true);
    }
}
