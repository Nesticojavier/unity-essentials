using UnityEngine;

public class KinematicArrive : MonoBehaviour
{
    public Transform target;  // El objetivo al que queremos movernos
    public float maxSpeed = 5f;  // Velocidad máxima
    public float radius = 2f;  // Radio de satisfacción
    public float timeToTarget = 0.25f;  // Tiempo para llegar al objetivo
    
    private Rigidbody2D rb2D;  // El componente Rigidbody2D del personaje

    // Start se llama al inicio
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();  // Obtener el componente Rigidbody2D
    }

    // Update se llama en cada frame
    void Update()
    {
        // Llamar al método de KinematicArrive para obtener la nueva velocidad
        Vector2 steering = GetSteering();

        // Aplicar la velocidad al Rigidbody2D
        if (steering != Vector2.zero)
        {
            rb2D.velocity = steering;

            // Opcional: Rotar el personaje para que apunte en la dirección de la velocidad
            float angle = Mathf.Atan2(steering.y, steering.x) * Mathf.Rad2Deg;
            rb2D.rotation = angle;
        }
        else
        {
            // Si no hay steering, detenemos el objeto
            rb2D.velocity = Vector2.zero;
        }
    }

    // Método que implementa el Kinematic Arrive
    Vector2 GetSteering()
    {
        // Obtener la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position);
        float distance = direction.magnitude;

        // Si estamos dentro del radio de satisfacción, no hacemos steering
        if (distance < radius)
        {
            return Vector2.zero;  // Detener el movimiento
        }

        // Queremos llegar al objetivo en timeToTarget segundos, así que calculamos la velocidad
        Vector2 velocity = direction / timeToTarget;

        // Limitar la velocidad si excede la velocidad máxima
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        return velocity;  // Devolver la velocidad ajustada
    }
}
