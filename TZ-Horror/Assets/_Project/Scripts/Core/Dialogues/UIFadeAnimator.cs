using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeAnimator : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 2f;
    
    [Space(height: 10)]
    [SerializeField] private Image[] _images;
    [SerializeField] private TMP_Text[] _textFields;

    public event Action OnEnableComplete;
    public event Action OnDisableComplete;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        UpdateAlpha(0);
    }

    public void ToggleMenu(bool isVisible)
    {
        StopAllCoroutines();
        StartCoroutine(isVisible ? FadeInMenu() : FadeOutMenu());
    }

    private IEnumerator FadeInMenu()
    {
        yield return FadeMenu(0f, 1f);
        OnEnableComplete?.Invoke();
        IsActive = true;
    }

    private IEnumerator FadeOutMenu()
    {
        yield return FadeMenu(1f, 0f);
        OnDisableComplete?.Invoke();
        IsActive = false;
    }

    private IEnumerator FadeMenu(float startAlpha, float targetAlpha)
    {
        float alpha = startAlpha;

        while (!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime * _fadeSpeed);
            UpdateAlpha(alpha);
            yield return null;
        }
    }

    private void UpdateAlpha(float alpha)
    {
        foreach (var image in _images)
        {
            SetAlpha(image, alpha);
        }

        foreach (var field in _textFields)
        {
            SetAlpha(field, alpha);
        }
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        var color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}
