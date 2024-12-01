using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachinePatrol : MonoBehaviour
{
    public enum State
    {
        Idle,
        Seek,
        Patrol
    }

    public State currentState;
    public PathFinding pathFinding;
    public float maxEnergy; // Energía máxima del guardia
    public float currentEnergy; // Energía actual del guardia
    public float energyDrainRate; // Cuánto se drena la energía por segundo
    public float energyRechargeRate; // Velocidad de recarga de energía en Idle

    // Método abstracto para inicialización
    public abstract void Initialize();

    // Métodos abstractos para cada estado
    public abstract void Idle();
    public abstract void Seek();
    public abstract void Patrol();

    // Update implementado en la clase base para manejar transiciones de estado
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Seek:
                Seek();
                break;
            case State.Patrol:
                Patrol();
                break;
        }
    }
}
