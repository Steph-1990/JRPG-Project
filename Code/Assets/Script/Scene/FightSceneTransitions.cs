using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightSceneTransitions : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private BoolVariable _transitionIsOver;
    [SerializeField] private FloatVariable _transitionDelay;

    private EndFight _endFight;
    private float _transitionTime;

    private void Awake()
    {
        _endFight = FindObjectOfType<EndFight>();   
    }

    private void Update()
    {
        LaunchTransitionTimer();
        PlayAnimationTransition();
    }

    private void LaunchTransitionTimer() // Lance un timer si collision avec le player
    {        
        if (_endFight.FightIsEnded || _transitionIsOver.value)
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

        else if (_endFight.FightIsEnded)
        {
            _canvas.alpha = _transitionTime * (1 / _transitionDelay.value);

            if (_transitionTime >= _transitionDelay.value)
            {
                SceneManager.LoadScene("SampleScene");
                _endFight.FightIsEnded = false;
                _transitionIsOver.value = true;
            }
        }      
    }
}
