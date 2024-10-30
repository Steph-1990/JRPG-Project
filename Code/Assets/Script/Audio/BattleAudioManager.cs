using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _confirmButton;
    [SerializeField] private AudioSource _cancelButton;
    [SerializeField] private AudioSource _cursor;

    public void PlayConfirmButtonSound()
    {
        _confirmButton.Play();
    }

    public void PlayCursorSound()
    {
        _cursor.Play();
    }

    public void PlayCancelSound()
    {
        _cancelButton.Play();
    }
}
