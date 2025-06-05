using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class ClownScreamer : MonoBehaviour
{
    private readonly string dialogueKey = "suddenMan";

    [SerializeField] private Transform _lookAt;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Renderer _objectRenderer; // Рендерер объекта (MeshRenderer/SpriteRenderer)

    private AudioSource _audioSource;
    private bool _screamerStarted;

    public event Action OnComplete;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        // Автоматически находим рендерер, если не задан
        if (_objectRenderer == null)
            _objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_screamerStarted || _objectRenderer == null || _mainCamera == null)
            return;

        // Проверяем видимость объекта (с учётом его границ)
        if (IsObjectVisible())
        {
            _screamerStarted = true;
            ShowScreamer();
        }
    }

    // Проверяет, виден ли объект (хотя бы частично)
    private bool IsObjectVisible()
    {
        Bounds bounds = _objectRenderer.bounds;
        Vector3[] corners = GetBoundsCorners(bounds);

        foreach (Vector3 corner in corners)
        {
            Vector3 viewportPos = _mainCamera.WorldToViewportPoint(corner);

            if (viewportPos.x >= 0 && viewportPos.x <= 1 &&
                viewportPos.y >= 0 && viewportPos.y <= 1 &&
                viewportPos.z > 0)
            {
                return true; // Хотя бы один угол в поле зрения
            }
        }

        return false;
    }

    // Возвращает 8 углов Bounds (3D) или 4 угла (2D)
    private Vector3[] GetBoundsCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.min; // Нижний-левый-ближний
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[7] = bounds.max; // Верхний-правый-дальний
        return corners;
    }

    private void ShowScreamer()
    {
        _objectRenderer.material.color = Color.black;
        _objectRenderer.material.DOColor(Color.white, 4).SetDelay(1);

        NewDialogueSystem.DialogueSystem.StartDialogue(dialogueKey, _lookAt, 0.25f);
        _audioSource.Play();
    }
}