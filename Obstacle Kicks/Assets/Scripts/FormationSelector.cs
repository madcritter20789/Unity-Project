using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FormationSelector : MonoBehaviour
{
    [SerializeField] GameObject[] formationsTeam1;
    [SerializeField] GameObject[] formationsTeam2;
    [SerializeField] TextMeshProUGUI formationName;
    //[SerializeField] Transform postionOfFormation;
    //private List<GameObject> formationsPreview;
    public int currentFormation=0;
    


    // Start is called before the first frame update
    void Start()
    {
        /*
        formationsPreview = new List<GameObject>();

        foreach(var format in formationsPreview)
        {
            GameObject fo = Instantiate(format.gameObject, postionOfFormation.position, Quaternion.identity);
            fo.SetActive(false);
            fo.transform.SetParent(postionOfFormation);
            formationsPreview.Add(fo);
        }
        */


        if (PlayerPrefs.GetString("team") == "Red")
        {
            formationsTeam2[currentFormation].SetActive(true);
            formationName.text = formationsTeam2[currentFormation].name;
        }
        else
        {
            formationsTeam1[currentFormation].SetActive(true);
            formationName.text = formationsTeam1[currentFormation].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void ShowFormation()
    {
        formationsPreview[currentFormation].SetActive(true);
    }

    public void OnClickNext()
    {
        if (currentFormation < formationsPreview.Count - 1) 
        { 
            currentFormation = currentFormation + 1;
        }
        else
        {
            currentFormation = 0;
        }
        ShowFormation();
    }

    public void OnClickPrev()
    {
        if (currentFormation == 0)
        {
            currentFormation = formationsPreview.Count - 1;
        }
        else
        {
            currentFormation = currentFormation - 1;
        }
        ShowFormation();
    }
    */

    public void NextFormation()
    {
        if(PlayerPrefs.GetString("team")=="Red")
        {
            formationsTeam2[currentFormation].SetActive(false);
            currentFormation = (currentFormation + 1) % formationsTeam2.Length;
            formationsTeam2[currentFormation].SetActive(true);
            formationName.text = formationsTeam2[currentFormation].name;
        }
        else
        {
            formationsTeam1[currentFormation].SetActive(false);
            currentFormation = (currentFormation + 1) % formationsTeam1.Length;
            formationsTeam1[currentFormation].SetActive(true);
            formationName.text = formationsTeam1[currentFormation].name;
        }

    }

    public void PreviousFormation()
    {
        if (PlayerPrefs.GetString("team") == "Red")
        {
            formationsTeam2[currentFormation].SetActive(false);
            currentFormation--;
            if (currentFormation < 0)
            {
                currentFormation += formationsTeam2.Length;
            }
            formationsTeam2[currentFormation].SetActive(true);
            formationName.text = formationsTeam2[currentFormation].name;
        }
        else
        {
            formationsTeam1[currentFormation].SetActive(false);
            currentFormation--;
            if (currentFormation < 0)
            {
                currentFormation += formationsTeam1.Length;
            }
            formationsTeam1[currentFormation].SetActive(true);
            formationName.text = formationsTeam1[currentFormation].name;
        }

    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedFormation", currentFormation);
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
