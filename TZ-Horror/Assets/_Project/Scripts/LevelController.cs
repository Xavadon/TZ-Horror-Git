using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private readonly string StartQuest = "Go to the bar and close bar opening";
    private readonly string FirstCustomer = "Talk to the customer to accept the order";
    private readonly string Coffee1 = "Place a cup in the coffee machine, insert a coffee capsule, and press the brew button";
    private readonly string Coffee2 = "Close the cup with a lid";
    private readonly string Coffee3 = "Give the coffee to the customer";
    private readonly string Coffee4 = "Serve the customers.";
    private readonly string Phone = "Answer the phone";

    [SerializeField] private PlayerCheckerCollider _playerChecker;
    [SerializeField] private NPC _firstNPC;
    [SerializeField] private CoffeeMachine _coffeeMachine;
    [SerializeField] private InteractabeDialogueTrigger _phone;
    [SerializeField] private AudioSource _radio;
    [SerializeField] private GameObject _clown;

    private TMP_Text _questText;
    private InteractableDoor _barOpening;
    private NPCSpawner _spawner;

    public void Construct(TMP_Text questText, InteractableDoor barOpening, NPCSpawner spawner)
    {
        _questText = questText;
        _barOpening = barOpening;
        _spawner = spawner;

        _barOpening.OnClose += StartNPCSpawner;
        _questText.text = StartQuest;

        _phone.SetInteractAble(false);
        _phone.SetOneShot(true);
        _spawner.OnAllCompleted += HandleLevelComplete;
        NewDialogueSystem.DialogueSystem.OnDialogueFinished += HandleDialogueEnd;
    }

    private void StartNPCSpawner()
    {
        if (_playerChecker.IsPlayerHere)
        {
            _barOpening.OnClose -= StartNPCSpawner;
            _spawner.MoveNextNPC();
            _questText.text = "";
            _playerChecker.gameObject.SetActive(false);
            _barOpening.SetInteractable(false);
            _firstNPC.OnReach += npcTip;
        }
    }

    private void npcTip()
    {
        _firstNPC.OnReach -= npcTip;
        _questText.text = FirstCustomer;

        NewDialogueSystem.DialogueSystem.OnDialogueFinished += CoffeeTip1;
    }

    private void CoffeeTip1(string key)
    {
        if (key == "order1")
        {
            NewDialogueSystem.DialogueSystem.OnDialogueFinished -= CoffeeTip1;
            _questText.text = Coffee1;
            _coffeeMachine.OnPour += CoffeeTip2;
        }
    }

    private void CoffeeTip2()
    {
        _coffeeMachine.OnPour -= CoffeeTip2;
        _questText.text = Coffee2;
        _coffeeMachine.OnComplete += CoffeeTip3;
    }

    private void CoffeeTip3()
    {
        _coffeeMachine.OnComplete -= CoffeeTip3;
        _questText.text = Coffee3;
        _firstNPC.OnGetItem += CoffeeTip4;
    }

    private void CoffeeTip4()
    {
        _firstNPC.OnGetItem -= CoffeeTip4;
        _questText.text = Coffee4;
    }

    private void HandleLevelComplete()
    {
        _spawner.OnAllCompleted -= HandleLevelComplete;
        _phone.SetInteractAble(true);
        _phone.GetComponent<AudioSource>().Play();
        _barOpening.SetInteractable(true);
        _questText.text = Phone;
        _radio.pitch = 0.6f;

        _phone.OnInteract += DisableNotes;
    }

    private void DisableNotes()
    {
        _questText.text = "";
    }

    private void HandleDialogueEnd(string key)
    {
        if (key == "phoneCall")
        {
            _phone.SetInteractAble(false);
            _clown.gameObject.SetActive(true);
        }
    }
}