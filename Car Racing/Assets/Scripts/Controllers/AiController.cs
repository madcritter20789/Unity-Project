using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AiController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask drivable;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private GameObject[] tires = new GameObject[4];
    [SerializeField] private GameObject[] frontTiresParent = new GameObject[2];
    [SerializeField] private TrailRenderer[] skidMarks = new TrailRenderer[2];
    [SerializeField] private ParticleSystem[] skidSmokes = new ParticleSystem[2];
    [SerializeField] private AudioSource engineSound, skidSound;

    [Header("Suspension Settings")]
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float wheelRadius;

    private int[] wheelIsGrounded = new int[4];
    private bool isGrounded = false;

    [Header("Input")]
    private float moveInput = 0;
    private float steerInput = 0;

    [Header("Car Settings")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float steerStrength = 15f;
    [SerializeField] private AnimationCurve turningCurve;
    [SerializeField] private float dragCoefficient = 1f;
    [SerializeField] private float breakingDecelearation = 100f;
    [SerializeField] private float breakingDragCoefficient = 0.5f;


    private Vector3 currentCarLocalVelocity = Vector3.zero;
    private float carVelocityRatio = 0;

    [Header("Visuals")]
    [SerializeField] private float tireRotSpeed = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float minSideKidVelocity = 10f;

    [Header("Audio")]
    [SerializeField]
    [Range(0, 1)] private float minPitch = 1f;
    [SerializeField]
    [Range(1, 5)] private float maxPitch = 1f;


    GameManager gameManager;
    [SerializeField] public int lapTime = 0;
    private bool raceFinsished = false;

    // Start is called before the first frame update
    void Start()
    {
        carRB = GetComponent<Rigidbody>();

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetPlayerInput();

        if (raceFinsished)
        {
            moveInput = Mathf.Lerp(moveInput, 0f, Time.deltaTime);
        }
        else
        {
            if (gameManager.time <= 0)
            {
                AiInput();
            }
        }

    }

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalaculateCarVelocity();
        Movement();
        Visuals();
        EngineSound();
    }

    #region Movement

    private void Movement()
    {
        if (isGrounded)
        {
            Accelaration();
            Decelaration();
            Turn();
            SideWaysDrag();
        }
    }
    private void Accelaration()
    {
        if (currentCarLocalVelocity.z < maxSpeed)
        {
            carRB.AddForceAtPosition(acceleration * moveInput * carRB.transform.forward, accelerationPoint.position, ForceMode.Acceleration);

        }
    }

    private void Decelaration()
    {
        carRB.AddForce((Input.GetKey(KeyCode.Space) ? breakingDecelearation : deceleration) * carVelocityRatio * -carRB.transform.forward, ForceMode.Acceleration);
    }

    private void Turn()
    {
        carRB.AddRelativeTorque(steerStrength * steerInput * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * transform.up, ForceMode.Acceleration);
    }

    private void SideWaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;

        float dragMagnitude = -currentSidewaysSpeed * (Input.GetKey(KeyCode.Space) ? breakingDragCoefficient : dragCoefficient);

        Vector3 dragForce = carRB.transform.right * dragMagnitude;

        carRB.AddForceAtPosition(dragForce, carRB.worldCenterOfMass, ForceMode.Acceleration);
    }

    #endregion

    #region Visuals
    private void Visuals()
    {
        TireVisuals();
        Vfx();
    }
    private void TireVisuals()
    {
        float SteeringAngle = maxSteerAngle * steerInput;

        for (int i = 0; i < tires.Length; i++)
        {
            if (i < 2)
            {
                tires[i].transform.Rotate(Vector3.right, tireRotSpeed * carVelocityRatio * Time.deltaTime, Space.Self);
                frontTiresParent[i].transform.localEulerAngles = new Vector3(frontTiresParent[i].transform.localEulerAngles.x, SteeringAngle, frontTiresParent[i].transform.localEulerAngles.z);
            }
            else
            {
                tires[i].transform.Rotate(Vector3.right, tireRotSpeed * moveInput * Time.deltaTime, Space.Self);
            }
        }
    }

    public void Vfx()
    {
        if (isGrounded && Mathf.Abs(currentCarLocalVelocity.x) > minSideKidVelocity && carVelocityRatio > 0)
        {
            ToggleSkidMarks(true);
            ToggleSkidSmokes(true);
            ToggleSkidSound(true);
        }
        else
        {
            ToggleSkidMarks(false);
            ToggleSkidSmokes(false);
            ToggleSkidSound(false);
        }
    }


    public void ToggleSkidMarks(bool toggle)
    {
        foreach (var skidMark in skidMarks)
        {
            skidMark.emitting = toggle;
        }
    }
    public void ToggleSkidSmokes(bool toggle)
    {

        foreach (var smoke in skidSmokes)
        {
            if (toggle)
            {
                smoke.Play();
            }
            else
            {
                smoke.Stop();
            }
        }
    }

    private void SetTirePosition(GameObject tire, Vector3 targetPosition)
    {
        tire.transform.position = targetPosition;
    }

    #endregion

    #region Audio

    private void EngineSound()
    {
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocityRatio));
    }

    private void ToggleSkidSound(bool toggle)
    {
        skidSound.mute = !toggle;
    }

    #endregion

    #region Car Status Check

    private void GroundCheck()
    {
        int tempGroundedWHeels = 0;
        for (int i = 0; i < wheelIsGrounded.Length; i++)
        {
            tempGroundedWHeels += wheelIsGrounded[i];
        }

        if (tempGroundedWHeels > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

    }

    private void CalaculateCarVelocity()
    {
        currentCarLocalVelocity = transform.InverseTransformDirection(carRB.velocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
    }

    #endregion

    #region Input Handling
    /*
    private void GetPlayerInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
    */
    #endregion

    #region Suspension Function
    private void Suspension()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            //float maxLength = restLength + springTravel;
            float maxLength = restLength;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, drivable))
            {
                wheelIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = restLength - currentSpringLength / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float dampForce = damperStiffness * springVelocity;

                float springForce = springStiffness * springCompression;

                float netForce = springForce - dampForce;

                carRB.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);

                //Tire Visual
                SetTirePosition(tires[i], hit.point + rayPoints[i].up * wheelRadius);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelIsGrounded[i] = 0;

                //ToreVodusl
                SetTirePosition(tires[i], rayPoints[i].position - rayPoints[i].up * maxLength);

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (wheelRadius + maxLength) * -rayPoints[i].up, Color.green);
            }
        }
    }

    #endregion

    /*
    private void AiInput()
    {

    }
    
    {
        try
        {
            if (Vector3.Distance(transform.position, currentNode.transform.position) <= distance)
                reachedDestination();
            {
                moveInput = Random.Range(0, 1);
                steerInput = Random.Range(0, 1);
            }
        }
        catch { }
    }
    */
    [Header("AI")]
    public float distance;
    public Node currentNode;
    void checkDistance()
    {
        try
        {
            if (Vector3.Distance(transform.position, currentNode.transform.position) <= distance)
                reachedDestination();
        }
        catch { }
    }
    private void AiInput()
    {
        if (currentNode == null) return;

        // Calculate the direction towards the current node
        Vector3 directionToNode = (currentNode.nextWaypoint.transform.position - transform.position).normalized;

        // Calculate the angle between the car's forward direction and the target direction
        float angleToNode = Vector3.SignedAngle(transform.forward, directionToNode, Vector3.up);

        // Convert the angle to a steering input
        steerInput = Mathf.Clamp(angleToNode / maxSteerAngle, -1f, 1f);

        // Calculate the distance to the current node
        float distanceToNode = Vector3.Distance(transform.position, currentNode.transform.position);

        // Accelerate the car towards the node, slow down as it gets closer
        if (distanceToNode > distance)
        {
            moveInput = Mathf.Lerp(moveInput, 1f, Time.deltaTime);
        }
        else
        {
            moveInput = Mathf.Lerp(moveInput, 0f, Time.deltaTime);
            if (distanceToNode <= distance)
            {
                reachedDestination();
            }
        }
    }
    private void reachedDestination()
    {
        if (currentNode.nextWaypoint == null)
        {
            currentNode = currentNode.previousWaypoint;
            return;
        }
        if (currentNode.previousWaypoint == null)
        {
            currentNode = currentNode.nextWaypoint;
            return;
        }
        else
            currentNode = currentNode.nextWaypoint;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (currentNode != null)
            Gizmos.DrawSphere(currentNode.transform.position, 0.5f);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag=="FinishWall")
        {
 
            lapTime += 1;
            if(lapTime>=6)
            {
                raceFinsished = true;
                //gameManager.playerList(gameObject.name);
                Debug.Log("Race Finished");
            }
        }
    }
}