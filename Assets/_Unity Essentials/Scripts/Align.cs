using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : MonoBehaviour
{

    public Transform target;
    public float maxAngularAcceleration = 5;
    public float maxRotation = 5;

    // The radius for arriving at the target.
    public float targetRadius = 2;

    // The radius for beginning to slow down.
    public float slowRadius = 50;

    // The time over which to achieve target speed.
    float timeToTarget = 0.1f;

    protected float orientation;
    protected bool notAlign = false;

    // utils variables
    private float rotation;
    protected Rigidbody2D rb2D;
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

        /*         float orientation = rb2D.rotation + rotation * time;
                rotation += steering.angular * time;
                rb2D.MoveRotation(orientation); */
        // 
        // float orientation = rb2D.rotation + rotation * time;
        rotation = rb2D.rotation + steering.angular * time;
        rb2D.MoveRotation(rotation);


        // 
        /*       float orientation = rb2D.rotation + steering.angular * time;
              rb2D.MoveRotation(orientation); */
    }

    public SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // Get the naive direction to the target.
        float rotation;
        if (notAlign == false){
            rotation = -Mathf.DeltaAngle(target.eulerAngles.z, transform.eulerAngles.z) ;
        }  else {
            rotation = -Mathf.DeltaAngle(orientation, transform.eulerAngles.z) ;
        }  

        // Debug.Log("target.eulerAngles.z: " + target.eulerAngles.z);
        // Debug.Log(" transform.eulerAngles.z: " + transform.eulerAngles.z);
        // Debug.Log("rotation: " + rotation);

        // Map the result to the (-pi, pi) interval.
        // rotation = MathUtilities.mapToRange(rotation);
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
        result.angular = targetRotation - rb2D.angularVelocity;
        result.angular /= timeToTarget;

        //  Check if the acceleration is too great.
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }
        // Debug.Log("result.angular: " + result.angular);

        result.linear = Vector2.zero;
        return result;
    }
}