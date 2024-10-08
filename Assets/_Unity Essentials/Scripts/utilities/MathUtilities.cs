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
    public static float mapToRange(float angle)
    {
        angle = (angle + 180) % 360;
        if (angle < 0) angle += 360;
        return angle - 180;
    }
}
