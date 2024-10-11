using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]public Animator circleOpen;
    // Start is called before the first frame update
    void Start()
    {
        //circleOpen.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenTackMainLevel(int Level)
    {
        /*
        if(circleOpen.GetBool("Open"))
        {
        }
        */
        //circleOpen.SetBool("Open", true);
        SceneManager.LoadScene(Level);
    }
    public void OpenTrack()
    {
        /*
        if(circleOpen.GetBool("Open"))
        {
        }
        */
        //circleOpen.SetBool("Open", true);
        //circleOpen.SetBool("Close", true);
        SceneManager.LoadScene(1);
    }
    public void OpenNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level+1);
    }

    public void Quitting()
    {
        Application.Quit();
    }
}
