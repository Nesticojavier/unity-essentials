using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{   
    public Dictionary<int,Node> nodes = new Dictionary<int,Node>();

    public List<Connection> connections = new List<Connection>();

    public Graph(){

    }

    public List<Connection> createConnections(){
        List<Connection> result = new List<Connection>();
        for(int i=0; i<nodes.Count;i++){
            for(int j=i;j<nodes.Count;j++){
                if (nodes.ContainsKey(i) && nodes.ContainsKey(j)){
                    if(nodesAside(nodes[i],nodes[j])){
                        addConection(nodes[i],nodes[j]);
                    }
                }
            }
        }

        return result;
    }

    public float heuristic(Node start, Node end){
        Vector3 X = start.center;
        Vector3 Y = end.center;
        return Mathf.Sqrt((Y.x - X.x) * (Y.x - X.x) + (Y.y - X.y) * (Y.y - X.y));
    }

    public bool nodesAside(Node node1, Node node2){

        for(int i=0; i< node1.vertices.Length ; i++){
            for(int j=0; j < node2.vertices.Length;j++){
				float x1=(node1.vertices[i%node1.vertices.Length].x + node1.vertices[(i+1)%node1.vertices.Length].x)/2;
				float y1=(node1.vertices[i%node1.vertices.Length].y + node1.vertices[(i+1)%node1.vertices.Length].y)/2;

				float x2=(node2.vertices[j%node2.vertices.Length].x + node2.vertices[(j+1)%node2.vertices.Length].x)/2;
				float y2=(node2.vertices[j%node2.vertices.Length].y + node2.vertices[(j+1)%node2.vertices.Length].y)/2;
							
				if(Mathf.Abs(x1-x2)<1f & Mathf.Abs(y1-y2)<1f){
					return true;
				}
            }
        }
        
        return false;
    }

    public void addConection(Node fromNode, Node toNode){

        foreach (Connection connection in connections){
            if((connection.fromNode.id == fromNode.id && connection.toNode.id == toNode.id) || (connection.toNode.id == fromNode.id && connection.fromNode.id == toNode.id)){
                return;
            }
        }
        connections.Add(new Connection(fromNode,toNode));
    }

    public NodeRecord smallestElement(Dictionary<Node,NodeRecord> open){
        NodeRecord smallest = open.First().Value;

        foreach (KeyValuePair<Node,NodeRecord> node in open){
            if(node.Value.estimatedTotalCost < smallest.estimatedTotalCost){
                smallest=node.Value;
            }
        }
        return smallest;
    }

    public void getTriangles(bool amin){
        int i = 0;

        foreach(GameObject cube in GameObject.FindGameObjectsWithTag("cuadrados") ){
            Vector3 pos = cube.transform.position;
            Vector3 scale = cube.transform.localScale;
            Vector3[] vertices = new Vector3[3];
            vertices[0] = new Vector3(pos.x - scale.x/2, pos.y + scale.y/2, 0);
            vertices[1] = new Vector3(pos.x - scale.x/2, pos.y - scale.y/2, 0);
            vertices[2] = new Vector3(pos.x + scale.x/2, pos.y - scale.y/2, 0);

            Node node = new Node(i++, vertices);
            nodes.Add(node.id,node);       

            Vector3[] vertices2 = new Vector3[3];
            vertices2[0] = new Vector3(pos.x - scale.x/2, pos.y + scale.y/2, 0);
            vertices2[1] = new Vector3(pos.x + scale.x/2, pos.y + scale.y/2, 0);
            vertices2[2] = new Vector3(pos.x + scale.x/2, pos.y - scale.y/2, 0);
           
            Node node2 = new Node(i++,vertices2);
            nodes.Add(node2.id,node2);
        }

        if(amin){
            foreach(GameObject cube in GameObject.FindGameObjectsWithTag("amin") ){
                Vector3 pos = cube.transform.position;
                Vector3 scale = cube.transform.localScale;
                Vector3[] vertices = new Vector3[3];
                vertices[0] = new Vector3(pos.x - scale.x/2, pos.y + scale.y/2, 0);
                vertices[1] = new Vector3(pos.x - scale.x/2, pos.y - scale.y/2, 0);
                vertices[2] = new Vector3(pos.x + scale.x/2, pos.y - scale.y/2, 0);

                Node node = new Node(i++, vertices);
                nodes.Add(node.id,node);       

                Vector3[] vertices2 = new Vector3[3];
                vertices2[0] = new Vector3(pos.x - scale.x/2, pos.y + scale.y/2, 0);
                vertices2[1] = new Vector3(pos.x + scale.x/2, pos.y + scale.y/2, 0);
                vertices2[2] = new Vector3(pos.x + scale.x/2, pos.y - scale.y/2, 0);
           
                Node node2 = new Node(i++,vertices2);
                nodes.Add(node2.id,node2);
            }
        }
    }

    public List<Node> AStar(Node start, Node end){

        List<Node> path = new List<Node>();
        NodeRecord startRecord = new NodeRecord(start,null,0,heuristic(start,end));
        Dictionary<Node,NodeRecord> open = new Dictionary<Node,NodeRecord>();
        Dictionary<Node,NodeRecord> close = new Dictionary<Node,NodeRecord>();

        open.Add(start,startRecord);
        NodeRecord current = new NodeRecord();

        while(open.Count > 0){
            current = smallestElement(open);

            if (current.node.Equals(end)){
                break;
            }

            List<Connection> currentConnections = getNodeConnections(current.node);

            foreach(Connection connection in currentConnections){
                Node endNode;
                if(connection.fromNode.id == current.node.id){
                    endNode = connection.toNode;
                } else{
                    endNode = connection.fromNode;
                }

                float endNodecost = current.costSoFar + connection.cost;

                NodeRecord endNodeRecord;
                float endNodeHeuristic;

                if (close.ContainsKey(endNode)){
                    endNodeRecord = close[endNode];
                    if (endNodeRecord.costSoFar <= endNodecost){
                        continue;
                    }
                    close.Remove(endNode);
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
        
                } else if (open.ContainsKey(endNode)){
                    endNodeRecord = open[endNode];
                    if (endNodeRecord.costSoFar<=endNodecost){
                        continue;
                    }
                    endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;

                } else {
                    endNodeRecord = new NodeRecord(endNode,null,0,0);
                    endNodeHeuristic = heuristic(endNode,end);

                }

                endNodeRecord.costSoFar = endNodecost;
                endNodeRecord.prev = current;
                endNodeRecord.connection = connection;
                endNodeRecord.estimatedTotalCost = endNodecost + endNodeHeuristic;

                if(!open.ContainsKey(endNode)){
                    open.Add(endNode,endNodeRecord);
                } 
            }

            open.Remove(current.node);
            close.Add(current.node,current);
        }

        if(current.node.id != end.id){
            return null;
         }else{
            
            NodeRecord nodeRecord = current;
            while(nodeRecord.node.id != start.id){
                path.Insert(0,nodeRecord.node);
                nodeRecord = nodeRecord.prev;
            }
            path.Insert(0,nodeRecord.node);
        }

        return path;
        

    }

    public List<Connection> getNodeConnections(Node node){
        List<Connection> nodeConnections = new List<Connection>();

        foreach(Connection connection in connections){
            if(node.id == connection.fromNode.id || node.id == connection.toNode.id){
                nodeConnections.Add(connection);
            }
        }

        return nodeConnections;
    }

    public bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b){
        Vector3 cp1 = Vector3.Cross(b-a, p1-a);
        Vector3 cp2 = Vector3.Cross(b-a, p2-a);
        
        return Vector3.Dot(cp1, cp2) >= 0;
    }

    public bool PointInTriangle(Vector3 p, Vector3[] vertices){
        Vector3 a, b, c;
        a = vertices[0];
        b = vertices[1];
        c = vertices[2];
        return SameSide(p,a, b,c) && SameSide(p,b, a,c) && SameSide(p,c, a,b);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
