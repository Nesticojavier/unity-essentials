using UnityEngine;

public class agent : MonoBehaviour
{

    public float rotation;

    public Vector3 velocity;

    public Steering steering;

    public float maxSpeed = 2f;

    public float maxAcceleration = 2f;
    public float maxAngularVelocity = 100f;


    // Start is called before the first frame update
    void Start()
    {
        steering = new Steering();

    }

    // Update is called once per frame
    void Update()
    {
        maxAcceleration = maxSpeed * 100;
        // Update position and orientation
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + rotation * Time.deltaTime * Mathf.Rad2Deg);

        // Update velocity and rotation
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxAngularVelocity * Time.deltaTime);
        }
    }
}
