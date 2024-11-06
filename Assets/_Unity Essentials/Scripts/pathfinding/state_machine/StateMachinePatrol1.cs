using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachinePatrol1 : MonoBehaviour
{
    public enum State
    {
        Idle,
        Seek,
        Patrol
    }
    public Transform otherGuardTransform;
    private StateMachinePatrol1 otherStateMachine;
    public State currentState;
    public PathFinding pathFinding;
    public Transform[] patrolPoints; // Puntos de patrullaje
    public Transform cuartel;
    private int patrolIndex = 0;

    // Variables para la energía de la patrulla
    public float maxEnergy = 50.0f; // Energía máxima del guardia
    public float currentEnergy;    // Energía actual del guardia
    public float energyDrainRate = 1.0f; // Cuánto se drena la energía por segundo
    public float energyRechargeRate = 0.5f; // Velocidad de recarga de energía en Idle


    void Start()
    {

        if (otherGuardTransform != null)
        {
            otherStateMachine = otherGuardTransform.GetComponent<StateMachinePatrol1>();
        }

        pathFinding.enabled = false;
        // currentState = State.Patrol; // Estado inicial
        currentEnergy = maxEnergy;
    }

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

    void Idle()
    {

        // Recargar energía en el estado de descanso
        currentEnergy += energyRechargeRate * Time.deltaTime;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy); // No exceder energía máxima

        if (pathFinding.character != null)
        {
            pathFinding.character.velocity = Vector3.zero;
            pathFinding.character.steering.linear = Vector3.zero;
        }

        // El agente permanece inactivo, aquí puedes definir una condición para cambiar de estado
        if (Input.GetKeyDown(KeyCode.S)) // Cambia a Seek al presionar "S"
        {
            currentState = State.Seek;
        }
        // else if (Input.GetKeyDown(KeyCode.P))
        // {
        //     currentState = State.Patrol;
        // }

        // patrullar cuando el otro esté descansando y yo tenga la bateria al maximo
        if (otherStateMachine.currentState.ToString() == State.Idle.ToString() && currentEnergy >= maxEnergy)
        {
            currentState = State.Patrol;
        }
    }

    void Seek()
    {
        // Activa el PathFinding para dirigirse al objetivo
        // if (pathFinding != null && pathFinding.target != null)
        // {
        //     pathFinding.enabled = true; // Asegura que el PathFinding esté activo
        // }

        pathFinding.enabled = true;
        pathFinding.target = cuartel.gameObject;
        // Condición para cambiar de estado al alcanzar el objetivo
        if (Vector3.Distance(transform.position, pathFinding.target.transform.position) < 0.6f)
        {
            currentState = State.Idle;
            pathFinding.enabled = false; // Desactiva el PathFinding
        }
    }

    void Patrol()
    {
        // Navega entre puntos de patrullaje
        if (patrolPoints.Length > 0)
        {
            pathFinding.target = patrolPoints[patrolIndex].gameObject; // Actualiza el objetivo en PathFinding
            pathFinding.enabled = true;

            if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < 0.6f)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length; // Cambia al siguiente punto
            }

            // Drenar energía mientras patrulla
            currentEnergy -= energyDrainRate * Time.deltaTime;
            // Debug.Log(currentEnergy);
            // State otherState = otherStateMachine.currentState;



            // cambiar de estado cuando se drene la energia
            if (currentEnergy <= 0)
            {
                pathFinding.enabled = false;
                currentState = State.Seek;
            }
        }
    }
}
