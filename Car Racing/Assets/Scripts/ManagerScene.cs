using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerScene : MonoBehaviour
{
    [SerializeField] AudioSource clickSound;
    [SerializeField] AudioClip click;
    // Start is called before the first frame update
    void Start()
    {
        clickSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenTrack(int Level)
    {
        clickSound.PlayOneShot(click);
        StartCoroutine(OpenTrackLevel(Level));
    }
    IEnumerator OpenTrackLevel(int level)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(level);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
