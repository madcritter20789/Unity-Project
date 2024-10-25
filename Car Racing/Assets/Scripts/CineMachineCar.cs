using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineCar : MonoBehaviour
{
    [SerializeField] Controller PlayerController;
    CinemachineVirtualCamera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        // Find all controllers in the scene
        Controller[] allControllers = FindObjectsOfType<Controller>();

        foreach (Controller controller in allControllers)
        {
            // Check if the controller has driver set to "player"
            if (controller.driveController == 0)
            {
                // Assign this controller to the player controller reference
                PlayerController = controller;
                break; // Stop the loop once the correct controller is found
            }
        }
        camera.Follow = PlayerController.transform;
        camera.LookAt = PlayerController.transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
