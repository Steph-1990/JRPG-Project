using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] HeroData _heroData;

    void Start()
    {
        _heroData.currentHP = _heroData.maxHP;
        _heroData.currentMP = _heroData.maxMP;
    }

    public void LoseHP()
    {
        _heroData.currentHP -= 10;
    }
}
