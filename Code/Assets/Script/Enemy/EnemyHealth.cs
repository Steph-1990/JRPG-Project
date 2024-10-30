using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    [SerializeField] private int _currentHP;

    private BattleRoutine _battleRoutine;
    private int _indexEnemy;

    void Start()
    {
        _currentHP = _enemyData.maxHP;
        _battleRoutine = FindObjectOfType<BattleRoutine>();
        Debug.Log(_enemyData.type);

        for (int i = 0; i < _battleRoutine._charactersList.Count; i++)
        {
            if (gameObject == _battleRoutine._charactersList[i].gameObject)
            {
                //_indexEnemy =  _battleRoutine._charactersList.IndexOf(_battleRoutine._charactersList[i]) ;
                _indexEnemy = i;
            }
        }
    }

    private void Update()
    {
        if (_currentHP <= 0)
        {
            _battleRoutine.EnemyIsDead = true;

            if (_battleRoutine.SortedCharactersIndex < _indexEnemy)
            {
                _battleRoutine.Defend.RemoveAt(_battleRoutine.SortedCharactersIndex);
            }

            Destroy(gameObject);
        }
    }

    public void LoseHp()
    {
        _currentHP -= 10;
    }
}
