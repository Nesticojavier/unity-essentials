using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek : MonoBehaviour
{
    public Transform target;  // El objetivo hacia el cual moverse
    public float maxSpeed = 5f;  // Velocidad máxima de movimiento

    private Rigidbody2D rb2D;  // El componente Rigidbody2D del objeto

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        KinematicSeekMovement();
    }

    void KinematicSeekMovement()
    {
        // Obtener la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;

        Debug.Log("Esta es el vector direccion");
        Debug.Log(direction);
        Debug.Log("Esta es la magnitud");
        Debug.Log(direction.magnitude);


        // Establecer la nueva velocidad, apuntando hacia el objetivo
        Vector2 velocity = direction * maxSpeed;

        // Asignar la velocidad al Rigidbody2D
        rb2D.velocity = velocity;

        // Opcional: Rotar el objeto para que apunte en la dirección del movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb2D.rotation = angle;
    }
}
