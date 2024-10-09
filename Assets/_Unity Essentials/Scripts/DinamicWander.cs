using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicWander : Face
{
    public float wanderOffset = 10;
    public float wanderRadius = 5;
    public float wanderRate = 5;
    public float maxAcceleration = 1;
    public float wanderOrientation;

    // utils variables
    private float rotation;
    private Vector2 velocity;

    void Start()
    {
        notFace = true;
        notAlign = true;
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // update rotation
        rotation = rb2D.rotation + steering.angular * time;
        rb2D.MoveRotation(rotation);

        // update velocity
        velocity = rb2D.velocity + steering.linear * time;
        if (velocity.magnitude > maxAcceleration)
        {
            velocity = velocity.normalized * maxAcceleration;
        }
        rb2D.velocity = velocity;

    }

    public new SteeringOutput getSteering()
    {

        SteeringOutput result = new SteeringOutput();

        wanderOrientation += RandomBinomial() * wanderRate;

        float targetOrientation = wanderOrientation + rb2D.rotation;

        newTarget = rb2D.position + wanderOffset * OrientationToVector(rb2D.rotation);

        newTarget += wanderRadius * OrientationToVector(targetOrientation);

        result = ((Face)this).getSteering();

        result.linear = maxAcceleration * OrientationToVector(rb2D.rotation);

        return result;
    }

    Vector2 OrientationToVector(float orientation)
    {
        // Convertir los grados a radianes
        float radians = orientation * Mathf.Deg2Rad;

        // Usar trigonometría para obtener la dirección
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    float RandomBinomial()
    {
        // return Random.Range(-1f, 1f);
        System.Random random = new System.Random();
        return (float)(random.NextDouble() - random.NextDouble());
    }
}