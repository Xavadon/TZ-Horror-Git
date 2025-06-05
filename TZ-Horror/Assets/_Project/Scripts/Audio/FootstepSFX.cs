using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footsteps;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSFX(AnimationEvent animationEvent)
    {
        int index = Random.Range(0, _footsteps.Length);
        _audioSource.PlayOneShot(_footsteps[index]);
    }
}
