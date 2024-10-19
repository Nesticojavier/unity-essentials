using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotationSpeed = 100f; // Velocidad de rotación

    private Rigidbody2D rb;

    void Start()
    {
        // Obtener el componente Rigidbody2D del personaje
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // Movimiento con flechas del teclado
        float moveX = Input.GetAxis("Horizontal"); // Flechas izquierda y derecha
        float moveY = Input.GetAxis("Vertical");   // Flechas arriba y abajo

        // Aplicar el movimiento
        Vector2 movement = new Vector2(moveX, moveY) * moveSpeed;
        rb.velocity = movement;

        // Solo rotar hacia la dirección de movimiento si hay movimiento
        if (movement.sqrMagnitude > 0.01f)  // Verifica si el movimiento no es cercano a cero
        {
            // Calcular el ángulo hacia el cual rotar
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;  // Ajustar la rotación del Rigidbody2D hacia el ángulo calculado
        }

        // Rotación adicional con las teclas 'E' y 'Q'
        if (Input.GetKey(KeyCode.E))
        {
            // Rotar en el sentido de las agujas del reloj
            rb.rotation -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            // Rotar en sentido contrario a las agujas del reloj
            rb.rotation += rotationSpeed * Time.deltaTime;
        }
    }
}
