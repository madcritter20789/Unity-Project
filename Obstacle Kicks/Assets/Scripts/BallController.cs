using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Ensures the GameObject has a Rigidbody component
[RequireComponent(typeof(Collider))]  // Ensures the GameObject has a Collider component
public class BallController : MonoBehaviour
{
    // Serialized fields to configure values in the Unity Inspector
    [SerializeField] private float power = 10f; // The force multiplier applied to the ball
    [SerializeField] private float maxDrag = 10f; // Maximum distance for drag input
    [SerializeField] private Rigidbody rb; // Reference to the ball's Rigidbody
    [SerializeField] private LineRenderer lR; // LineRenderer for visualizing drag direction

    // Variables to track mouse positions and state
    private Vector3 mousePressDownPos; // Position of the mouse when pressed
    private Vector3 mouseReleasePos;   // Position of the mouse when released
    private bool isShoot;              // Flag to prevent multiple shots during cooldown
    private float cooldownTime = 3f;   // Minimum time between shots
    private float lastShootTime;       // Records the last time a shot was made
   
    private GameManager gameManager;    // Reference to the GameManager

    void Start()
    {
        // Assign Rigidbody if not already assigned
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Assign LineRenderer if not already assigned
        if (lR == null)
            lR = GetComponent<LineRenderer>();

        lR.enabled = false; // Disable LineRenderer at the start

        // Find GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        if (gameManager.GetPlayerSide() == "Blue" || gameManager.GetPlayerSide() == "Red") // Ensure only the active player can act
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        // Handle mouse button press
        if (Input.GetMouseButtonDown(0))
        {
            mousePressDownPos = Input.mousePosition;
            lR.enabled = true;
        }

        // Handle dragging (visualization only)
        if (Input.GetMouseButton(0) && lR.enabled)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDirection = GetDragDirection(currentMousePos);
            VisualizeDrag(dragDirection);
        }

        // Handle mouse button release
        if (Input.GetMouseButtonUp(0))
        {
            lR.enabled = false;
            mouseReleasePos = Input.mousePosition;

            if (Time.time - lastShootTime >= cooldownTime)
            {
                Vector3 force = GetDragDirection(mouseReleasePos);
                Shoot(force);
                lastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDragDirection(Vector3 currentMousePos)
    {
        // Calculate the drag vector in screen space
        Vector3 dragVector = mousePressDownPos - currentMousePos; // Drag is opposite of mouse movement

        // Convert screen-space drag to world-space direction
        Ray cameraRay = Camera.main.ScreenPointToRay(currentMousePos); // Ray from camera through mouse position
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Horizontal plane at y=0

        if (groundPlane.Raycast(cameraRay, out float enter)) // Check if the ray intersects the ground plane
        {
            Vector3 currentWorldPos = cameraRay.GetPoint(enter); // Get the intersection point
            cameraRay = Camera.main.ScreenPointToRay(mousePressDownPos); // Ray from initial mouse press
            groundPlane.Raycast(cameraRay, out enter);
            Vector3 startWorldPos = cameraRay.GetPoint(enter); // Intersection point of the initial press

            dragVector = startWorldPos - currentWorldPos; // Calculate world-space drag direction
        }

        dragVector.y = 0; // Ensure no vertical force
        return Vector3.ClampMagnitude(dragVector, maxDrag); // Limit drag length to maxDrag
    }

    private void Shoot(Vector3 force)
    {
        if (isShoot) return; // Prevent shooting if already shot

        Vector3 finalForce = force * power; // Multiply force by the power factor
        rb.AddForce(finalForce, ForceMode.Impulse); // Apply the force as an impulse
        isShoot = true; // Mark as shot

        // Start cooldown coroutine
        StartCoroutine(ResetShoot());
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown time
        isShoot = false; // Allow shooting again

        // Switch turn to the other player
        gameManager.ChangePlayer();
    }

    private void VisualizeDrag(Vector3 dragDirection)
    {
        // Update LineRenderer to show drag direction
        Vector3 startPoint = transform.position; // Start at the ball's position
        Vector3 endPoint = startPoint + dragDirection; // End at the drag target

        lR.positionCount = 2; // Line has two points
        lR.SetPosition(0, startPoint); // Start of the line
        lR.SetPosition(1, endPoint); // End of the line
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox1")
        {
            Debug.Log(" Team 1 Goal");
            rb.velocity = Vector3.zero;
            gameManager.playerTeamBlue.goals += 1;
            gameManager.goalCeleberationEffect1.Emit(100);
            gameManager.UpdateScoreTeam();

        }
        else if (other.gameObject.tag == "Hitbox2")
        {
            Debug.Log("Team 2 Goal");
            rb.velocity = Vector3.zero;
            gameManager.playerTeamRed.goals += 1;
            gameManager.goalCeleberationEffect2.Emit(100);
            gameManager.UpdateScoreTeam();
        }
    }
}
