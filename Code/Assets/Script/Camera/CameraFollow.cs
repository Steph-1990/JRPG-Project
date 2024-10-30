using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PlayerController _playerController;
    private Transform _transform; // Transform de la camera

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>(); // Au start pour laisser le temps au MoveGameObjectToScene de replacer les personnages sur la scène
    }

    private void Update()
    {
        SnapMode();
    }

    private void SnapMode() // La caméra suit le joueur
    {
        Vector3 newPosition = _playerController.transform.position;
        newPosition.z = _transform.position.z;
        _transform.position = newPosition;
    }
}
