using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleRoutine : MonoBehaviour
{
    [SerializeField] private GameObject _actionMenu; // Le menu de selection d'action
    [SerializeField] private Transform _playerCursor; // Curseur permettant de savoir quel héros est selectionné lors du choix d'action dans le menu
    [SerializeField] private Transform _enemyCursor; // Le cursor présent sur la tête d'un ennemi lors du choix de l'ennemi à attaquer
    [SerializeField] private RectTransform _damageText;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioSource _cursorSFX;
    [SerializeField] private float _charactersAnimationSpeed; // Vitesse d'animation des personnages

    [SerializeField] public List<Characters> _charactersList = new List<Characters>(); // Liste de tous les personnages présents sur la scène
    [SerializeField] private List<PlayerHealth> _heroesList = new List<PlayerHealth>(); // Liste de tous les héros présents sur la scène
    [SerializeField] private List<EnemyHealth> _enemiesList = new List<EnemyHealth>(); // Liste de tous les ennemis présents sur la scène
    [SerializeField] private List<GameObject> _applyDamageToCharacter = new List<GameObject>(); // Liste de l'ordre dans lequel les personnages doivent subir des dégâts
    [SerializeField] private List<int> _heroesAttackOrder = new List<int>(); // Ordre dans lequel attaque les héros
    [SerializeField] private List<bool> _defend = new List<bool>(); // Liste de booléen qui permet de savoir si le personnage est en train de défendre

    private int _sortedCharactersIndex; // Index de la liste des personnages triés en fonction de leur agilité
    private int _movementIndex; // Index qui précise quel vecteur de mouvement doit être utilisé en fonction de chaque animation
    private int _enemyIndex; // Index de la liste d'ennemis
    private int _heroToAttackIndex; // Nombre aléatoire compris entre 0 et 2 pour connaître l'ennemi à attaquer 
    private float _animationTimer; //Timer qui se lance à chaque début d'animation
    private float _safetyMargin = 0.01f; // Marge de sécurité pour empêcher l'animation d'un personnage de se lancer une seconde fois
    private bool _joystickIsPressed; // Passe à true si le joystick est poussé vers l'avant ou vers l'arrière
    private bool _attackButtonReleased; // Index pour gérer l'appui des boutons de la manette lors de la navigation menu
    private bool _attackButtonPressed; // Passe à true lors de l'appui sur le bouton "Attaquer"
    private bool _enemyIsDead;
    private bool _playerHasMadeChoice; // Passe à true si le joueur a fini de choisir les actions à effectuer pour ses personnages  
    private bool _defendButtonPressed; // Passe à true si le joueur choisi "Défendre"
    private Vector3[] _enemiesMovement; // Vecteurs de mouvement des ennemies
    private Vector3[] _heroesMovement;  // Vecteurs de mouvement des héros
    private Vector3[] _charactersMovement; // Vecteurs de mouvement des personnages qui varie suivant qu'il soit un héros ou un ennemi
    private Vector2 _screenPos; // Position de l'affichage des dégâts
    private Animator _characterAnimator; // Stocke temporairement l'animator d'un personnage lorsque ce dernier est actif
    private Transform _characterTransform; // Stocke temporairement le transform d'un personnage lorsque ce dernier est actif
    private PlayerHealth _playerHealth;
    private EnemyHealth _enemyHealth;
    private EndFight _endFight;
    private BattleAudioManager _battleAudioManager;

    bool _cancelDefense;
    bool test;
    [SerializeField] private List<int> _recupIndex = new List<int>(); // Liste de booléen qui permet de savoir si le personnage est en train de défendre
    [SerializeField] private List<Animator> _characterAnimator4 = new List<Animator>();


    public bool EnemyIsDead { get => _enemyIsDead; set => _enemyIsDead = value; }
    public bool AttackButtonPressed { get => _attackButtonPressed; set => _attackButtonPressed = value; }
    public bool DefendButtonPressed { get => _defendButtonPressed; set => _defendButtonPressed = value; }
    public int SortedCharactersIndex { get => _sortedCharactersIndex; set => _sortedCharactersIndex = value; }
    public List<bool> Defend { get => _defend; set => _defend = value; }

    private void Awake()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _battleAudioManager = FindObjectOfType<BattleAudioManager>();
        _endFight = GetComponent<EndFight>();
    }

    private void Start()
    {
        _charactersList = new List<Characters>(FindObjectsOfType<Characters>()); // Au Start pour laisser le temps aux ennemies d'apparaître
        _enemiesList = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
        _enemiesMovement = new Vector3[] { new Vector3(4 * _charactersAnimationSpeed, 0, 0), new Vector3(0, 0, 0), new Vector3(-4 * _charactersAnimationSpeed, 0, 0) };
        _heroesMovement = new Vector3[] { new Vector3(-4 * _charactersAnimationSpeed, 0, 0), new Vector3(0, 0, 0), new Vector3(4 * _charactersAnimationSpeed, 0, 0) };
        SortCharacters(); // On trie les personnages en fonction de leur agilité
    }

    private void Update()
    {
        if (_enemyIsDead)
        {
            UpdateCharactersNumber(); // On met à jour les personnages présents sur la scène
            _enemyIsDead = false; // On repasse la valeur à false pour check si d'autres ennemis meurt
        }
        else if (_enemiesList.Count == 0 && _movementIndex == 0) // Si il n'y a pas d'ennemi sur la scène et que tous les héros ont finis leur action, on repasse sur la scène exploration
        {
            _endFight.ReturnExplorationScene();
        }
        else
        {
            RunBattleRoutine(); // Sinon on exécute la routine de combat
        }
    }



    private void UpdateCharactersNumber() // Mise à jour de la liste des personnages présent sur la scène lors de la mort d'un personnage
    {
        _charactersList = new List<Characters>(FindObjectsOfType<Characters>());
        SortCharacters();
        _enemiesList.Clear();
        _enemiesList = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());

        if (_sortedCharactersIndex != 0)
        {
            _sortedCharactersIndex--;
        }
    }

    private void RunBattleRoutine() // Lance la routine de combat
    {
        if (_sortedCharactersIndex == _charactersList.Count) // Lorsque l'on a parcouru toute la liste de personnages on passe du mode selection menu au mode animation ou inversement
        {

            _playerHasMadeChoice = !_playerHasMadeChoice;
            _sortedCharactersIndex = 0;

            if (_defend != null && !_playerHasMadeChoice)
            {
                _defend.Clear();
                test = true;               
            }
            
        }


        if (_playerHasMadeChoice) // Si le player a fait son choix 
        {
            PlayAnimations(); // On lance les animations
        }
        else
        {
            if (test)
            {
                
                Test();
            }
            else
            {
                if (_sortedCharactersIndex == 0)
                {
                    _characterAnimator4.Clear();
                }
                SelectMenu(); // Sinon on laisse le joueur faire son choix dans le menu       
            }
        }
    }

    private void RetrieveCharacterComponents() // Récupère les composants animator et transform des personnages
    {
        if (_characterAnimator == null)
        {
            _animationTimer = 0;
            _characterAnimator = _charactersList[_sortedCharactersIndex].gameObject.GetComponent<Animator>();
        }

        if (_characterTransform == null)
        {
            _characterTransform = _charactersList[_sortedCharactersIndex].gameObject.transform;
        }
    }

    private void RetrieveAppropriateVectors() // Récupère les vecteurs de mouvements adéquats suivant que le personnages soit un ennemi ou un héros
    {
        _charactersMovement = _charactersList[_sortedCharactersIndex].CharactersData is HeroData ? _heroesMovement : _enemiesMovement;
    }

    private void Test()
    {
        RetrieveCharacterComponents(); // Faire en sorte qu'il ne s'active pas lors de la fin du tour
        _characterAnimator.speed = _charactersAnimationSpeed; // On applique la vitesse de l'animator en fonction de celle choisit par l'utilisateur
        _animationTimer += Time.deltaTime;

        if (_characterAnimator4 != null)
        {
            foreach (var item in _characterAnimator4)
            {
                if (_animationTimer < (item.GetCurrentAnimatorStateInfo(0).length - _safetyMargin))
                {
                    item.SetBool("CancelDefense", true); // on lance l'animation
                    item.gameObject.transform.Translate(new Vector3(-4 * _charactersAnimationSpeed, 0, 0) * Time.deltaTime);
                }
                else
                {
                    test = false;
                    item.SetBool("CancelDefense", false); // on lance l'animation
                    _recupIndex.Clear();

                }
            }
        }
    }

    private void SetCharacterAnimation(string animatorParameter)
    {

        RetrieveCharacterComponents(); // On récupère les composants nécessaires

        _characterAnimator.speed = _charactersAnimationSpeed; // On applique la vitesse de l'animator en fonction de celle choisit par l'utilisateur
        _animationTimer += Time.deltaTime;

        if (_animationTimer < (_characterAnimator.GetCurrentAnimatorStateInfo(0).length - _safetyMargin))
        {

            _characterAnimator.SetBool(animatorParameter, true); // on lance l'animation
            MoveCharacters(animatorParameter); // On déplace le personnage tant que le timer ne dépasse pas la durée de l'animation
        }
        else
        {
            _animationTimer = 0; // On reset le timer
            _movementIndex++; // On passe au mouvement suivant

            if (_movementIndex == 1 && _playerHasMadeChoice)
            {
                ApplyAttackDamage();
            }
            else if (!_playerHasMadeChoice && !_cancelDefense)
            {
                _defend.Add(true); // Indique que le personnage est en défense en passant la valeur à true
                _characterAnimator4.Add(_characterAnimator);
                _recupIndex.Add(_sortedCharactersIndex);
                _heroesAttackOrder.Add(_sortedCharactersIndex); // Récupère l'index du héros actuel dans la liste de personnages et le stock dans une liste
            }
        }
    }

    private void MoveCharacters(string _animatorName) // Déplace le personnage lors de l'animation d'attaque
    {
        if (_movementIndex < _charactersMovement.Length)
        {
            _characterTransform.Translate(_charactersMovement[_movementIndex] * Time.deltaTime);
        }
        else
        {
            _characterAnimator.SetBool(_animatorName, false);
            _characterAnimator = null;
            _characterTransform = null;
            _movementIndex = 0;
            _defendButtonPressed = false; // Passe à true si le joueur sélectionne "défendre", on le repasse donc à false directement pour check le prochain appui

            if (_cancelDefense)
            {
                _cancelDefense = false;
                _defend.RemoveAt(_sortedCharactersIndex);
            }
            else
            {
                _sortedCharactersIndex++;
            }
        }
    }

    private void PlayAnimations() // Joue les animations des personnages
    {
        RetrieveAppropriateVectors(); // On récupère les vecteurs de mouvements adéquats

        if (_defend[_sortedCharactersIndex])
        {
            _characterAnimator = null;
            _characterTransform = null;
            _sortedCharactersIndex++;
            _movementIndex = 0;
            _heroesAttackOrder.RemoveAt(0);
        }
        else
        {
            SetCharacterAnimation("AnimationIsTriggered");
        }
    }

    private void ApplyAttackDamage() // Applique les dommages 
    {
        _damageText.gameObject.SetActive(false);

        if (_charactersList[_sortedCharactersIndex].CharactersData is HeroData)
        {
            if (_applyDamageToCharacter[0].gameObject != null)
            {
                _applyDamageToCharacter[0].GetComponent<EnemyHealth>().LoseHp();
                DisplayDamage(_applyDamageToCharacter[0].gameObject);
            }
            else
            {
                _enemiesList[0].LoseHp();
                DisplayDamage(_enemiesList[0].gameObject);
            }
            _applyDamageToCharacter.RemoveAt(0); // Une fois les dégâts appliqués sur le personnage, on supprime ces degâts pour ne pas les appliquer une seconde fois
            _heroesAttackOrder.RemoveAt(0);
        }
        else
        {
            _heroToAttackIndex = Random.Range(0, 3);
            _heroesList[_heroToAttackIndex].LoseHP();
            DisplayDamage(_heroesList[_heroToAttackIndex].gameObject);
        }
    }

    private void DisplayDamage(GameObject character) // Affiche les dégâts subit sur les personnages
    {
        _screenPos = _camera.WorldToScreenPoint(character.transform.position);
        _damageText.position = _screenPos + new Vector2(0, 80);
        _damageText.gameObject.SetActive(true);
    }

    private void MenuAndCursorDisplayManager(Transform cursor, Vector2 characterPosition, bool displayMenu, bool displayCursor) // Gère l'affichage des curseurs et du menu 
    {
        _actionMenu.SetActive(displayMenu); // On active ou non le menu
        cursor.gameObject.SetActive(displayCursor); // On active le curseur de selection
        cursor.position = new Vector2(characterPosition.x, characterPosition.y + 3.5f); // On place le curseur au dessus de la tête du personnage actif
    }

    private void SelectEnemyToAttack() // Choix de la selection de l'ennemi
    {
        Vector2 _enemyPosition = _enemiesList[_enemyIndex].transform.position;
        MenuAndCursorDisplayManager(_enemyCursor, _enemyPosition, false, true);

        if (Input.GetButtonUp("Submit"))
        {
            _attackButtonReleased = true;
            _battleAudioManager.PlayConfirmButtonSound();
        }

        if (Input.GetButtonDown("Submit") && _attackButtonReleased)
        {
            _heroesAttackOrder.Add(_sortedCharactersIndex);
            _applyDamageToCharacter.Add(_enemiesList[_enemyIndex].gameObject);
            _enemyIndex = 0; // Replace le curseur sur le premier ennemi à chaque début de tour
            _enemyCursor.gameObject.SetActive(false); // On désactive le curseur de selection de l'ennemi
            _playerCursor.gameObject.SetActive(false); // On désactive le curseur de selection du personnage en cours
            _sortedCharactersIndex++; // On passe au personnage suivant
            _attackButtonPressed = false; // On repasse la variable à false pour permettre l'appui sur le bouton
            _attackButtonReleased = false;
            _defend.Add(false);
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            _joystickIsPressed = false;
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && _enemyIndex < _enemiesList.Count - 1 && _joystickIsPressed == false)
        {
            _battleAudioManager.PlayCursorSound();
            _joystickIsPressed = true;
            _enemyIndex++;
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && _enemyIndex > 0 && _joystickIsPressed == false)
        {
            _battleAudioManager.PlayCursorSound();
            _joystickIsPressed = true;
            _enemyIndex--;
        }
    }

    private void SelectMenu() // Autorise le joueur à faire ses choix d'actions dans le menu pour chaque personnage
    {
        if (_charactersList[_sortedCharactersIndex].CharactersData is EnemyData)
        {
            _sortedCharactersIndex++; // Si le personnage actuel est un ennemi, on passe au personnage suivant
            _defend.Add(false); // L'ennemi ne defend pas donc on passe la valeur à false pour chaque ennemi
        }
        else
        {
            Vector2 _characterPosition = _charactersList[_sortedCharactersIndex].gameObject.transform.position;
            MenuAndCursorDisplayManager(_playerCursor, _characterPosition, true, true); // Affiche le menu et le curseur au dessus de la tête du personnage actif

            if (_attackButtonPressed) // Le joueur a selectionné "Attaque"
            {
                SelectEnemyToAttack(); // Permet au joueur de sélectionner l'ennemi à attaquer
            }
            else if (_cancelDefense)
            {
                MenuAndCursorDisplayManager(_playerCursor, _characterPosition, false, false);
                _charactersMovement = new Vector3[] { new Vector3(-4 * _charactersAnimationSpeed, 0, 0) };
                SetCharacterAnimation("CancelDefense");
            }
            else if (_defendButtonPressed) // Le joueur a selectionné "Défendre"
            {
                MenuAndCursorDisplayManager(_playerCursor, _characterPosition, false, false);
                _battleAudioManager.PlayConfirmButtonSound();
                _charactersMovement = new Vector3[] { new Vector3(4 * _charactersAnimationSpeed, 0, 0) };
                SetCharacterAnimation("isDefend");
            }


            if (Input.GetButtonDown("Cancel") && !_defendButtonPressed && !_cancelDefense) // "&& !_defendButtonPressed && !_cancelDefense" pour empêcher l'appui sur la touche annuler pendant l'animation du personnage
            {
                CancelButtonPressed(); // Annule la dernière action
            }
        }
    }

    private void CancelButtonPressed()
    {
        _battleAudioManager.PlayCancelSound();

        if (_attackButtonPressed) // Si le bouton d'attaque a déjà été enfoncé
        {
            _attackButtonReleased = false;
            _attackButtonPressed = false;
            _enemyCursor.gameObject.SetActive(false); // On désactive le curseur de selection de l'ennemi
        }
        else if (_defend[_defend.Count - 1])
        {
            _sortedCharactersIndex = _heroesAttackOrder[_heroesAttackOrder.Count - 1]; // Alors on revient au héros précédent
            _heroesAttackOrder.RemoveAt(_heroesAttackOrder.Count - 1);
            _cancelDefense = true;
        }
        else if (_charactersList[_sortedCharactersIndex].gameObject != _heroesList[0].gameObject) // Si le personnage actif n'est pas le héros avec l'agilité la plus forte
        {
            _sortedCharactersIndex = _heroesAttackOrder[_heroesAttackOrder.Count - 1]; // Alors on revient au héros précédent
            _heroesAttackOrder.RemoveAt(_heroesAttackOrder.Count - 1);

            if (!_defend[_sortedCharactersIndex])
            {
                _applyDamageToCharacter.RemoveAt(_applyDamageToCharacter.Count - 1);
            }

            for (int i = 0; i < _defend.Count; i++)
            {
                if (i >= _sortedCharactersIndex)
                {
                    _defend.RemoveRange(i, _defend.Count - i);
                }
            }
        }
    }

    private void SortCharacters() // On trie les personnages en fonction de leur agilité (de la plus forte à la plus faible)
    {
        _charactersList.Sort((charA, charB) => charA.CharactersData.agility > charB.CharactersData.agility ? -1 : 1);

        foreach (var item in _charactersList)
        {
            if (item.CharactersData is HeroData)
            {
                _heroesList.Add(item.GetComponent<PlayerHealth>());
            }
        }

    }
}

