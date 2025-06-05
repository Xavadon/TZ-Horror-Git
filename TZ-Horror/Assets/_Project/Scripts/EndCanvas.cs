using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private EvolveGames.PlayerController _playerController;

    private void OnEnable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += HandleDialogueFinished;
    }

    private void OnDisable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueFinished -= HandleDialogueFinished;
    }

    private void HandleDialogueFinished(string key)
    {
        if (key == "suddenMan")
        {
            _playerController.SetCanMove(false);
            _canvasGroup.DOFade(1, 1).From(0);
            StartCoroutine(EndRoutine());
        }
    }

    private IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
