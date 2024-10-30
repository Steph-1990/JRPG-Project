using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    [SerializeField] CharactersData _charactersData;
    public CharactersData CharactersData { get => _charactersData; set => _charactersData = value; }
}
