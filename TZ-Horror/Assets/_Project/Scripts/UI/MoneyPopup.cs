using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _popupText;
    [SerializeField] private float _moveUpDistance = 50f;
    [SerializeField] private float _fadeDuration = 1f;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        MoneyPopupService.OnShow += ShowPopup;
    }

    private void OnDisable()
    {
        MoneyPopupService.OnShow -= ShowPopup;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public void ShowPopup(int amount)
    {
        _popupText.text = $"+{amount}$";
        _canvasGroup.alpha = 1f;
        _rectTransform.anchoredPosition = Vector2.zero;
        StopAllCoroutines();
        StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float elapsedTime = 0f;
        Vector2 startPos = _rectTransform.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, _moveUpDistance);

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;
            _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            _canvasGroup.alpha = 1f - t;
            yield return null;
        }

        _rectTransform.anchoredPosition = startPos;
    }
}

public static class MoneyPopupService
{
    public static event Action<int> OnShow;
    
    public static void ShowPopup(int amount)
    {
        OnShow?.Invoke(amount);
    }
}