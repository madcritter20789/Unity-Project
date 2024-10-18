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
