using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneTransitions : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private BoolVariable _transitionIsOver;
    [SerializeField] private BoolVariable _fightTookPlace;
    [SerializeField] private BoolVariable _collision;
    [SerializeField] private FloatVariable _transitionDelay;

    private SaveCharacters _updatedCharacters; 
    private SaveCharacters _initialCharacters; 
    private float _transitionTime;

    private void Awake()
    {
        _updatedCharacters = FindObjectOfType<SaveCharacters>(true);
        _initialCharacters = FindObjectOfType<SaveCharacters>();
    }

    private void Start()
    {
        PlaceUpdatedEnnemiesOnScene();
    }

    private void Update()
    {
        LaunchTransitionTimer();
        PlayAnimationTransition();
    }

    private void PlaceUpdatedEnnemiesOnScene() // Replace les ennemies sauvegardés sur la scène et les réactive
    {
        if (_fightTookPlace.value)
        {
            Destroy(_initialCharacters.gameObject);
            SceneManager.MoveGameObjectToScene(_updatedCharacters.gameObject, SceneManager.GetSceneByName("SampleScene"));
            _updatedCharacters.gameObject.SetActive(true);
            _fightTookPlace.value = false;
        }
    }

    private void LaunchTransitionTimer() // Lance un timer si collision avec le player
    {
        if (_collision.value || _transitionIsOver.value)
        {
            _transitionTime += Time.deltaTime;
        }
        else
        {
            _transitionTime = 0;
        }
    }

    private void PlayAnimationTransition() // Joue les transitions entre les scènes 
    {
        if (_transitionIsOver.value)
        {
            _canvas.alpha = (1 - _transitionTime * (1 / _transitionDelay.value));

            if (_transitionTime >= _transitionDelay.value)
            {
                _transitionIsOver.value = false;
            }
        }
        else if (_collision.value)
        {
            _canvas.alpha = _transitionTime * (1 / _transitionDelay.value);

            if (_transitionTime >= _transitionDelay.value)
            {     
                SceneManager.LoadScene("FightScene"); 
                _updatedCharacters.gameObject.SetActive(false);
                _collision.value = false; // On repasse la variable à false pour les futurs collisions
                _transitionIsOver.value = true;              
            }
        }               
    }
}
