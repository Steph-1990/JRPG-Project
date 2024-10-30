using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFight : MonoBehaviour
{
    [SerializeField] BoolVariable _fightTookPlace; // Indique si un combat à eu lieu

    private bool _fightIsEnded; // Passe à true si le combat est terminé 

    public bool FightIsEnded { get => _fightIsEnded; set => _fightIsEnded = value; }
    public BoolVariable FightTookPlace { get => _fightTookPlace; set => _fightTookPlace = value; }

    public void ReturnExplorationScene()
    {
        _fightIsEnded = true;
        _fightTookPlace.value = true;
    }
}
