using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _caveTheme;
    [SerializeField] private AudioSource _enemyCollisionSound;

    public void PlayEnemyCollisionSFX()
    {
        _caveTheme.Stop();
        _enemyCollisionSound.Play();
    }
}
