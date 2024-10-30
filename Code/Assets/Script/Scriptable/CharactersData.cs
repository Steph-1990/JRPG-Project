using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject qui contiendra toutes les informations de chaque ennemi (Type, nombre d'HP/MP, stats,... )

[CreateAssetMenu(menuName = "Scriptables/CharactersData")]
public class CharactersData : ScriptableObject
{    
    public int maxHP;
    public int maxMP;
    public int agility;
}
