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

    [Header("Sprites")]
    public SpriteRenderer carSprite;
    public SpriteRenderer shadowSprite;

    [Header("Jumping")]
    public AnimationCurve jumpCurve;
    public ParticleSystem landing;

    [Header("Local")]
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;
    bool isJumping = false;
    Rigidbody2D rb2d;
    Collider2D collider;
    CarSfxHandler sfxHandler;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponentInChildren<Collider2D>();
        sfxHandler = GetComponent<CarSfxHandler>();
    }


    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }



    private void ApplyEngineForce()
    {
        if(isJumping && accelerationInput < 0)
        {
            accelerationInput = 0;
        }

        velocityVsUp = Vector2.Dot(transform.up, rb2d.velocity);
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;
        if(velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) 
            return;
        if (rb2d.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0 && !isJumping)
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

        if (isJumping)
            return false;

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

    public void Jump(float jumpHeighScale, float jumpPushScale)
    {
        if(!isJumping)
            StartCoroutine(JumpC(jumpHeighScale, jumpPushScale));
    }
    
    private IEnumerator JumpC(float jumpHeighScale, float jumpPushScale)
    {
        isJumping = true;

        float startTime = Time.time;
        float duration = rb2d.velocity.magnitude * 0.05f;

        jumpHeighScale = jumpHeighScale * rb2d.velocity.magnitude * 0.05f;
        jumpHeighScale = Mathf.Clamp(jumpHeighScale, 0.0f, 1.0f);

        collider.enabled = false;

        sfxHandler.PlayJumpSfx();

        carSprite.sortingLayerName = "Flying";
        shadowSprite.sortingLayerName = "Flying";

        rb2d.AddForce(rb2d.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        while(isJumping)
        {
            float completedPercentage = (Time.time - startTime) / duration;
            completedPercentage = Mathf.Clamp01(completedPercentage);

            carSprite.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(completedPercentage) * jumpHeighScale;

            shadowSprite.transform.localScale = carSprite.transform.localScale * 0.75f;

            shadowSprite.transform.localPosition = new Vector3(1, -1, 0f) * 3f * jumpCurve.Evaluate(completedPercentage) * jumpHeighScale;

            if (completedPercentage == 1f)
                break;

            yield return null;
        }

        if(Physics2D.OverlapCircle(transform.position, 1.5f))
        {
            isJumping = false;

            Jump(0.2f, 0.6f);
        }
        else
        {
            carSprite.transform.localScale = Vector3.one;

            shadowSprite.transform.localPosition = Vector3.zero;
            shadowSprite.transform.localScale = carSprite.transform.localScale;

            collider.enabled = true;



            carSprite.sortingLayerName = "Default";
            shadowSprite.sortingLayerName = "Default";
            if(jumpHeighScale > 0.2f)
            {
                landing.Play();

                sfxHandler.PlayLandingSfx();
            }

            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jump"))
        {
            JumpData jumpData = collision.GetComponent<JumpData>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }

    }
}
