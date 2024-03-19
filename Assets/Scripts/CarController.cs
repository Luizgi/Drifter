using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor = 0.95f;
    public float accelarationFactor = 30.0f;
    public float turnFactor = 1.5f;
    public float maxSpeed = 20;

    [Header("Local")]
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }



    private void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, rb2d.velocity);
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;
        if(velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) 
            return;
        if (rb2d.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;


        if (accelerationInput == 0)
            rb2d.drag = Mathf.Lerp(rb2d.drag, 3.0f, Time.fixedDeltaTime * 3);
        else 
            rb2d.drag = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelarationFactor;
        rb2d.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = (rb2d.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        rb2d.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb2d.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb2d.velocity, transform.right);

        rb2d.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
    
    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb2d.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if(accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;

    }
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return rb2d.velocity.magnitude;
    }
}
