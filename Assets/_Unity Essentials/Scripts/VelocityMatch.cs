using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatch : MonoBehaviour
{

    public Transform target;
    public float maxAcceleration = 20f;
    public float maxSpeed = 20f;
    private float timeToTarget = 0.05f;
    private Vector2 velocity;      // Velocidad actual  
    private float rotation;        // Velocidad angular actual
    private Rigidbody2D rb2D;
    private Rigidbody2D targetRB;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        targetRB = target.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // Actualizar la posición y orientación
        Vector2 position = rb2D.position;
        // float orientation = rb2D.rotation;

        position += velocity * time;
        // orientation += rotation * time;

        // Actualizar la velocidad y rotación en base a las fuerzas de steering
        velocity += steering.linear * time;
        // rotation += steering.angular * time;

        // limitar velocidad
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Aplicamos los cambios al Rigidbody2D
        rb2D.MovePosition(position);
        // rb2D.MoveRotation(orientation);
    }

    SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = targetRB.velocity - rb2D.velocity;
        result.linear = result.linear / timeToTarget;

        // Check if the acceleration is too fast.
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear = result.linear.normalized;
            result.linear *= maxAcceleration;
        }

        result.angular = 0f;
        return result;
    }
}
