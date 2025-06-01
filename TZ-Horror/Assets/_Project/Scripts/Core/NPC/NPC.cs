using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _pathPoints;
    [SerializeField] private Transform _moveOutTo;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private ItemPlacementZone _itemPlacementZone;
    [SerializeField] private InteractabeDialogueTrigger _dialogueTrigger;

    private StateMachine _stateMachine;
    private int _currentPointIndex;
    private bool _completed;

    public event Action OnComplete;

    private void OnEnable()
    {
        _itemPlacementZone.OnItemPlaced += HandleItemPlaced;
        _dialogueTrigger.OnInteract += EnableItemPlacement;
    }

    private void OnDisable()
    {
        _itemPlacementZone.OnItemPlaced -= HandleItemPlaced;
        _dialogueTrigger.OnInteract -= EnableItemPlacement;
    }

    public void Construct(int dialogueIndex, Transform playerTransform)
    {
        _animationController = new AnimationController(_animator, _agent);

        _dialogueTrigger.Construct(dialogueIndex);

        _stateMachine = new StateMachine();
        _stateMachine.AddState("WalkPath", new WalkPathState(this, _agent, _pathPoints, _stateMachine));
        _stateMachine.AddState("Wait", new WaitState(_dialogueTrigger, transform));
        _stateMachine.AddState("Leave", new LeaveState(this, _agent, _moveOutTo));
    }

    public void Init()
    {
        if (_pathPoints.Length == 0)
            return;

        _dialogueTrigger.SetInteractAble(false);
        _itemPlacementZone.SetInteractAble(false);

        _currentPointIndex = 0;
        _stateMachine.ChangeState("WalkPath");
    }

    private void Update()
    {
        _animationController.Update();
        _stateMachine?.Update();
    }

    private void EnableItemPlacement()
    {
        _itemPlacementZone.SetInteractAble(true);
    }

    private void HandleItemPlaced(Item item)
    {
        Destroy(item.gameObject);
        _dialogueTrigger.DisableOutline();
        _dialogueTrigger.SetInteractAble(false);
        _stateMachine.ChangeState("Leave");
        CurrencySystem.Increace(200);
    }

    public void Complete()
    {
        if (_completed)
            return;

        _completed = true;
        OnComplete?.Invoke();
        Destroy(gameObject);
    }

    public bool HasReached(Vector3 target)
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        target.y = 0;

        return Vector3.Distance(pos, target) <= 0.1f;
    }

    public Vector3 GetNextPathPoint()
    {
        _currentPointIndex++;
        return _pathPoints[_currentPointIndex].position;
    }

    public bool IsLastPoint()
    {
        return _currentPointIndex + 1 >= _pathPoints.Length;
    }
}

public class StateMachine
{
    private IState _currentState;
    private Dictionary<string, IState> _states = new();

    public void AddState(string key, IState state)
    {
        _states[key] = state;
    }

    public void ChangeState(string key)
    {
        _currentState?.Exit();
        _currentState = _states[key];
        _currentState.Enter();

        Debug.Log($"STATE:{key}");
    }

    public void Update()
    {
        _currentState?.Update();
    }
}

public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class WalkPathState : IState
{
    private StateMachine _stateMachine;
    private NPC _npc;
    private NavMeshAgent _agent;
    private Transform[] _points;
    private Vector3 _currentDestination;

    public WalkPathState(NPC npc, NavMeshAgent agent, Transform[] points, StateMachine stateMachine)
    {
        _npc = npc;
        _agent = agent;
        _points = points;
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        if (_points.Length == 0)
            return;

        _currentDestination = _points[0].position;
        _agent.SetDestination(_points[0].position);
    }

    public void Update()
    {
        if (_points.Length == 0)
            return;

        if (_npc.HasReached(_currentDestination))
        {
            if (_npc.IsLastPoint())
            {
                _stateMachine.ChangeState("Wait");
                return;
            }

            _currentDestination = _npc.GetNextPathPoint();
            _agent.SetDestination(_currentDestination);
        }
    }

    public void Exit() { }
}

public class WaitState : IState
{
    private InteractabeDialogueTrigger _trigger;
    private Transform _transform;

    public WaitState(InteractabeDialogueTrigger trigger, Transform transform)
    {
        _trigger = trigger;
        _transform = transform;
    }

    public void Enter()
    {
        _trigger.SetInteractAble(true);
        _transform.DORotateQuaternion(Quaternion.identity, 1);
    }

    public void Update() { }

    public void Exit() { }
}

public class LeaveState : IState
{
    private NPC _npc;
    private NavMeshAgent _agent;
    private Transform _target;

    public LeaveState(NPC npc, NavMeshAgent agent, Transform target)
    {
        _npc = npc;
        _agent = agent;
        _target = target;
    }

    public void Enter()
    {
        _agent.SetDestination(_target.position);
    }

    public void Update()
    {
        if (_npc.HasReached(_target.position))
        {
            _npc.Complete();
        }
    }

    public void Exit() { }
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
