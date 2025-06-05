using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreamer : MonoBehaviour
{
    [SerializeField] private Image _screamerImage;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(10);
        _audioSource.Play();
        yield return new WaitForSeconds(0.4f);
        _screamerImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
