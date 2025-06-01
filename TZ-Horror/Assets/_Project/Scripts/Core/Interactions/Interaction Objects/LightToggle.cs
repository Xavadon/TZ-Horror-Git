using QuickOutline;
using UnityEngine;

public class LightToggle : MonoBehaviour, IInteractable, IOutlinable
{
    [SerializeField] private GameObject _light;

    private Outline _outline;
    private bool _toggled;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        if (_toggled)
        {
            transform.localScale = Vector3.one * 0.5f;
            _light.gameObject.SetActive(true);
            _toggled = false;
        }
        else
        {
            transform.localScale = Vector3.one * 0.25f;
            _light.gameObject.SetActive(false);
            _toggled = true;
        }
    }

    public void DisableOutline()
    {
        _outline.SetOutlineWidth(0);
    }

    public void EnableOutline()
    {
        _outline.SetOutlineWidth(3);
    }
}