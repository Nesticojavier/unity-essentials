using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : DinamicSeek
{

    public List<Transform> targets;
    private Path path;

    public float pathOffset = 1;

    private float currentParam;

    // Start is called before the first frame update
    void Start()
    {
        pathFollowing = true;
        rb2D = GetComponent<Rigidbody2D>();
        path = new Path(targets);
    }

    // Update is called once per frame
    new void Update()
    {

        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;
        Vector2 position = rb2D.position;
        velocity = rb2D.velocity + steering.linear * time;
        position += velocity * time;

        // limitar velocidad
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        
        rb2D.MovePosition(position);
        rb2D.velocity = velocity;
    }

    public new SteeringOutput getSteering()
    {

        currentParam = path.getParam(
          rb2D.position,
          4f);

        float targetParam = currentParam + pathOffset;

        if (targetParam > 17)
        {
            targetParam -= 18;
        }

        newTarget = path.getPosition(targetParam);

        return base.getSteering();
    }


}
