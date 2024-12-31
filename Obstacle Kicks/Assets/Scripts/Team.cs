using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/TeamPlayer")]
public class Team : ScriptableObject
{
    public Image panel;
    public TextMeshProUGUI text;
    public TextMeshProUGUI scoreText;
    public int goals;
  
}
