using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _moveTo;
    [SerializeField] private Transform _moveOutTo;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private ItemPlacementZone _itemPlacementZone;
    [SerializeField] private InteractabeDialogueTrigger _dialogueTrigger;

    private bool _reached = false;

    public event Action OnComplete;

    private void OnEnable()
    {
        _itemPlacementZone.OnItemPlaced += GoAway;
        _dialogueTrigger.OnInteract += () => _itemPlacementZone.SetInteractAble(true);
    }

    private void OnDisable()
    {
        _itemPlacementZone.OnItemPlaced -= GoAway;
        _dialogueTrigger.OnInteract -= () => _itemPlacementZone.SetInteractAble(true);
    }

    public void Construct(int dialogueIndex)
    {
        _animationController = new(_animator, _agent);
        _dialogueTrigger.Construct(dialogueIndex);
        _dialogueTrigger.SetInteractAble(false);
        _itemPlacementZone.SetInteractAble(false);
    }

    public void Init()
    {
        _agent.SetDestination(_moveTo.position);
    }

    private void Update()
    {
        _animationController.Update();

        Vector2 position = transform.position;
        position.y = 0;

        Vector2 target = _moveTo.position;
        target.y = 0;

        if (Vector2.Distance(position, target) <= 0.1f && !_reached)
        {
            _reached = true;

            _dialogueTrigger.SetInteractAble(true);
        }
    }

    private void GoAway(Item obj)
    {
        Destroy(obj.gameObject);
        _agent.SetDestination(_moveOutTo.position);
        _dialogueTrigger.DisableOutline();
        _dialogueTrigger.SetInteractAble(false);
    }
}

public class AnimationController
{
    private readonly int Velocity = Animator.StringToHash("Velocity");
    private readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    private Animator _animator;
    private NavMeshAgent _agent;

    public AnimationController(Animator animator, NavMeshAgent agent)
    {
        _animator = animator;
        _agent = agent;

        _animator.SetFloat(MoveSpeed, _agent.speed / 3f);
    }

    public void Update()
    {
        float velocity = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat(Velocity, velocity);
    }

    public void CrossFade(string key)
    {
        _animator.CrossFade(key, 0.1f);
    }
}
