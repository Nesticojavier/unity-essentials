using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRecord
{
    public Node node;

    public NodeRecord prev;

    public Connection connection;

    public float costSoFar;

    public float estimatedTotalCost;

    public NodeRecord(Node node, Connection connection, float costSoFar, float estimatedTotalCost){
        this.node = node;
        this.connection = connection;
        this.costSoFar = costSoFar;
        this.estimatedTotalCost = estimatedTotalCost;
    }

    public NodeRecord(){
    }
}
