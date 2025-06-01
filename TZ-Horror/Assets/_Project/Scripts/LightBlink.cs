using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    [SerializeField] private Light _lightA;
    [SerializeField] private Light _lightB;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private float _minBlinkDelay = 0.1f;
    [SerializeField] private float _maxBlinkDelay = 1.5f;
    [SerializeField] private float _lightOnDuration = 0.2f;

    private float _timer;
    private bool _isBlinking;

    private void Start()
    {
        _timer = GetNextDelay();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f && !_isBlinking)
        {
            StartCoroutine(Blink());
            _timer = GetNextDelay();
        }
    }

    private IEnumerator Blink()
    {
        _isBlinking = true;

        _renderer.material = _materials[1];

        _lightA.enabled = true;
        _lightB.enabled = true;

        yield return new WaitForSeconds(_lightOnDuration);

        _lightA.enabled = false;
        _lightB.enabled = false;
        _renderer.material = _materials[0];

        _isBlinking = false;
    }

    private float GetNextDelay()
    {
        return Random.Range(_minBlinkDelay, _maxBlinkDelay);
    }
}
