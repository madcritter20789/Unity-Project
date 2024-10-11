using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using static Controller;
using System;


[System.Serializable]
public class RaceResult
{
    public string carName;
    public int position;
    public float timeTaken;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI[] finalPanelList;
    //[SerializeField] TextMeshProUGUI playerPositionText;

    [SerializeField] public int time;

    [SerializeField] Controller playerController;
    [SerializeField] public Controller[] PlayerNames;
    [SerializeField] public List<Controller> PlayerCurrentNames;

    [SerializeField] public List<vehicles> presentvehicles = new List<vehicles>();

    private bool raceFinished = false;
    private List<RaceResult> raceResults = new List<RaceResult>();


    [Header("Result Panel")]
    [SerializeField] GameObject resultPanel;  // Panel that shows race results
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject ImagePanel;
    [SerializeField] TextMeshProUGUI resultText; // Text element to display race results
    [SerializeField] TextMeshProUGUI PositionText;

    [Header("Player Info")]
    [SerializeField] TextMeshProUGUI currentLap;
    [SerializeField] TextMeshProUGUI currentPosition;
    [SerializeField] TextMeshProUGUI currentTime;
    [SerializeField] TextMeshProUGUI currentSpeed;

    private void Awake()
    {
        ImagePanel.SetActive(false);

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
        PlayerNames = FindObjectsOfType<Controller>();
        PlayerCurrentNames = new List<Controller>();
        EndPanel.SetActive(false);
        foreach (Controller r in PlayerNames)
        {
            PlayerCurrentNames.Add(r);
        }

        // Initialize presentvehicles list based on PlayerCurrentNames
        foreach (Controller g in PlayerCurrentNames)
        {
            presentvehicles.Add(new vehicles(g.GetComponent<Controller>().position, g.GetComponent<Controller>().name));

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EndPanel.SetActive(false);

        StartCoroutine(CountDown());
        for (int i = 0; i < PlayerCurrentNames.Count; i++)
        {
            finalPanelList[i].gameObject.SetActive(true);
        }
        //playerPositionText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!raceFinished)
        {
            NamePanel();
            PlayerCurrentPosition();
            PlayerCurrentLap();
            PlayerCurrentSpeed();
            PlayerCurrentTime();
        }

    }

    IEnumerator CountDown()
    {
        while (time > 0)
        {
            timerText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        timerText.text = "GO";
        yield return new WaitForSeconds(1f);
        timerText.gameObject.SetActive(false);
    }
    private void NamePanel()
    {
        SortPosition(); // Sort the positions before updating the leaderboard

        for (int i = 0; i < PlayerCurrentNames.Count; i++)
        {
            // Show the player name and current position for everyone on the leaderboard
            //finalPanelList[i].text = (i + 1) + " " + PlayerCurrentNames[i].gameObject.name + " Position: " + PlayerCurrentNames[i].GetComponent<Controller>().position;
            finalPanelList[i].text = (i + 1) + " " + PlayerCurrentNames[i].gameObject.name;

            // Check if the current player in the list is the main player
            if (PlayerCurrentNames[i] == playerController)
            {
                // Display player's rank in the form of "1/5"
                //playerPositionText.gameObject.SetActive(true);
                //playerPositionText.text = (i + 1) + "/" + PlayerCurrentNames.Count; // Example: "1/5"
            }
        }
    }
    public void PlayerCurrentPosition()
    {
        SortPosition(); // Sort the positions before updating the leaderboard

        for (int i = 0; i < PlayerCurrentNames.Count; i++)
        {
            // Check if the current player in the list is the main player
            if (PlayerCurrentNames[i] == playerController)
            {
                currentPosition.text = "Position: " + (i + 1) + "/" + PlayerCurrentNames.Count; // Example: "1/5"
            }
        }
    }
    public void PlayerCurrentLap()
    {
        SortPosition(); // Sort the positions before updating the leaderboard
        if (playerController.lapCount == 0)
        {
            currentLap.text = "Lap: 1/3";
        }
        else if (playerController.lapCount == 2)
        {
            currentLap.text = "Lap: 2/3";
        }
        else
        {
            currentLap.text = "Lap: 3/3";
        }
    }
    public void PlayerCurrentSpeed()
    {
        int i = (int)Math.Ceiling(playerController.currentCarLocalVelocity.z);
        currentSpeed.text = i.ToString() + " Kmph";
    }
    public void PlayerCurrentTime()
    {
        if(playerController.laptime<=3)
        {
            currentTime.text = "Time: 0:00";
        }
        else
        {
            int timeTakenByPlayer = (int)(playerController.laptime - 3f);
            float secondsRemainder = Mathf.Floor((timeTakenByPlayer % 60) * 100) / 100.0f;
            int minutes = ((int)(timeTakenByPlayer / 60)) % 60;
            currentTime.text = "Time: " + System.String.Format("{0:00}:{1:00}", minutes, secondsRemainder);
        }
 
    }
    private void SortPosition()
    {
        // Sort PlayerCurrentNames by the position in descending order
        PlayerCurrentNames = PlayerCurrentNames.OrderByDescending(p => p.GetComponent<Controller>().position).ToList();
    }
    public void OnRaceFinished(Controller player, float timeTaken)
    {
        raceFinished = true;

        // Store result for player
        raceResults.Add(new RaceResult { carName = player.gameObject.name, position = player.position, timeTaken = timeTaken-3f });

        // Show results panel when all players finish
        /*
        if (raceResults.Count == PlayerCurrentNames.Count)
        {
            //DisplayRaceResults();
        }
        */
        ImagePanel.SetActive(true);
        StartCoroutine(displayNameDelay());
    }

    IEnumerator displayNameDelay()
    {
        yield return new WaitForSeconds(0.5f);
        DisplayRaceResults();
    }
    private void DisplayRaceResults()
    {
        EndPanel.SetActive(true);

        resultPanel.SetActive(true);
        resultText.text = "Race Results:\n";

        // Sort results by position
        var sortedResults = raceResults.OrderBy(r => r.position).ToList();

        foreach (var result in sortedResults)
        {
            int timeTakenByPlayer = (int)result.timeTaken;
            float secondsRemainder = Mathf.Floor((timeTakenByPlayer % 60) * 100) / 100.0f;
            int minutes = ((int)(timeTakenByPlayer / 60)) % 60;
            //resultText.text += $"{result.position}. {result.carName} - Time: {result.timeTaken:F2}s\n";
            resultText.text += $"{result.carName} - Time Taken: {System.String.Format("{0:00}:{1:00}", minutes, secondsRemainder)}s\n";
        }
        // Display the player's position in the form "1/5" or "2/5", etc.
        var playerResult = sortedResults.FirstOrDefault(r => r.carName == playerController.gameObject.name);
        if (playerResult != null)
        {
            int playerRank = sortedResults.IndexOf(playerResult) + 1; // Player's rank (1-based index)
            int totalPlayers = PlayerCurrentNames.Count;

            // Add player's rank in the format "1/5"
            //resultText.text += $"\nYour Position: {playerRank}/{totalPlayers}";
        }

        for (int i = 0; i < PlayerCurrentNames.Count; i++)
        {
            PlayerCurrentNames[i].engineSound.mute = true;
            PlayerCurrentNames[i].skidSound.mute = true;
            // Check if the current player in the list is the main player
           
            if (PlayerCurrentNames[i] == playerController)
            {
                PositionText.text = "Place: "+(i + 1) + "/" + PlayerCurrentNames.Count; // Example: "1/5"
            }
           
        }
    }

}
