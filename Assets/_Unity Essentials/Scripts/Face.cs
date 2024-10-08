using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face: Align
{
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    void Start(){
        notAlign = true;
        rb2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // 6. Llamar a getSteering de Face para que el personaje siempre mire al objetivo
        SteeringOutput steering = getSteering();
        float time = Time.deltaTime;

        // Si no hay más rotación que hacer, simplemente mantener la orientación actual
        if (steering.angular == 0f)
        {
            return;
        }

        // Aplicar la rotación calculada al Rigidbody2D
        float orientation = rb2D.rotation + steering.angular * time;
        rb2D.MoveRotation(orientation);
    }


public new SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();


        // 1. Calcular la dirección hacia el objetivo
        Vector2 direction = target.position - transform.position;

        // 2. Comprobar si la dirección es cero, si es así no hay que cambiar la orientación
        if (direction.magnitude == 0)
        {
            return result; // No hay cambio de rotación si no hay dirección.
        }

        // 3. Crear un objetivo explícito para Align
        orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("direction: " + direction);
        Debug.Log("orientation: " + orientation);

        // 4. Establecer la orientación del target en Align
        // faceTarget.eulerAngles = new Vector3(0, 0, targetOrientation);

        // 5. Delegar la rotación a la función Align (usando la implementación en Align)
        return base.getSteering(); // Llama a getSteering() de Align
    }


}