using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject qui contiendra toutes les informations de chaque héros (Type, nombre d'HP/MP, stats,... )

[CreateAssetMenu(menuName = "Scriptables/HeroData")]
public class HeroData : CharactersData
{
    public HeroType type;
    public int currentHP;
    public int currentMP;
}
