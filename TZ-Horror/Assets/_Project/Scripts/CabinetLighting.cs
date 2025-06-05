using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetLighting : MonoBehaviour
{
    [SerializeField] private ManualLightBlink[] _lightBlink;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueStart += HandleDialogueStart;
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        NewDialogueSystem.DialogueSystem.OnDialogueStart -= HandleDialogueStart;
    }

    private void HandleDialogueEnd(string key)
    {
        if (key == "phoneCall")
        {
            _audioSource.Play();
        }
    }

    private void HandleDialogueStart(string key, Transform arg2, float arg3)
    {
        if (key == "phoneCall")
        {
            StartCoroutine(StartBlinking());
        }
    }

    private IEnumerator StartBlinking()
    {
        yield return new WaitForSeconds(1);
        _lightBlink[0].Switch(false);
        yield return new WaitForSeconds(0.2f);
        _lightBlink[1].Switch(false);
        yield return new WaitForSeconds(0.3f);
        _lightBlink[0].Switch(true);
        yield return new WaitForSeconds(0.15f);
        _lightBlink[1].Switch(true);
        _lightBlink[0].Switch(false);
        yield return new WaitForSeconds(0.35f);
        _lightBlink[0].Switch(true);
        yield return new WaitForSeconds(0.3f);
        _lightBlink[1].Switch(false);
        yield return new WaitForSeconds(0.2f);
        _lightBlink[1].Switch(true);
        yield return new WaitForSeconds(0.15f);
        _lightBlink[0].Switch(false);
        yield return new WaitForSeconds(0.3f);
        _lightBlink[0].Switch(true);
    }
}
