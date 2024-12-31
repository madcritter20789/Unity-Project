using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text;
    public TextMeshProUGUI scoreText;
    public int goals;
    public string teamName;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}


public class GameManager : MonoBehaviour
{
    public string playerSide;

    [Header("Player Properties")]
    public Player playerTeamBlue;
    public Player playerTeamRed;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    [Header("Celberation Particle Effect")]
    public ParticleSystem goalCeleberationEffect1;
    public ParticleSystem goalCeleberationEffect2;

    [SerializeField] GameObject ballPrefab;

    [Header("Countdown Timer")]
    [SerializeField] TextMeshProUGUI countDowntimerText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingtime;
    int time = 3;

    [Header("Endpanel Timer")]
    [SerializeField] GameObject endPanel;
    [SerializeField] TextMeshProUGUI teamWonText;
    [SerializeField] TextMeshProUGUI teamWonScoreText;

    [SerializeField] bool isGivenFreeTime = false;
    BallController ball;

    private void Awake()
    {
        playerSide = PlayerPrefs.GetString("team");
        Debug.Log(PlayerPrefs.GetString("team"));
        isGivenFreeTime = false;
        if (playerSide == "Blue")
        {
            SetPlayerColors(playerTeamBlue, playerTeamRed);
        }
        else
        {
            SetPlayerColors(playerTeamRed, playerTeamBlue);
        }
        ball = FindObjectOfType<BallController>();
        ball.enabled = false;
        endPanel.SetActive(false);
        playerTeamBlue.teamName = "Blue";
        playerTeamRed.teamName = "Red";

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateScoreTeam();
        if(!countDowntimerText.isActiveAndEnabled)
        {
            if (remainingtime > 0)
            {
                remainingtime -= Time.deltaTime;
            }
            else if (remainingtime < 0)
            {
                remainingtime = 0;
                if(playerTeamBlue.goals == playerTeamRed.goals && !isGivenFreeTime)
                {
                    remainingtime += 15;
                    isGivenFreeTime = true;
                    StartCoroutine(ExtraTime());
                }
                else
                {
                    openEndPanel();
                }
            }
            int minute = Mathf.FloorToInt(remainingtime / 60);
            int seconds = Mathf.FloorToInt(remainingtime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minute, seconds);
        }

    }

    IEnumerator CountDown()
    {
        while (time > 0)
        {
            countDowntimerText.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        countDowntimerText.text = "Start";
        yield return new WaitForSeconds(1f);
        countDowntimerText.gameObject.SetActive(false);
        ball.enabled = true;
    }

    IEnumerator ExtraTime()
    {
        countDowntimerText.gameObject.SetActive(true);
        countDowntimerText.text = "+15 sec Extra Time";
        yield return new WaitForSeconds(1.5f);
        countDowntimerText.gameObject.SetActive(false);

    }

    void openEndPanel()
    {
        endPanel.SetActive(true);
        if(PlayerPrefs.GetString("team") == playerTeamBlue.teamName)
        {
            if (playerTeamBlue.goals > playerTeamRed.goals)
            {
                teamWonText.text = "Team "+playerTeamBlue.teamName.ToString() + " won";
                teamWonScoreText.text = "Scored "+playerTeamBlue.goals.ToString()+" goals";
            }
            else if (playerTeamBlue.goals < playerTeamRed.goals)
            {
                teamWonText.text = "Team "+playerTeamBlue.teamName.ToString() + " lost";
                teamWonScoreText.text = "Scored " + playerTeamBlue.goals.ToString() + " goals";
            }
            else
            {
                teamWonText.text = "It is a draw.";
                teamWonScoreText.text = "Both team scored" + playerTeamBlue.goals + "goals";
            }

        }
        else if (PlayerPrefs.GetString("team") == playerTeamRed.teamName)
        {
            if (playerTeamBlue.goals < playerTeamRed.goals)
            {
                teamWonText.text = "Team " + playerTeamRed.teamName.ToString() + " won";
                teamWonScoreText.text = "Scored " + playerTeamRed.goals.ToString() + " goals";
            }
            else if (playerTeamBlue.goals > playerTeamRed.goals)
            {
                teamWonText.text = "Team " + playerTeamRed.teamName.ToString() + " lost";
                teamWonScoreText.text = "Scored " + playerTeamRed.goals.ToString() + " goals";
            }
            else
            {
                teamWonText.text = "It is a draw.";
                teamWonScoreText.text = "Both team scored" + playerTeamBlue.goals + "goals";
            }
        }


        /*
        if (playerTeamBlue.goals> playerTeamRed.goals)
        {

        }
        else if (playerTeamBlue.goals < playerTeamRed.goals)
        {
            teamWonText.text = playerTeamBlue.ToString();
            teamWonScoreText.text = playerTeamRed.goals.ToString();
        }
        */
    }

    #region Switch Player


    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void SetPlayerColors(Player newplayer, Player oldplayer)
    {
        newplayer.panel.color = activePlayerColor.panelColor;
        newplayer.text.color = activePlayerColor.textColor;
        oldplayer.panel.color = inactivePlayerColor.panelColor;
        oldplayer.text.color = inactivePlayerColor.textColor;
    }
    public void ChangePlayer()
    {
        playerSide = (playerSide == "Blue") ? "Red" : "Blue";

        if (playerSide == "Blue")
        {
            SetPlayerColors(playerTeamBlue, playerTeamRed);
        }
        else
        {
            SetPlayerColors(playerTeamRed, playerTeamBlue);
        }
    }

    #endregion


    public void UpdateScoreTeam()
    {
        playerTeamBlue.scoreText.text = playerTeamBlue.goals.ToString();
        playerTeamRed.scoreText.text = playerTeamRed.goals.ToString();
        Destroy(FindObjectOfType<BallController>());
        StartCoroutine(delayInSpawn());
    }

    IEnumerator delayInSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        UpdateBallPosition();
    }

    public void UpdateBallPosition()
    {
        Destroy(GameObject.FindWithTag("Player"));
        Vector3 pos = new Vector3(0, 0.15f, -1);
        Instantiate(ballPrefab, pos, Quaternion.identity);
    }

    public void Rematch()
    {
        SceneManager.LoadScene(3);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }

}
