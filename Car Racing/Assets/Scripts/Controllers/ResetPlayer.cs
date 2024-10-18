using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> checkPoints;
    Vector3 minDistance1;
    Vector3 minDistance2;
    Vector3 ActualminDistance;
    int nodeNum;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.R))
        {
            distanceminimum();
        }
    }

    public void distanceminimum()
    {

        // Make sure there are checkpoints
        if (checkPoints.Count == 0) return;

        // Initialize minimum distance with a large value
        float minDistance = Mathf.Infinity;

        // Loop through all the checkpoints
        for (int i = 0; i < checkPoints.Count; i++)
        {
            // Calculate the distance from the player to the current checkpoint
            Vector3 distanceToCheckpoint = gameObject.transform.position - checkPoints[i].transform.position;

            // If this distance is less than the current minimum distance
            if (distanceToCheckpoint.magnitude < minDistance)
            {
                // Update the minimum distance and store the closest checkpoint index
                minDistance = distanceToCheckpoint.magnitude;
                ActualminDistance = distanceToCheckpoint;
                nodeNum = i;
            }
        }

        // Log the nearest checkpoint and move the player there
        Debug.Log("Nearest checkpoint index: " + nodeNum);
        Debug.Log("Distance to nearest checkpoint: " + ActualminDistance.magnitude);

        // Set the player's position to the nearest checkpoint
        gameObject.transform.position = checkPoints[nodeNum].transform.position;
        gameObject.transform.rotation = checkPoints[nodeNum].transform.rotation;
    }
}
