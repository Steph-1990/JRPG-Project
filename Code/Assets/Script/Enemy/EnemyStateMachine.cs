using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE,
    ROAMING,
    CHASING,
}

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyState _currentState;
    [SerializeField] private BoolVariable _collision;

    private EnemyController _enemyController;
    private Animator _animator;

    public EnemyState CurrentState { get => _currentState; set => _currentState = value; }

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnStateEnter(EnemyState.IDLE);
    }

    private void Update()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                OnStateEnterIdle();
                break;

            case EnemyState.ROAMING:
                OnStateEnterRoaming();
                break;

            case EnemyState.CHASING:
                OnStateEnterChasing();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateUpdate(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                OnStateUpdateIdle();
                break;

            case EnemyState.ROAMING:
                OnStateUpdateRoaming();
                break;

            case EnemyState.CHASING:
                OnStateUpdateChasing();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateExit(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                OnStateExitIdle();
                break;

            case EnemyState.ROAMING:
                OnStateExitRoaming();
                break;

            case EnemyState.CHASING:
                OnStateExitChasing();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void TransitionToState(EnemyState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void OnStateEnterIdle()
    {
        _animator.SetTrigger("Idle");
        _enemyController.EnterIdleTime = Time.time; // On enregistre le temps à l'entrée du mode Idle
        _enemyController.WayPointReached = false;
    }

    private void OnStateUpdateIdle()
    {
        if (_enemyController.PlayerWithinAttackRange() && !_collision.value) // Si le player est à portée d'attaque de l'ennemi
        {
            TransitionToState(EnemyState.CHASING);
        }
        else if (_enemyController.IdleTimerEnded() && !_collision.value) // Si le timer du mode idle est terminé
        {
            TransitionToState(EnemyState.ROAMING);
        }
    }

    private void OnStateExitIdle()
    {

    }

    private void OnStateEnterRoaming()
    {
        _animator.SetTrigger("Roam");

    }

    private void OnStateUpdateRoaming()
    {
        if (_enemyController.PlayerWithinAttackRange()) // Si le player est à portée d'attaque de l'ennemi
        {
            TransitionToState(EnemyState.CHASING);
        }
        else if (_enemyController.WayPointReached || _collision.value) // Si un way point est atteint par l'ennemi
        {
            TransitionToState(EnemyState.IDLE);
        }
    }

    private void OnStateExitRoaming()
    {

    }

    private void OnStateEnterChasing()
    {
        _animator.SetTrigger("Chase");
    }

    private void OnStateUpdateChasing()
    {
        if (_collision.value)
        {
            TransitionToState(EnemyState.IDLE);
        }
        if (!_enemyController.PlayerWithinAttackRange()) // Si le player n'est plus à portée d'attaque de l'ennemi
        {
            TransitionToState(EnemyState.ROAMING);
        }
    }

    private void OnStateExitChasing()
    {

    }
}
