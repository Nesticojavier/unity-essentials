using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateMachine : MonoBehaviour
{
    public enum State
    {
        Hidden,
        TakeCover,
        SeekCheese,
        EatCheese,

    }
    // public Transform otherGuardTransform;
    public Transform[] cheeses;
    public StateMachinePatrol1[] patrol_cars;
    public Transform home;
    public State currentState = State.Hidden;
    public PathFinding pathFinding;

    void Start()
    {
        pathFinding.enabled = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Hidden:
                Hidden();
                break;
            case State.TakeCover:
                TakeCover();
                break;
            case State.SeekCheese:
                SeekCheese();
                break;
            case State.EatCheese:
                EatCheese();
                break;
        }
    }

    void Hidden()
    {

        if (pathFinding.character != null)
        {
            pathFinding.character.velocity = Vector3.zero;
            pathFinding.character.steering.linear = Vector3.zero;
        }


        // salir si todos los patrulleros están quietos
        if (patrol_cars[0].currentState.ToString() == "Idle" && patrol_cars[1].currentState.ToString() == "Idle")
        {
            currentState = State.SeekCheese;
        }
    }

    void TakeCover()
    {
        // go home
        pathFinding.enabled = true;
        pathFinding.target = home.gameObject;

        // cambiar al estado oculto cuando llego al objetivo
        if (Vector3.Distance(transform.position, pathFinding.target.transform.position) < 0.6f)
        {
            currentState = State.Hidden;
            pathFinding.enabled = false; // Desactiva el PathFinding
        }
    }

    void SeekCheese()
    {
        int SeekCheeseIndex = Random.Range(0, 2);
        
        if (cheeses.Length > 0)
        {
            // actualiza el target
            pathFinding.target = cheeses[SeekCheeseIndex].gameObject;
        }

        // habilita pathfinding
        pathFinding.enabled = true;



        // ir al estado, comoer queso
        if (Vector3.Distance(transform.position, cheeses[SeekCheeseIndex].position) < 0.6f)
        {
            // SeekCheeseIndex = (SeekCheeseIndex + 1) % cheeses.Length;
            currentState = State.EatCheese;
            pathFinding.enabled = false; // Desactiva el PathFinding
        }

        // si alguno no está quieto, ir ocultarme (alarma)
        if (patrol_cars[0].currentState.ToString() != "Idle" || patrol_cars[1].currentState.ToString() != "Idle")
        {
            pathFinding.enabled = false;
            currentState = State.TakeCover;
        }

    }

    void EatCheese()
    {
        pathFinding.character.velocity = Vector3.zero;
        pathFinding.character.steering.linear = Vector3.zero;

        // si alguno no está quieto, ir ocultarme (alarma)
        if (patrol_cars[0].currentState.ToString() != "Idle" || patrol_cars[1].currentState.ToString() != "Idle")
        {
            pathFinding.enabled = false;
            currentState = State.TakeCover;
        }
    }
}
