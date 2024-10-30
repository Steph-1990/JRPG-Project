using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //private int _maVariable = 1;
    [SerializeField] Transform _hero;
    [SerializeField] RectTransform _gameObject;
    [SerializeField] Camera cam;


    private void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(_hero.position);
        _gameObject.position = (Vector2) screenPos + new Vector2(0,80);
        Debug.Log(screenPos);
        Debug.Log(_gameObject.position);

    }

}
