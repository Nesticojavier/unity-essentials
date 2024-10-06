using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicArrive : MonoBehaviour
{

    public Transform target;
    public float maxAcceleration = 2f;
    public float maxSpeed = 10f;
    private Vector2 velocity;      // Velocidad actual  
    private float rotation;        // Velocidad angular actual
    private Rigidbody2D rb2D;

    // kinematic arrive
    public float targetRadius = 2f;
    public float slowRadius = 7f;
    public float timeToTarget = 0.1f;


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
        if (steering.linear == Vector2.zero)
        {
            velocity = Vector2.zero;
            rb2D.velocity = Vector2.zero;
            return;  
        }

        // Actualizar la posición y orientación
        Vector2 position = rb2D.position;
        float orientation = rb2D.rotation;

        position += velocity * time;
        orientation += rotation * time;

        // Actualizar la velocidad y rotación en base a las fuerzas de steering
        velocity += steering.linear * time;
        rotation += steering.angular * time;

        // limitar velocidad
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Aplicamos los cambios al Rigidbody2D
        rb2D.MovePosition(position);
        rb2D.MoveRotation(orientation);
    }

    SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        float targetSpeed;
        Vector2 targetVelocity;

        // obtenemos direccion
        Vector2 direction = target.position - transform.position;

        // obtenemos distancia
        float distance = direction.magnitude;

        // veirficamos que la distancia este en el límite
        if (distance < targetRadius)
        {
            return new SteeringOutput { linear = Vector2.zero, angular = 0f };
        }


        // Calculo de la velocidad objetivo
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        // target velocity combina velocidad objetiv y direccion
        targetVelocity = direction;
        targetVelocity = targetVelocity.normalized;
        targetVelocity *= targetSpeed;


        // Aceleracion
        result.linear = targetVelocity - velocity;
        result.linear = result.linear / timeToTarget;

        // verificar si la aceleracion es muy rapida
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear = result.linear.normalized;
            result.linear *= maxAcceleration;
        }

        // set angular value
        result.angular = 0f;

        // finish
        return result;
    }
}
