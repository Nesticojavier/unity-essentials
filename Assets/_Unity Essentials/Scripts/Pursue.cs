using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : DinamicSeek
{
    public float maxPrediction;
    private Rigidbody2D targetRB;


    void Start()
    {
        targetRB = target.GetComponent<Rigidbody2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // Actualizar la posición y orientación
        // Vector2 position = rb2D.position;
        // float orientation = rb2D.rotation;

        // position += velocity * time;  
        // orientation += rotation * time;

        // // Actualizar la velocidad y rotación en base a las fuerzas de steering
        // velocity += steering.linear * time;  
        // rotation += steering.angular * time; 
        Vector2 position = rb2D.position;

        velocity = rb2D.velocity + steering.linear * time;
        position += velocity * time;

        // limitar velocidad
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Aplicamos los cambios al Rigidbody2D
        rb2D.MovePosition(position);
        rb2D.velocity = velocity;
    }


    public new SteeringOutput getSteering()
    {
        Vector3 direction = target.position - transform.position;

        float distance = direction.magnitude;

        // float speed = character.velocity.magnitude;
        float speed = rb2D.velocity.magnitude;

        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        future = targetRB.velocity * prediction;

        return base.getSteering();

    }


}