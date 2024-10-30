using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState
{
    MENU,
    ANIMATION,
    VICTORY,
    DEFEAT,
}

public class BattleStateMachine : MonoBehaviour
{
    [SerializeField] private BattleState _currentState;
    private BattleRoutine _battle;

    private void Awake()
    {
        _battle = GetComponent<BattleRoutine>();
    }

    private void Start()
    {
        OnStateEnter(BattleState.MENU);
    }

    private void Update()
    {
        OnStateUpdate(_currentState);
    }

    private void OnStateEnter(BattleState state)
    {
        switch (state)
        {
            case BattleState.MENU:
                OnStateEnterMenu();
                break;

            case BattleState.ANIMATION:
                OnStateEnterAnimation();
                break;

            case BattleState.VICTORY:
                OnStateEnterVictory();
                break;

            case BattleState.DEFEAT:
                OnStateEnterDefeat();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateUpdate(BattleState state)
    {
        switch (state)
        {
            case BattleState.MENU:
                OnStateUpdateMenu();
                break;

            case BattleState.ANIMATION:
                OnStateUpdateAnimation();
                break;

            case BattleState.VICTORY:
                OnStateUpdateVictory();
                break;

            case BattleState.DEFEAT:
                OnStateUpdateDefeat();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void OnStateExit(BattleState state)
    {
        switch (state)
        {
            case BattleState.MENU:
                OnStateExitMenu();
                break;

            case BattleState.ANIMATION:
                OnStateExitAnimation();
                break;

            case BattleState.VICTORY:
                OnStateExitVictory();
                break;

            case BattleState.DEFEAT:
                OnStateExitDefeat();
                break;

            default:
                Debug.LogError($"OnStateEnter: Invalid state {state}");
                break;
        }
    }

    private void TransitionToState(BattleState toState)
    {
        OnStateExit(_currentState);
        _currentState = toState;
        OnStateEnter(toState);
    }

    private void OnStateEnterMenu()
    {
        
    }

    private void OnStateUpdateMenu()
    {
        
    }

    private void OnStateExitMenu()
    {

    }

    private void OnStateEnterAnimation()
    {
       
    }

    private void OnStateUpdateAnimation()
    {
        
    }

    private void OnStateExitAnimation()
    {
       
    }

    private void OnStateEnterVictory()
    {
        
    }

    private void OnStateUpdateVictory()
    {
       
    }

    private void OnStateExitVictory()
    {

    }

    private void OnStateEnterDefeat()
    {
        
    }

    private void OnStateUpdateDefeat()
    {
             
    }

    private void OnStateExitDefeat()
    {
        
    }
}
