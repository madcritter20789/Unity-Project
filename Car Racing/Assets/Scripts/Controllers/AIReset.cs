using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIReset : MonoBehaviour
{
    [Header("Nodes")]
    public Node currentNode; // Reference to the AI's current node
    public int currentPosition; // AI's current position in the node system
    public int lastPosition; // AI's last known position in the node system

    [SerializeField] List<GameObject> checkPoints; // List of checkpoints (or nodes) the AI car follows.
    [SerializeField] private float resetTime = 10f; // Time limit for the AI to reach the next node.

    private Vector3 lastNodePosition; // Stores the position of the last node the AI successfully reached.
    Vector3 ActualminDistance;
    private int nodeNum; // Tracks the current node the AI is moving toward.

    private float timer; // Timer to track how long the AI has been stuck.
    private bool isStuck; // Flag to check if the AI is stuck.

    private Controller aiController; // Reference to the AI's controller script

    // Start is called before the first frame update
    void Start()
    {
        aiController = GetComponent<Controller>(); // Get the AI's controller script

        if (aiController != null)
        {
            currentNode = aiController.currentNode; // Initialize with the AI's current node
            if (currentNode != null)
            {
                lastNodePosition = currentNode.transform.position; // Set the last known node position
                lastPosition = int.Parse(currentNode.name); // Initialize lastPosition from node name
                currentPosition = lastPosition; // Initialize currentPosition to match lastPosition
                timer = resetTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aiController != null && currentNode != null)
        {
            CheckIfStuck();
            UpdateNodeTracking();

            if (ReachedNode())
            {
                // Reset the timer when the AI reaches the node
                timer = resetTime;
                lastNodePosition = currentNode.transform.position; // Update last node position
                lastPosition = currentPosition; // Update the lastPosition to the current node
                isStuck = false; // AI is not stuck anymore
            }
        }
    }

    // Function to check if the AI is stuck
    void CheckIfStuck()
    {
        if (lastPosition == currentPosition)
        {
            // If the AI's position hasn't changed, start the stuck timer
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                // The AI has been stuck for longer than the reset time
                isStuck = true;
                ResetAIPosition();
                timer = resetTime; // Reset the timer after resetting the AI's position
            }
        }
        else
        {
            // If the AI is moving, reset the timer and update the last position
            timer = resetTime;
            lastPosition = currentPosition;
        }
    }

    // Function to reset the AI's position to the nearest checkpoint or node
    void ResetAIPosition()
    {
        // Make sure there are checkpoints
        if (checkPoints.Count == 0) return;

        // Initialize minimum distance with a large value
        float minDistance = Mathf.Infinity;

        // Loop through all the checkpoints
        for (int i = 0; i < checkPoints.Count; i++)
        {
            // Calculate the distance from the AI to the current checkpoint
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

        // Log the nearest checkpoint and move the AI there
        Debug.Log("AI is stuck. Resetting position to nearest checkpoint at index: " + nodeNum);
        Debug.Log("Distance to nearest checkpoint: " + ActualminDistance.magnitude);

        // Set the AI's position to the nearest checkpoint
        gameObject.transform.position = checkPoints[nodeNum].transform.position;
        gameObject.transform.rotation = checkPoints[nodeNum].transform.rotation;
    }

    // Function to check if the AI reached its current target node
    bool ReachedNode()
    {
        // Calculate the distance between the AI and the current node
        float distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position);

        // If the AI is within a certain distance of the node, consider it reached
        return distanceToNode <= 15f; // Adjust this threshold as needed
    }

    // Update node tracking by checking the current node in the AI's controller
    void UpdateNodeTracking()
    {
        if (aiController != null && aiController.currentNode != null)
        {
            currentNode = aiController.currentNode;
            currentPosition = int.Parse(currentNode.name); // Use node name as the identifier (assuming it's an int)
        }
    }
}