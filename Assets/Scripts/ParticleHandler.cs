using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    float emissionRate = 0;

    CarController carController;
    ParticleSystem particleSystem;
    ParticleSystem.EmissionModule emissionModule;


    private void Awake()
    {
        carController = GetComponentInParent<CarController>();
        particleSystem = GetComponent<ParticleSystem>();
        emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        emissionRate = Mathf.Lerp(emissionRate, 0, Time.deltaTime * 5);
        emissionModule.rateOverTime = emissionRate;

        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                emissionRate = 30;
            else
                emissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}
