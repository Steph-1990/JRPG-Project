using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Axe deplacement
    private Vector2 _movementDirection;
    private Vector2 _normalizedMovement;
    // Bouton sprint
    private bool _sprint;

    public Vector2 MovementDirection { get => _movementDirection; }
    public Vector2 NormalizedMovement { get => _normalizedMovement; }
    public bool HasMovement { get => _movementDirection.sqrMagnitude > 0f; }
    public bool Sprint { get => _sprint; }

    private void Update()
    {
        // On stocke les valeurs brute et normalisée de l'axe de déplacement
        _movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _normalizedMovement = _movementDirection.normalized;

        // On stocke la valeur de l'input Sprint
        _sprint = Input.GetButton("Sprint");      
    }
}