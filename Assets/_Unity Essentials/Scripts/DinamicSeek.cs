using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicSeek : MonoBehaviour
{
    public bool flee = false;

    public float fleeRadius = 10f;
    public float slowRadius = 7f;
    public Transform target;
    public float maxAcceleration = 30f;
    public float maxSpeed = 20f;
    protected Vector2 velocity;      // Velocidad actual  
    protected float rotation;        // Velocidad angular actual
    protected Rigidbody2D rb2D;

    // variable usada para sumarle la prediccion del algoritmo porsue
    protected Vector3 future = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;


        if (steering.linear == Vector2.zero)
        {
            velocity = Vector2.zero;
            rb2D.velocity = Vector2.zero;
            return;
        }

        // Actualizar la posición y orientación
        Vector2 position = rb2D.position;
        // float orientation = rb2D.rotation;

        // orientation += rotation * time;

        // Actualizar la velocidad y rotación en base a las fuerzas de steering
        velocity = rb2D.velocity + steering.linear * time;
        position += velocity * time;

        // rotation += steering.angular * time; 

        // limitar velocidad
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Aplicamos los cambios al Rigidbody2D
        rb2D.MovePosition(position);
        rb2D.velocity = velocity;
    }

    protected SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        if (!flee)
        {
            result.linear = (target.position + future) - transform.position;
        }

        // solo en caso de flee
        else
        {
            result.linear = transform.position - (target.position + future);
            Vector2 direction = transform.position - (target.position + future);

            float distance = result.linear.magnitude;
            float targetSpeed;
            Vector2 targetVelocity;
            if (distance > fleeRadius)
            {
                result.linear = Vector2.zero;
                return result;
            }

            // Calculo de la velocidad objetivo
            if (distance < slowRadius)
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
            result.linear = result.linear / 0.1f;

            // verificar si la aceleracion es muy rapida
            if (result.linear.magnitude > maxAcceleration)
            {
                result.linear = result.linear.normalized;
                result.linear *= maxAcceleration;
            }
            return result;
        }

        result.linear = result.linear.normalized;
        result.linear *= maxAcceleration;
        result.angular = 0f;
        return result;
    }
}
