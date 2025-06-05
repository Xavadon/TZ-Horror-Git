using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _answerClip;

    private InteractabeDialogueTrigger _dialogueTrigger;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _dialogueTrigger = GetComponent<InteractabeDialogueTrigger>();
        _dialogueTrigger.OnInteract += AnswerSFX;

        _audioSource = GetComponent<AudioSource>();

        NewDialogueSystem.DialogueSystem.OnDialogueFinished += HandleDialogueFinish;
    }

    private void HandleDialogueFinish(string key)
    {
        if (key == "phoneCall")
        {
            PlayAnswerSFX();
            NewDialogueSystem.DialogueSystem.OnDialogueFinished -= HandleDialogueFinish;
        }
    }

    private void AnswerSFX()
    {
        _dialogueTrigger.OnInteract -= AnswerSFX;
        PlayAnswerSFX();
    }

    private void PlayAnswerSFX()
    {
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.clip = _answerClip;
        _audioSource.Play();
    }
}
