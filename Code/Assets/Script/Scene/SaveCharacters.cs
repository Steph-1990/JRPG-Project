using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCharacters : MonoBehaviour
{
    [SerializeField] BoolVariable _collision;

    private void Update()
    {
        if (_collision.value)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
