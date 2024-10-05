using UnityEngine;

// Clase que representa el comportamiento cinemático del objeto
public class Kinematic : MonoBehaviour
{
    public Vector2 position;      // Posición en el mundo
    public Vector2 velocity;      // Velocidad actual
    public float orientation;     // Orientación (en radianes)
    public float rotation;        // Velocidad de rotación (angular)

    public float maxSpeed = 5f;   // Velocidad máxima permitida
    public float maxRotation = 360f; // Rotación máxima permitida (grados por segundo)

    // Método para actualizar la cinemática del objeto basado en las fuerzas aplicadas
    public void UpdateKinematic(SteeringOutput steering, float time)
    {
        // Actualizar posición y orientación usando la velocidad y rotación actuales
        position += velocity * time;
        orientation += rotation * time;

        // Limitar la orientación entre 0 y 360 grados
        orientation = Mathf.Repeat(orientation, 360f);

        // Actualizar la velocidad lineal y la rotación angular con base en las fuerzas (steering)
        velocity += steering.linear * time;
        rotation += steering.angular * time;

        // Limitar la velocidad y la rotación a sus valores máximos
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);

        // Aplicar la nueva posición y orientación al objeto en Unity
        transform.position = new Vector3(position.x, position.y, 0); // Posición en 2D
        transform.rotation = Quaternion.Euler(0, 0, orientation);    // Orientación en 2D (usando z)
    }

    void Update() {
        // Crear las fuerzas (steering) que afectan al objeto
        SteeringOutput steering = new SteeringOutput();
        steering.linear = new Vector2(1f, 0f);  // Una fuerza lineal constante en el eje X
        steering.angular = 10f;                 // Una rotación constante (grados por segundo)

        // Llamar al método para actualizar el movimiento cinemático
        GetComponent<Kinematic>().UpdateKinematic(steering, Time.deltaTime);
    }
}
