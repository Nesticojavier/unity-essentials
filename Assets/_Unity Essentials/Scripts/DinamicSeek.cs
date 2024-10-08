using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicSeek : MonoBehaviour
{

    public Transform target;
    public float maxAcceleration = 2f;
    public float maxSpeed = 10f;
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
    void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

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

    protected SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = (target.position + future) - transform.position;
        result.linear = result.linear.normalized;
        result.linear *= maxAcceleration;
        result.angular = 0f;
        return result;
    }
}
