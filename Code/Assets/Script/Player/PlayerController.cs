using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 5f; // Vitesse de déplacement du personnage lorsqu'il marche
    [SerializeField] private float _sprintSpeed = 10f; // Vitesse de déplacement du personnage en sprint
    [SerializeField] private BoolVariable _collision; // Passe à true si collision avec l'ennemi

    private float _currentSpeed; // Vitesse de déplacement actuel du personnage
    private Vector2 _lastDirection; // La dernière direction dans laquelle le personnage regarde avant de s'arrêter
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerInput _playerInput;

    public float WalkSpeed { get => _walkSpeed; set => _walkSpeed = value; }
    public float SprintSpeed { get => _sprintSpeed; set => _sprintSpeed = value; }
    public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        SetAnimatorParameters();
        SaveLastDirection();
    }

    private void FixedUpdate()
    {
        ApplyInput();
    }

    private void ApplyInput() // On applique les entrées clavier/manettes au Rigidbody pour déplacer le personnage ou l'empêcher de bouger si il est entré en collision avec l'ennemi
    {
        if (_collision.value)
        {
            _playerInput.enabled = false;
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _playerInput.enabled = true;
            _rigidbody.velocity = _playerInput.NormalizedMovement * _currentSpeed;
        }
    }

    private void SetAnimatorParameters() // Ajuste les paramètres de l'animator en fonction des déplacements du personnage
    {
        _animator.SetFloat("VectorX", _lastDirection.x);
        _animator.SetFloat("VectorY", _lastDirection.y);
    }

    private void SaveLastDirection() // On sauvegarde la dernière direction du joueur
    {
        if (_playerInput.HasMovement) 
        {
            _lastDirection = _playerInput.NormalizedMovement; 
        }
    }
}
