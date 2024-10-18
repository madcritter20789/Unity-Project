using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject LoadingPanel;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI progressText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
