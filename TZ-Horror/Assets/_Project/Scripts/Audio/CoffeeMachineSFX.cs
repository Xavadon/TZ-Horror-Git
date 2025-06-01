using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachineSFX : MonoBehaviour
{
    private CoffeeMachine _coffeeMachine;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        if (_coffeeMachine == null)
        {
            _coffeeMachine = GetComponent<CoffeeMachine>();
        }

        _coffeeMachine.OnPour += PlaySFX;
    }

    private void OnDisable()
    {
        _coffeeMachine.OnPour -= PlaySFX;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void PlaySFX()
    {
        _audioSource.Play();
    }
}
