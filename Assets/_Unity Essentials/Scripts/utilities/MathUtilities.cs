using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtilities
{
    /* 
     Funcion que recibe el angulo de rotacion (en grados)
     para mapear dicho angulo al intervalo [pi, -pi].

     Carcula en radianes

     Retorna el resultado en grados
  */
    public static float mapToRange(float degAngle)
    {
        // convertir a radianes
        float radAngle = degAngle * Mathf.Deg2Rad;

        // realizar calculos
        float result = (radAngle + Mathf.PI) % (2 * Mathf.PI) - Mathf.PI;

        // retornar el resultado en grados
        return result * Mathf.Rad2Deg;
    }
}
