using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Graph graph;
    List<Node> path = new List<Node>();
    List<Node> path2 = new List<Node>();
    Node start;
    Node end;
    Node middle;
    public GameObject target;
    public GameObject obstacle = null;
    private float slowRadius = 3f;
    private float timeToTarget = 1.0f;
    public agent character;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph();
        graph.getTriangles();
        graph.createConnections();
    }

    // Update is called once per frame
    void Update()
    {

        // draw polygon
        foreach (Node node in graph.nodes.Values)
        {
            if (graph.PointInTriangle(target.transform.position, node.vertices))
            {
                end = node;
            }
            if (graph.PointInTriangle(character.transform.position, node.vertices))
            {
                start = node;
            }
            // get obtacle point
            if (obstacle != null)
            {

                if (graph.PointInTriangle(obstacle.transform.position, node.vertices))
                {
                    middle = node;
                }
            }
            node.DrawTriangle();
        }

        // draw graph
        // foreach (Connection cone in graph.connections)
        // {
        //     cone.DrawConnection();
        // }


        // found path
        path = graph.AStar(start, end, middle);
        path2 = graph.AStar(start, end, null);

        // draw path
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].center, path[i + 1].center, Color.green);
        }
        // draw path2
        for (int i = 0; i < path2.Count - 1; i++)
        {
            Debug.DrawLine(path2[i].center, path2[i + 1].center, Color.red);
        }

        // following path
        for (int i = 0; i < path.Count; i++)
        {

            // estamos en el ultimo nodo
            if (graph.PointInTriangle(character.transform.position, path[path.Count - 1].vertices))
            {

                Vector3 direction = target.transform.position - character.transform.position;
                float distance = direction.magnitude;

                float targetSpeed;
                if (distance > slowRadius)
                {
                    targetSpeed = character.maxSpeed;
                }
                else
                {
                    targetSpeed = character.maxSpeed * distance / slowRadius;
                }

                Vector3 targetVelocity = direction;
                targetVelocity.Normalize();
                targetVelocity *= targetSpeed;

                character.steering.linear = targetVelocity - character.velocity;
                character.steering.linear /= timeToTarget;

                if (character.steering.linear.magnitude > character.maxAcceleration)
                {
                    character.steering.linear.Normalize();
                    character.steering.linear *= character.maxAcceleration;
                }

            }

            // Si estamos en el último nodo, no intentamos acceder al siguiente
            if (i < path.Count - 1)
            {
                Debug.DrawLine(path[i].center, path[i + 1].center, Color.green);
                // Debug.DrawLine(path2[i].center, path2[i + 1].center, Color.red);
            }

            // estamos en un nodo del camino
            if (i < path.Count - 1 && graph.PointInTriangle(character.transform.position, path[i].vertices))
            {

                character.steering.linear = path[i + 1].center - transform.position;

                character.steering.linear.Normalize();

                character.steering.linear *= character.maxAcceleration;

            }
        }

    }
}
