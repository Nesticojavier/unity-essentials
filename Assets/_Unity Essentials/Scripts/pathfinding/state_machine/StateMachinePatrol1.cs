using UnityEngine;

public class StateMachinePatrol1 : StateMachinePatrol
{
    public Transform otherGuardTransform;
    private StateMachinePatrol1 otherStateMachine;
    public Transform[] patrolPoints; // Puntos de patrullaje
    public Transform cuartel;
    private int patrolIndex = 0;

    public override void Initialize()
    {
        if (otherGuardTransform != null)
        {
            otherStateMachine = otherGuardTransform.GetComponent<StateMachinePatrol1>();
        }

        pathFinding.enabled = false;
        currentEnergy = maxEnergy;
    }

    public override void Idle()
    {
        currentEnergy += energyRechargeRate * Time.deltaTime;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);

        if (pathFinding.character != null)
        {
            pathFinding.character.velocity = Vector3.zero;
            pathFinding.character.steering.linear = Vector3.zero;
        }

        if (currentEnergy >= maxEnergy)
        {
            currentState = State.Patrol;
        }
    }

    public override void Seek()
    {
        pathFinding.enabled = true;
        pathFinding.target = cuartel.gameObject;

        if (Vector3.Distance(transform.position, pathFinding.target.transform.position) < 0.6f)
        {
            currentState = State.Idle;
            pathFinding.enabled = false;
        }
    }

    public override void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            pathFinding.target = patrolPoints[patrolIndex].gameObject;
            pathFinding.enabled = true;

            if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < 2f)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            }

            currentEnergy -= energyDrainRate * Time.deltaTime;

            if (currentEnergy <= 0)
            {
                pathFinding.enabled = false;
                currentState = State.Seek;
            }
        }
    }

    void Start()
    {
        Initialize();
    }
}
