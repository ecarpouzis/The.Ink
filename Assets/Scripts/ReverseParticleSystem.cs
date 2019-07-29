using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseParticleSystem : MonoBehaviour
{

    ParticleSystem particleSystem;

    float simulationTime = 0;

    public float startTime = 0f;
    public float simulationSpeedScale = 1.0f;

    void Initialize()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>(false);
    }

    void OnEnable()
    {
        if (particleSystem == null)
        {
            Initialize();
        }
        startTime = GameController.G.currentTimePoint;
        simulationTime = startTime;
        particleSystem.Simulate(startTime, true, false, true);
    }

    private void OnDisable()
    {
        particleSystem.Simulate(GameController.G.currentTimePoint, true, false, true);
        particleSystem.Play();
    }

    void Update()
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        bool useAutoRandomSeed = particleSystem.useAutoRandomSeed;
        particleSystem.useAutoRandomSeed = false;

        particleSystem.Play(false);

        float deltaTime = particleSystem.main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        simulationTime -= (deltaTime * particleSystem.main.simulationSpeed) * simulationSpeedScale;

        float curSimTime = startTime + simulationTime;
        particleSystem.Simulate(curSimTime, true, false, true);
        particleSystem.useAutoRandomSeed = useAutoRandomSeed;
        
    }
}