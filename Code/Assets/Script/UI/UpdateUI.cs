using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private HeroData _ryu;
    [SerializeField] private HeroData _nina;
    [SerializeField] private HeroData _rei;

    [SerializeField] private TextMeshProUGUI _enemyName;
    [SerializeField] private TextMeshProUGUI _enemyNb;
    [SerializeField] private TextMeshProUGUI _ryuHP;
    [SerializeField] private TextMeshProUGUI _ryuMP;
    [SerializeField] private TextMeshProUGUI _ninaHP;
    [SerializeField] private TextMeshProUGUI _ninaMP;
    [SerializeField] private TextMeshProUGUI _reiHP;
    [SerializeField] private TextMeshProUGUI _reiMP;
    [SerializeField] private RectTransform _pointerMenu;
    [SerializeField] private Button _attackButton;

    private SetUpFightScene _initialSettings;
    private BattleAudioManager _battleAudioManager;
    private Vector2 _lastAnchoredPosition;

    private int _ryuCurrentHP;
    private int _ryuCurrentMP;
    private int _ninaCurrentHP;
    private int _ninaCurrentMP;
    private int _reiCurrentHP;
    private int _reiCurrentMP;


    public Button AttackButton { get => _attackButton; set => _attackButton = value; }

    private void Awake()
    {
        _initialSettings = FindObjectOfType<SetUpFightScene>();
        _battleAudioManager = FindObjectOfType<BattleAudioManager>();
    }

    private void Update()
    {
        Vector2 anchoredPosition = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;

        if (_lastAnchoredPosition != anchoredPosition && (Input.GetAxisRaw("Vertical") != 0))
        {
            _lastAnchoredPosition = anchoredPosition;
            _battleAudioManager.PlayCursorSound();
        }
        _pointerMenu.anchoredPosition = new Vector2(_pointerMenu.anchoredPosition.x, anchoredPosition.y);
    }

    public void SetUIInformation() // Affiche divers informations sur l'UI au lancement du combat (HP des héros, nombre d'ennemies, etc...)
    {
        _ryuCurrentHP = _ryu.currentHP;
        _ryuCurrentMP = _ryu.currentMP;
        _ninaCurrentHP = _nina.currentHP;
        _ninaCurrentMP = _nina.currentMP;
        _reiCurrentHP = _rei.currentHP;
        _reiCurrentMP = _rei.currentMP;

        _enemyName.text = _initialSettings.CollidedEnemy.type.ToString();
        _enemyNb.text = _initialSettings.EnemyNumber.ToString();
        _ryuHP.text = _ryuCurrentHP.ToString() + "/" + _ryu.maxHP.ToString();
        _ryuMP.text = _ryuCurrentMP.ToString() + "/" + _ryu.maxMP.ToString();
        _ninaHP.text = _ninaCurrentHP.ToString() + "/" + _nina.maxHP.ToString();
        _ninaMP.text = _ninaCurrentMP.ToString() + "/" + _nina.maxMP.ToString();
        _reiHP.text = _reiCurrentHP.ToString() + "/" + _rei.maxHP.ToString();
        _reiMP.text = _reiCurrentMP.ToString() + "/" + _rei.maxMP.ToString();
    }
}
