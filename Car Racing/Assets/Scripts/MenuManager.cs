using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]public Animator circleOpen;


    public GameObject LoadingPanel;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI progressText;

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
        //SceneManager.LoadScene(level+1);
        StartCoroutine(LoadAsynchronousScene(level+1));
    }

    public void Quitting()
    {
        Application.Quit();
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronousScene(sceneIndex));
    }

    IEnumerator LoadAsynchronousScene(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingPanel.gameObject.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
