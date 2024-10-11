using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableonce;

    void PlaySound(AudioClip whichSound)
    {
        if (!disableonce)
        {
            menuButtonController.audioSource.PlayOneShot(whichSound);
        }
        else
        {
            disableonce = false;
        }
    }
}
