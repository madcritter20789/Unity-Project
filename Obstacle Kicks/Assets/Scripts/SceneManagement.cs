using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
/*    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadTeamSelection()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadFormationSelection(string team)
    {
        PlayerPrefs.SetString("team", team);
        Debug.Log(PlayerPrefs.GetString("team", team));
        SceneManager.LoadScene(2);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(3);
    }

    public void Quitting()
    {
        Application.Quit();
    }
}
