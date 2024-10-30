using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _detectionLimitDistance; // Seuil à partir duquel l'ennemi détecte le joueur
    [SerializeField] private float _idleStateDuration; // Durée pendant laquelle l'ennemi reste en état idle avant de reprendre son chemin
    [SerializeField] private float _roamSpeed; // Vitesse de l'ennemi lorsque ce dernier se déplace entre les way points
    [SerializeField] private float _chaseSpeed; // Vitesse de l'ennemi lorsque ce dernier pourchasse le player
    [SerializeField] private BoolVariable _collision;

    private AIDestinationSetter _aiDestinationSetter;
    private AIPath _aiPath;
    private Transform _enemyTransform;
    private PlayerController _playerController;
    private EnemyStateMachine _enemyStateMachine;
    private Animator _animator;
    private Vector2 _lastDirection;
    private float _enterIdleTime; // Temps lors de l'entrée en état IDLE
    private float _currentspeed; // Vitesse actuel de l'ennemi
    private int _index;
    private bool _wayPointReached; // Passe à true si un way point est atteint par l'ennemi

    public float EnterIdleTime { get => _enterIdleTime; set => _enterIdleTime = value; }
    public bool WayPointReached { get => _wayPointReached; set => _wayPointReached = value; }


    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        _enemyStateMachine = GetComponent<EnemyStateMachine>();
        _animator = GetComponent<Animator>();
        _aiPath = GetComponent<AIPath>();
        _enemyTransform = transform;
    }

    private void Update()
    {
        NextWayPointCalculation();
        SetEnemyDestination();
        SetEnemySpeed();
        SetAnimatorParameters();
        SaveLastDirection();
    }

    public bool PlayerWithinAttackRange() // Renvoie true si le player est à portée d'attaque de l'ennemi
    {
        return Vector3.Distance(_enemyTransform.position, _playerController.gameObject.transform.position) <= _detectionLimitDistance;
    }

    public bool IdleTimerEnded() // Renvoie true si le timer du mode idle est terminée
    {
        return (Time.time >= _enterIdleTime + _idleStateDuration);
    }

    private void NextWayPointCalculation() // Détermine le prochain WayPoint vers lequel doit se diriger l'ennemi 
    {
        float distanceNextWayPoint = Vector3.Distance(_enemyTransform.position, _wayPoints[_index].position);

        if (distanceNextWayPoint <= 1 && _index == _wayPoints.Length - 1)
        {
            _wayPointReached = true;
            _index = 0;
        }
        else if (distanceNextWayPoint <= 1)
        {
            _wayPointReached = true;
            _index++;
        }
    }

    private void SetEnemySpeed() // Ajuste la vitesse de l'ennemi en fonction de l'état de la state machine
    {
        if (_enemyStateMachine.CurrentState == EnemyState.CHASING)
        {
            _aiPath.maxSpeed = _chaseSpeed;
        }
        else if (_enemyStateMachine.CurrentState == EnemyState.ROAMING)
        {
            _aiPath.maxSpeed = _roamSpeed;
        }
        else if (_enemyStateMachine.CurrentState == EnemyState.IDLE)
        {
            _aiPath.maxSpeed = 0;
        }
    }

    private void SetEnemyDestination() // Définie la direction dans laquelle doit se diriger l'ennemi en fonction de l'état de la state machine
    {
        if (_enemyStateMachine.CurrentState == EnemyState.CHASING)
        {
            _aiDestinationSetter.target = _playerController.gameObject.transform;
        }
        else if (_enemyStateMachine.CurrentState == EnemyState.ROAMING)
        {
            _aiDestinationSetter.target = _wayPoints[_index];
        }
    }

    private void SetAnimatorParameters() // Ajuste les paramètres de l'animator en fonction des déplacements du personnage
    {
        if (_collision.value) // Lors de la collision, l'ennemi continue de regarder dans la dernière direction
        {
            _animator.SetFloat("VectorX", _lastDirection.x); 
            _animator.SetFloat("VectorY", _lastDirection.y);
        }
        else
        {
            _animator.SetFloat("VectorX", _aiPath.desiredVelocity.x); // Lorsqu'il s'arrête à un way point, il se tourne vers l'écran
            _animator.SetFloat("VectorY", _aiPath.desiredVelocity.y);
        }       
    }

    private void SaveLastDirection() // On sauvegarde la dernière direction du joueur
    {
        if (_aiPath.desiredVelocity != Vector3.zero)
        {
            _lastDirection = _aiPath.desiredVelocity;
        }
    }
}
