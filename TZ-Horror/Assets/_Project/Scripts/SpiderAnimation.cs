using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _spider;
    [SerializeField] private CoffeeMachine _coffeeMachine;

    public bool _showed = false;

    private void OnEnable()
    {
        _coffeeMachine.OnPour += ShowSpider;
    }

    private void OnDisable()
    {
        _coffeeMachine.OnPour -= ShowSpider;
    }

    private void ShowSpider()
    {
        if (!_showed)
        {
            _showed = true;
            _spider.gameObject.SetActive(true);
            StartCoroutine(SpiderRoutine());
        }
    }

    private IEnumerator SpiderRoutine()
    {
        yield return new WaitForSeconds(10);
        _spider.gameObject.SetActive(false);
    }
}
