using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtons : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayClickSound(AudioClip clip)
    {
        buttonSound.PlayOneShot(clip);
    }
}
