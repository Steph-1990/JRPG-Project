using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    IDLE,
    WALKING,
    SPRINTING,
}

public class MovementStateMachine : MonoBehaviour
{
    [SerializeField] private MovementState _currentState;
    [SerializeField] private BoolVariable _collision;

    private PlayerController _playerController;
    private PlayerInput _playerInput;
    private Animator _animator;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {      
        OnStateEnter(MovementState.IDLE);
    }

    private void Update()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateEnterIdle();
                break;

            case MovementState.WALKING:
                OnStateEnterWalking();
                break;

            case MovementState.SPRINTING:
                OnStateEnterSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateUpdate(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateUpdateIdle();
                break;

            case MovementState.WALKING:
                OnStateUpdateWalking();
                break;

            case MovementState.SPRINTING:
                OnStateUpdateSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateExit(MovementState state)
    {
        switch (state)
        {
            case MovementState.IDLE:
                OnStateExitIdle();
                break;

            case MovementState.WALKING:
                OnStateExitWalking();
                break;

            case MovementState.SPRINTING:
                OnStateExitSprinting();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void TransitionToState(MovementState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void OnStateEnterIdle()
    {
        _animator.SetTrigger("Idle");
    }

    private void OnStateUpdateIdle()
    {
        if (_playerInput.HasMovement && !_playerInput.Sprint && !_collision.value) 
        {
            TransitionToState(MovementState.WALKING);
        }
        else if (_playerInput.HasMovement && _playerInput.Sprint && !_collision.value) 
        {
            TransitionToState(MovementState.SPRINTING);
        }
    }

    private void OnStateExitIdle()
    {

    }

    private void OnStateEnterWalking()
    {
        _animator.SetTrigger("Walk");
        _playerController.CurrentSpeed = _playerController.WalkSpeed;
    }

    private void OnStateUpdateWalking()
    {
        if (!_playerInput.HasMovement || _collision.value)
        {
            TransitionToState(MovementState.IDLE);
        }
        else if (_playerInput.Sprint) 
        {
            TransitionToState(MovementState.SPRINTING);
        }
    }

    private void OnStateExitWalking()
    {

    }

    private void OnStateEnterSprinting()
    {
        _animator.SetTrigger("Sprint");
        _playerController.CurrentSpeed = _playerController.SprintSpeed;
    }

    private void OnStateUpdateSprinting()
    {
        if (!_playerInput.HasMovement || _collision.value)
        {
            TransitionToState(MovementState.IDLE);
        }
        else if (!_playerInput.Sprint)
        {
            TransitionToState(MovementState.WALKING);
        }
    }

    private void OnStateExitSprinting()
    {
        
    }
}
