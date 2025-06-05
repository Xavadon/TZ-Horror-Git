using UnityEngine;

public class ManualLightBlink : MonoBehaviour
{
    [SerializeField] private Light[] _light;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _materials;

    public void Switch(bool value)
    {
        foreach (var light in _light)
        {
            light.enabled = value;
        }

        if (value)
        {
            _renderer.material = _materials[0];
        }
        else
        {
            _renderer.material = _materials[1];
        }
    }
}
