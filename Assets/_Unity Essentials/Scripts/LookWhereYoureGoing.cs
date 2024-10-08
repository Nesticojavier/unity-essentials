using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYoureGoing : Align
{
    void Start()
    {
        maxAngularAcceleration = 500;
        maxRotation = 500;
        notAlign = true;
        rb2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // 6. Llamar a getSteering de Face para que el personaje siempre mire al objetivo
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // Si no hay más rotación que hacer, simplemente mantener la orientación actual
        if (steering.angular == 0f)
        {
            return;
        }

        // Aplicar la rotación calculada al Rigidbody2D
        float orientation = rb2D.rotation + steering.angular * time;
        rb2D.MoveRotation(orientation);
    }

    public new SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();


        // 1. Calcular la dirección hacia el objetivo
        Vector2 velocity = rb2D.velocity;

        Debug.Log("velocity: " + velocity);

        if (velocity.magnitude == 0)
        {
            return result; // No hay cambio de rotación si no se mueve.
        }

        orientation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        return base.getSteering(); // Llama a getSteering() de Align
    }


}