using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler// required interface when using the OnPointerDown method.
{
    [Header("OnScreenInput")]

    [SerializeField] Controller PlayerController;

    // Start is called before the first frame update
    public enum ControlType { Left, Right, Accelerate, Decelerate, Brake }
    public ControlType controlType;


    private void Start()
    {
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (controlType)
        {
            case ControlType.Left:
                PlayerController.horizontalInput = -1;
                break;
            case ControlType.Right:
                PlayerController.horizontalInput = 1;
                break;
            case ControlType.Accelerate:
                PlayerController.verticalInput = 1;
                break;
            case ControlType.Decelerate:
                PlayerController.verticalInput = -1;
                break;
            /*
            case ControlType.Brake:
                PlayerController.isBraking = true; // You can add an "isBraking" flag to handle brake logic separately
                break;
            */
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (controlType)
        {
            case ControlType.Left:
            case ControlType.Right:
                PlayerController.horizontalInput = 0;
                break;
            case ControlType.Accelerate:
            case ControlType.Decelerate:
                PlayerController.verticalInput = 0;
                break;
            /*
            case ControlType.Brake:
                PlayerController.isBraking = false;
                break;
            */
        }
    }

}
