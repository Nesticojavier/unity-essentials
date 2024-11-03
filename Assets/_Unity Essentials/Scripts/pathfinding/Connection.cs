using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{

    public Node fromNode;

    public Node toNode;

    public float cost;

    public Connection(Node fromNode, Node toNode){
        this.fromNode = fromNode;
        this.toNode = toNode;
    }

    public Node getFromNode(){
        Node node = fromNode;
        return node;
    }

    public float calculateCost(){
        Vector3 X = fromNode.center;
        Vector3 Y = toNode.center;
        return Mathf.Sqrt((Y.x - X.x) * (Y.x - X.x) + (Y.y - X.y) * (Y.y - X.y));
    }


    public void DrawConnection(){
        Debug.DrawLine(fromNode.center,toNode.center,Color.blue);
    }
}
