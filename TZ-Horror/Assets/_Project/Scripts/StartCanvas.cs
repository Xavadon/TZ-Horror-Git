using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EvolveGames.PlayerController _playerController;

    private void Start()
    {
        _canvasGroup.DOFade(0, 1).SetDelay(3);
        _playerController.SetCanMove(false);
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(4);
        _playerController.SetCanMove(true);
    }
}
