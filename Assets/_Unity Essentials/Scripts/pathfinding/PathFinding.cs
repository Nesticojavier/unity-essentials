using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{   
    public Graph graph;
    List<Node> path = new List<Node>();
    Node start;
    Node end;
    public GameObject target;
    private float targetRadius = 0.5f, slowRadius = 3f, timeToTarget = 0.5f;
    public agent character;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph();
        if(character.name == "amin"){
            graph.getTriangles(true);
        } else{
            graph.getTriangles(false);
        }
        graph.createConnections();    
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Node node in graph.nodes.Values){
            if(graph.PointInTriangle(target.transform.position, node.vertices)){
                end = node;
            }
            if(graph.PointInTriangle(character.transform.position,node.vertices)){
                start = node;
            }
            node.DrawTriangle();
        }

        foreach(Connection cone in graph.connections){
            cone.DrawConnection();
        }


        // found path
        path = graph.AStar(start,end);

        // draw path
        for(int i = 0; i < path.Count - 1 ; i++){
            Debug.DrawLine(path[i].center,path[i+1].center,Color.red);
        }

        for(int i = 0; i< path.Count ; i++){
            if(graph.PointInTriangle(character.transform.position,path[path.Count - 1].vertices)){
                
                Vector3 direction = target.transform.position - character.transform.position;
                float distance = direction.magnitude;

                if (distance > targetRadius){
                    float targetSpeed; 
                    if (distance > slowRadius){
                        targetSpeed = character.maxSpeed;
                    } else {
                        targetSpeed = character.maxSpeed * distance / slowRadius;
                    }

                    Vector3 targetVelocity = direction;
                    targetVelocity.Normalize();
                    targetVelocity *= targetSpeed;

                    character.steering.linear = targetVelocity - character.velocity;
                    character.steering.linear /= timeToTarget;

                    if (character.steering.linear.magnitude > character.maxAcceleration){
                        character.steering.linear.Normalize();
                        character.steering.linear *= character.maxAcceleration;
                    }
                } else {
                    character.steering.linear = Vector3.zero;
                    character.velocity =Vector3.zero;
                }

            }

            if(graph.PointInTriangle(character.transform.position,path[i].vertices)){
                
                character.steering.linear = path[i + 1].center - transform.position;
                
                character.steering.linear.Normalize();

                character.steering.linear *= character.maxAcceleration;

            }
        }
        
    }
}
