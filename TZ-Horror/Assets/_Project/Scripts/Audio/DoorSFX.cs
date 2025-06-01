using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSFX : MonoBehaviour
{
    [SerializeField] private AudioClip _openSFX;
    [SerializeField] private AudioClip _closeSFX;

    private InteractableDoor _interactable;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        if (_interactable == null)
        {
            _interactable = GetComponent<InteractableDoor>();
        }

        _interactable.OnOpen += PlayOpenSoundEffect;
        _interactable.OnClose += PlayCloseSoundEffect;
    }

    private void OnDisable()
    {
        _interactable.OnOpen -= PlayOpenSoundEffect;
        _interactable.OnClose -= PlayCloseSoundEffect;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void PlayOpenSoundEffect()
    {
        _audioSource.clip = _openSFX;
        _audioSource.Play();
    }
    
    private void PlayCloseSoundEffect()
    {
        _audioSource.clip = _closeSFX;
        _audioSource.Play();
    }
}
