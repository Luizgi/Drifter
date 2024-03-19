using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource tireScreeching;
    public AudioSource engine;
    public AudioSource carHit;

    CarController carController;
    float desiredEnginePitch = 0.05f;
    float tireScreechPitch = 0.05f;

    private void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }
    private void Start()
    {
        audioMixer.SetFloat("SFXVol", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechingSFX();
    }
    private void UpdateEngineSFX()
    {
        float velocityMagnitude = carController.GetVelocityMagnitude();
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);
        engine.volume = Mathf.Lerp(engine.volume, desiredEngineVolume, Time.deltaTime);

        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engine.pitch = Mathf.Lerp(engine.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    private void UpdateTireScreechingSFX()
    {
        if(carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                tireScreeching.volume = Mathf.Lerp(tireScreeching.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                tireScreeching.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else
        {
            tireScreeching.volume = Mathf.Lerp(tireScreeching.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelocity = collision.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        carHit.pitch = Random.Range(0.95f, 1.05f);
        carHit.volume = volume;

        if (!carHit.isPlaying)
            carHit.Play();
    }
}
