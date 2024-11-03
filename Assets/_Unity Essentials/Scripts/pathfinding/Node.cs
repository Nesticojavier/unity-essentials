using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int id;

    public Vector3[] vertices = new Vector3[3];

    public Vector3 center;

    public Node(int id , Vector3[] vertices){
        this.id = id;
        this.vertices = vertices;
        this.center = calculateCenter();
    }

    public Vector3 calculateCenter(){
        float x = (vertices[0].x+vertices[1].x+vertices[2].x)/3;
        float y = (vertices[0].y+vertices[1].y+vertices[2].y)/3;
        return new Vector3(x,y,0f);
    }
    
    public void DrawTriangle(){
        Debug.DrawLine(vertices[0],vertices[1],Color.green);
        Debug.DrawLine(vertices[1],vertices[2],Color.green);
        Debug.DrawLine(vertices[2],vertices[0],Color.green);

    }
}
