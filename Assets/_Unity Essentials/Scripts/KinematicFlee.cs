using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicFlee : MonoBehaviour
{
    public Transform target;  // El objetivo del cual huir
    public float maxSpeed = 5f;  // Velocidad máxima de movimiento
    public float fleeRadius = 10f;  // Radio de huida

    private Rigidbody2D rb2D;  // El componente Rigidbody2D del objeto

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        KinematicFleeMovement();
    }

    void KinematicFleeMovement()
    {
        // Obtener la dirección hacia el objetivo
        Vector2 direction = (transform.position - target.position).normalized;  // Dirección opuesta

        // Calcular la distancia entre el objeto y el objetivo
        float distance = Vector2.Distance(transform.position, target.position);

        Debug.Log("Distancia al objetivo: " + distance);

        // Si el objeto está dentro del radio de huida, huir
        if (distance < fleeRadius)
        {
            // Establecer la nueva velocidad, alejándose del objetivo
            Vector2 velocity = direction * maxSpeed;

            // Asignar la velocidad al Rigidbody2D
            rb2D.velocity = velocity;

            // Opcional: Rotar el objeto para que apunte en la dirección de movimiento
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb2D.rotation = angle;
        }
        else
        {
            // Si está fuera del radio de huida, detenerse
            rb2D.velocity = Vector2.zero;
        }
    }
}
