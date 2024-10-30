using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpFightScene : MonoBehaviour
{
    [SerializeField] private GameObject _skeletonPrefab;
    [SerializeField] private GameObject _maidZombiePrefab;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private Transform[] _enemyPopPoints; // Points de pop des ennemis qui varie en fonction de leur nombre
    [SerializeField] private EnemyData _collidedEnemy; // Référence au type de l'ennemi entré en collision avec le joueur
    [SerializeField] private int _desiredEnemyNumber;

    private GameObject _enemy; // Variable qui contient les ennemies instanciés
    private UpdateUI _updateUI;
    private EnemyTransform _enemyTransform;
    private int _enemyNumber; // Nombre d'ennemi qui pop au début du fight
 
    public int EnemyNumber { get => _enemyNumber; set => _enemyNumber = value; }
    public EnemyData CollidedEnemy { get => _collidedEnemy; set => _collidedEnemy = value; }

    private void Awake()
    {
        _updateUI = FindObjectOfType<UpdateUI>();
        _enemyTransform = FindObjectOfType<EnemyTransform>();

        GenerateRandomNumber();
        InstantiateEnemy();
    }

    private void Update()
    {
        _updateUI.SetUIInformation(); // On met à jour les informations de l'UI au début du combat (nombre et type d'ennemies, PV des héros, etc...)
    }
    
    private void GenerateRandomNumber() // Génère un nombre entier aléatoire entre 1 et 3 (nombre d'ennemi à instancier) 
    {
        _enemyNumber = Random.Range(1, 4);
    }

    private Transform EnemyTransform(int i) // Renvoie le transform permettant de placer l'ennemi instancié sur l'écran
    {
        return _enemyPopPoints[_enemyNumber - 1].GetChild(i); // 3 ==> _enemyNumber
    }

    private void InstantiateEnemy() // On instancie le bon ennemi en fonction du type qui a été enregistré lors de la collision
    {
        for (int i = 0; i < _enemyNumber; i++) // 3 ==> _enemyNumber
        {
            if (_collidedEnemy.type == EnemyType.SKELETON)
            {
                _enemy =  Instantiate(_skeletonPrefab, EnemyTransform(i));
            }
            else if (_collidedEnemy.type == EnemyType.MAIDZOMBIE)
            {
                _enemy = Instantiate(_maidZombiePrefab, EnemyTransform(i));
            }
            else if (_collidedEnemy.type == EnemyType.GHOST)
            {
                _enemy = Instantiate(_ghostPrefab, EnemyTransform(i));
            }

            _enemy.transform.SetParent(_enemyTransform.transform); // On replace les ennemies au bon endroit dans la hiérarchie
        }
    } 
}
