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

        // Rotación con las teclas 'E' (sentido de las agujas del reloj) y 'Q' (sentido contrario)
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
