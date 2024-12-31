using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadFormation : MonoBehaviour
{
    public GameObject[] formationPrefabsteam1;
    public GameObject[] formationPrefabsteam2;
    public Transform spawnPoint;
    //public TMP_Text label;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("team") == "Red")
        {
            int selectedFormat = PlayerPrefs.GetInt("selectedFormation");
            GameObject prefabteam2 = formationPrefabsteam2[selectedFormat];
            GameObject cloneteam2 = Instantiate(prefabteam2, spawnPoint.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            //label.text = prefab.name;
            cloneteam2.SetActive(true);

            int formationOppTeam = Random.Range(0, 11);
            GameObject prefabteam1 = formationPrefabsteam1[formationOppTeam];
            GameObject cloneteam1 = Instantiate(prefabteam1, spawnPoint.position, Quaternion.identity);
            //label.text = prefab.name;
            prefabteam1.SetActive(true);
        }
        else
        {
            int selectedFormat = PlayerPrefs.GetInt("selectedFormation");
            GameObject prefabteam1 = formationPrefabsteam1[selectedFormat];
            GameObject cloneteam1 = Instantiate(prefabteam1, spawnPoint.position, Quaternion.identity);
            //label.text = prefab.name;
            cloneteam1.SetActive(true);

            int formationOppTeam = Random.Range(0, 11);
            GameObject prefabteam2 = formationPrefabsteam2[formationOppTeam];
            GameObject cloneteam2 = Instantiate(prefabteam2, spawnPoint.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            //label.text = prefab.name;
            prefabteam2.SetActive(true);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
