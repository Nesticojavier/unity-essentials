using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour
{

    public Transform target;
    public float maxAngularAcceleration;
    public float maxRotation;

    // The radius for arriving at the target.
    public float targetRadius;

    // The radius for beginning to slow down.
    public float slowRadius;

    // The time over which to achieve target speed.
    float timeToTarget = 0.1f;

    private Rigidbody2D rb2D;

    // utils variables
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // Comprobamos si el personaje debería detenerse
        if (steering.angular == 0f)
        {
            rotation = transform.eulerAngles.z;
            rb2D.rotation = transform.eulerAngles.z;
            return;
        }

        float orientation = rb2D.rotation;
        orientation += rotation * time;
        rotation += steering.angular * time;
        rb2D.MoveRotation(orientation);
    }

    SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // Get the naive direction to the target.
        float rotation = target.eulerAngles.z - transform.eulerAngles.z;

        // Map the result to the (-pi, pi) interval.
        rotation = MathUtilities.mapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        // Check if we are there, return no steering.
        if (rotationSize < targetRadius)
        {
            return new SteeringOutput { linear = Vector2.zero, angular = 0f };
        }

        float targetRotation;
        //  If we are outside the slowRadius, then 
        //  use maximum rotation.
        if (rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }

        //  Otherwise calculate a scaled rotation.
        else
        {
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        // The final target rotation combines speed (already in the
        //  variable) and direction.
        targetRotation *= rotation / rotationSize;

        //  Acceleration tries to get to the target rotation.
        result.angular = targetRotation - transform.eulerAngles.z;
        result.angular = result.angular / timeToTarget;

        //  Check if the acceleration is too great.
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular = result.angular / angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        result.linear = Vector2.zero;
        return result;
    }
}
