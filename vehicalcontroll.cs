using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class vehiclecontroll : MonoBehaviour
{
    private float accelerationInput;
    private float currentTurnInput;
    private float targetTurnInput;
    private Vector3 currentVelocity;

    public float maxTurnangle = 20f;
    public Rigidbody carbody;
    public float CarHorsepower = 400f;
    public float nitroBoost = 1.5f;       
    public float jumpForce = 300f;        
    public float handbrakeTorque = 3000f; 

    [Header("Wheel Collider")]
    public WheelCollider wc_FrontLeft;
    public WheelCollider wc_FrontRight;
    public WheelCollider wc_BackLeft;
    public WheelCollider wc_BackRight;

    private bool isNitroActive = false;
    private bool isHandbrakeActive = false;

    void Start()
    {
        carbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentVelocity = carbody.velocity;

       
        accelerationInput = Input.GetAxis("Vertical");

        
        targetTurnInput = Input.GetAxis("Horizontal");

        // Nitro 
        if (Input.GetKeyDown(KeyCode.L))
        {
            isNitroActive = true;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            isNitroActive = false;
        }

        // Handbrake
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isHandbrakeActive = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHandbrakeActive = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.K))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Vector3 combinedInput = (transform.forward ) * accelerationInput;
        float Dotproduct = Vector3.Dot(currentVelocity.normalized, combinedInput);

        if (Dotproduct < 0)
        {
            ApplyBrake();
        }
        else
        {
            ReleaseBrake();

            // Nitro 
            float finalHorsepower = isNitroActive ? CarHorsepower * nitroBoost : CarHorsepower;
            wc_BackLeft.motorTorque = accelerationInput * finalHorsepower * 1;
            wc_BackRight.motorTorque = accelerationInput * finalHorsepower * 1;
        }

        if (isHandbrakeActive)
        {
            ApplyHandbrake();
        }
        else
        {
            ReleaseHandbrake();
        }

        // Apply turning
        currentTurnInput = ApproachTargetValueWithIncrement(currentTurnInput, targetTurnInput, 0.07f);
        wc_FrontLeft.steerAngle = currentTurnInput * maxTurnangle;
        wc_FrontRight.steerAngle = currentTurnInput * maxTurnangle;

        
        Debug.Log($"Input = {(accelerationInput > 0 ? "w" : (accelerationInput < 0 ? "s" : "no key pressed"))}, " +
                  $"Velocity = {currentVelocity.normalized}, Dot product = {Dotproduct}");
    }

    private void ApplyBrake()
    {
        wc_BackLeft.brakeTorque = 1000f;
        wc_BackRight.brakeTorque = 1000f;
        wc_FrontLeft.brakeTorque = 1000f;
        wc_FrontRight.brakeTorque = 1000f;

        wc_BackLeft.motorTorque = 0;
        wc_BackRight.motorTorque = 0;
    }

    private void ReleaseBrake()
    {
        wc_BackLeft.brakeTorque = 0;
        wc_BackRight.brakeTorque = 0;
        wc_FrontLeft.brakeTorque = 0;
        wc_FrontRight.brakeTorque = 0;
    }

    private void ApplyHandbrake()
    {
        wc_BackLeft.brakeTorque = handbrakeTorque;
        wc_BackRight.brakeTorque = handbrakeTorque;
    }

    private void ReleaseHandbrake()
    {
        wc_BackLeft.brakeTorque = 0;
        wc_BackRight.brakeTorque = 0;
    }

    private void Jump()
    {
        if (Mathf.Abs(carbody.velocity.y) < 0.1f) 
        {
            carbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private float ApproachTargetValueWithIncrement(float currentValue, float targetValue, float increment)
    {
        if (currentValue == targetValue)
        {
            return currentValue;
        }
        else
        {
            return currentValue < targetValue ? currentValue + increment : currentValue - increment;
        }
    }
}
