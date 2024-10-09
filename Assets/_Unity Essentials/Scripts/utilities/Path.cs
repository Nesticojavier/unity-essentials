using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que representa las fuerzas de aceleración lineal y angular
public class Path
{
    private List<Transform> points;  // Array de Transforms

    public Path(List<Transform> transforms)
    {
        points = transforms;
    }

    // Método de ejemplo que usa el array de Transforms
    public float getParam(Vector2 position, float lastParam)
    {
        // Si no hay puntos en el array, retornamos -1 o algún valor inválido
        if (points == null || points.Count == 0)
        {
            Debug.LogWarning("No points available in the path.");
            return -1f;
        }

        float closestDistance = Mathf.Infinity;  // Inicializamos con un valor muy grande
        float closestParam = 0f;  // El valor param del punto más cercano
        int closestIndex = -1;    // El índice del punto más cercano

        // Recorremos todos los puntos
        for (int i = 0; i < points.Count; i++)
        {
            // Convertimos la posición del punto a Vector2 para compararlo con 'position'
            Vector2 pointPosition = new Vector2(points[i].position.x, points[i].position.y);

            // Calculamos la distancia entre el punto actual y 'position'
            float distance = Vector2.Distance(position, pointPosition);

            // Si encontramos una distancia más corta, la actualizamos
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestParam = i;  // Usamos el índice como parámetro por ahora
                closestIndex = i;
            }
        }

        // Para ahora, retornamos el índice o param más cercano
        // Debug.Log("Punto más cercano: " + closestIndex + " con distancia: " + closestDistance);
        return closestParam;
    }

    // Método que retorna la posición de un punto dado su índice
    public Vector2 getPosition(float param)
    {
        int paramInt = (int)param;
        Debug.Log("Este mi 7 final: " + points[paramInt] + " " + points[paramInt].position);
        // Verificamos que el índice sea válido
        if (paramInt >= 0 && paramInt < points.Count)
        {
            return new Vector2(points[paramInt].position.x, points[paramInt].position.y); 
        }
        else
        {
            // Debug.LogWarning("Invalid parameter index: " + param);
            return Vector2.zero; 
        }
    }
}


