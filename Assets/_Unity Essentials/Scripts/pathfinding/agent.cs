using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{

    public float rotation;

    public Vector3 velocity;

    public Steering steering;

    public float maxSpeed = 0.1f;

    public float maxAcceleration = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
        steering = new Steering();

    }

    // Update is called once per frame
    void Update()
    {   
        maxAcceleration = 2 * maxSpeed;
        // Update position and orientation
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z + rotation * Time.deltaTime * Mathf.Rad2Deg);

        // Update velocity and rotation
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if (velocity.magnitude > maxSpeed){
            velocity.Normalize();
            velocity *= maxSpeed;
        }        
        

    }

    public Quaternion newOrientation(Quaternion current, Vector3 velocity){
        // Make sure we have a velocity.
        if (velocity.magnitude > 0){
            // Calculate orientation from the velocity.
            return Quaternion.Euler(current.x,current.y,Mathf.Atan2(-velocity.x, velocity.y)*Mathf.Rad2Deg);
        } else {
            return current;
        }
    }

    public Vector3 asVector(float orientation){
        Vector3 result = Vector3.zero;
        result.x = - Mathf.Sin(orientation);
        result.y = Mathf.Cos(orientation);
        result.Normalize();
        return result;

    }


}
