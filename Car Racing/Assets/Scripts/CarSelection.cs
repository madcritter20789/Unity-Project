using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject Ramp;
    public Button next;
    public Button prev;
    [SerializeField] float rotationSpeed;
    int index;
    [SerializeField] Slider[] carConfig;
    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("carIndex");
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }

        cars[index].GetComponent<Controller>();
        carConfig[0].value = cars[index].GetComponent<Controller>().acceleration;
        carConfig[1].value = cars[index].GetComponent<Controller>().steerStrength;
        carConfig[2].value = cars[index].GetComponent<Controller>().maxSpeed;

        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        Ramp.transform.Rotate(0, rotationSpeed, 0);
    }


    public void Next()
    {
        index++;
        if (index >= cars.Length) // Ensure the index is within bounds
        {
            index = 0; // Loop back to the first car if the index exceeds the array length
        }

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
        }
        cars[index].SetActive(true);

        cars[index].GetComponent<Controller>();
        carConfig[0].value = cars[index].GetComponent<Controller>().acceleration;
        carConfig[1].value = cars[index].GetComponent<Controller>().steerStrength;
        carConfig[2].value = cars[index].GetComponent<Controller>().maxSpeed;
    

        PlayerPrefs.SetInt("carIndex", index);
        
    }

    public void Prev()
    {
        index--;
        if (index < 0) // Ensure the index is within bounds
        {
            index = cars.Length - 1; // Loop back to the last car if the index goes below 0
        }

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
        }
        cars[index].SetActive(true);

        cars[index].GetComponent<Controller>();
        carConfig[0].value = cars[index].GetComponent<Controller>().acceleration;
        carConfig[1].value = cars[index].GetComponent<Controller>().steerStrength;
        carConfig[2].value = cars[index].GetComponent<Controller>().maxSpeed;

        PlayerPrefs.SetInt("carIndex", index);
        
    }


    public void SelectCar()
    {
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("carIndex"));
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
