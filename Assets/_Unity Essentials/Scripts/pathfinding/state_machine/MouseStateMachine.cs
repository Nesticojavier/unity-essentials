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
    public StateMachinePatrol1 patrol_cars0;
    public StateMachinePatrol1[] patrol_cars;
    public StateMachinePatrol1 patrol_cars1;
    // public Transform home;
    public Transform[] coverPoints;
    public State currentState = State.Hidden;
    public PathFinding pathFinding;

    private System.Random randomGenerator;
    private int SeekCheeseIndex = 0;

    void Start()
    {
        pathFinding.enabled = false;
        randomGenerator = new System.Random();
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
    private float hiddenCooldown = 2f; // Tiempo mínimo antes de reevaluar
    private float hiddenTimer;        // Temporizador para Hidden

    void Hidden()
    {
        if (pathFinding.character != null)
        {
            pathFinding.character.velocity = Vector3.zero;
            pathFinding.character.steering.linear = Vector3.zero;
        }

        pathFinding.target = null;

        // Actualizar el temporizador
        hiddenTimer += Time.deltaTime;

        // Solo reevaluar si el temporizador supera el tiempo de cooldown
        if (hiddenTimer < hiddenCooldown)
            return;

        // Verificar si todos los patrulleros están en Idle
        bool allIdle = true;
        foreach (var patrol in patrol_cars)
        {
            if (patrol.currentState.ToString() != "Idle")
            {
                allIdle = false;
                break;
            }
        }

        if (allIdle)
        {
            currentState = State.SeekCheese;
            hiddenTimer = 0f; // Reiniciar el temporizador
        }
        else
        {
            foreach (var patrol in patrol_cars)
            {
                // Si alguno pasa a Patrol, cambiar a TakeCover
                if (patrol.currentState.ToString() == "Patrol")
                {
                    currentState = State.TakeCover;
                    hiddenTimer = 0f; // Reiniciar el temporizador
                    return;
                }
            }
        }
    }

    void TakeCover()
    {
        // Activar el PathFinding
        pathFinding.enabled = true;

        // Verificar el estado de los patrulleros y determinar qué propiedad evaluar
        bool considerPatrol1 = patrol_cars[0].currentState.ToString() == "Patrol";
        bool considerPatrol2 = patrol_cars[1].currentState.ToString() == "Patrol";

        CoverPoint closestCover = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform coverPoint in coverPoints)
        {
            CoverPoint coverProps = coverPoint.GetComponent<CoverPoint>();
            if (coverProps == null) continue;

            // Evaluar la propiedad adecuada
            bool isHidden = (!considerPatrol1 || !considerPatrol2) && ((considerPatrol1 && coverProps.hiddenForPatrol1) ||
                            (considerPatrol2 && coverProps.hiddenForPatrol2));

            isHidden = isHidden || (considerPatrol1 && considerPatrol2 && coverProps.hiddenForPatrol1 && coverProps.hiddenForPatrol2);


            if (!isHidden) continue;

            // Calcular la distancia al personaje
            float distance = Vector3.Distance(transform.position, coverPoint.position);

            // Buscar el punto más cercano
            if (distance < closestDistance)
            {
                closestCover = coverProps;
                closestDistance = distance;
            }
        }

        // Asignar el punto de cobertura más cercano como objetivo
        if (closestCover != null)
        {
            pathFinding.target = closestCover.gameObject;
            Debug.Log($"Nuevo objetivo: {closestCover}");
        }
        else
        {
            Debug.LogWarning("No se encontró un punto de cobertura adecuado.");
        }

        // Cambiar al estado oculto si llegamos al objetivo
        if (pathFinding.target != null &&
            Vector3.Distance(transform.position, pathFinding.target.transform.position) < 0.6f)
        {
            currentState = State.Hidden;
            pathFinding.enabled = false; // Desactiva el PathFinding
        }
    }


    void SeekCheese()
    {
        /*    // Si aún no se ha asignado un objetivo de queso, lo hacemos aquí
           if (pathFinding.target == null && cheeses.Length > 0)
           {
               SeekCheeseIndex = randomGenerator.Next(0, cheeses.Length);
               pathFinding.target = cheeses[SeekCheeseIndex].gameObject;
               pathFinding.enabled = true;
           }



           // ir al estado, comoer queso
           if (Vector3.Distance(transform.position, cheeses[SeekCheeseIndex].position) < 1f)
           {
               currentState = State.EatCheese;
               pathFinding.enabled = false;
           }

           // si alguno no está quieto, ir ocultarme (alarma)
           if (patrol_cars[0].currentState.ToString() != "Idle" || patrol_cars[1].currentState.ToString() != "Idle")
           {
               pathFinding.enabled = false;
               currentState = State.TakeCover;
           } */

        pathFinding.target = cheeses[SeekCheeseIndex].gameObject;
        pathFinding.enabled = true;
        if (Vector3.Distance(transform.position, cheeses[SeekCheeseIndex].position) < 2f)
        {
            SeekCheeseIndex = (SeekCheeseIndex + 1) % cheeses.Length;
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
        // pathFinding.character.velocity = Vector3.zero;
        // pathFinding.character.steering.linear = Vector3.zero;




        // si alguno no está quieto, ir ocultarme (alarma)
        if (patrol_cars[0].currentState.ToString() != "Idle" || patrol_cars[1].currentState.ToString() != "Idle")
        {
            pathFinding.enabled = false;
            currentState = State.TakeCover;
        }
    }
}
