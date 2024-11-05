using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetManager : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject house;
    private GameObject currentTarget;
    private PathFinding pathFinding;

    void Start()
    {
        pathFinding = GetComponent<PathFinding>();
        currentTarget = target1;
        pathFinding.target = currentTarget;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, pathFinding.target.transform.position);
        
        // Cambia de target cuando llega al actual
        if (distance < 0.5f) // distancia mínima para considerar que ha llegado
        {
            pathFinding.target = pathFinding.target == target1 ? target2 : target1;
        }
    }
}
