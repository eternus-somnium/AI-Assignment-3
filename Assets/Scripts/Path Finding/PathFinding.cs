using UnityEngine;
using System.Collections;

public class PathFinding : MonoBehaviour {

    AStar aStar;
    Dijkstras dijkstras;

    private PathNode[] nodes;

	// Use this for initialization
	void Start () {
        aStar = gameObject.GetComponent<AStar>();
        dijkstras = gameObject.GetComponent<Dijkstras>();

        if (aStar == null || dijkstras == null)
        {
            Debug.Log("PathFinding needs to be on the same GameObject as AStar and Dijkstras");
        }

        nodes = GameObject.Find("PathNodes").GetComponentsInChildren<PathNode>();
	}

    //Gets the path node nearest to the passed in game object
    public PathNode GetNearestNode(GameObject obj)
    {
        PathNode start = nodes[0];

        foreach (PathNode node in nodes)
        {
            if (Vector3.Distance(obj.transform.position, node.transform.position) < Vector3.Distance(obj.transform.position, start.transform.position))
            {
                start = node;
            }
        }

        return start;
    }

    public PathNode GetRandomGoal()
    {
        return nodes[Random.Range(0, nodes.Length)];
    }

    //Returns the stack "path" of a specific target, uses AStar search
    public Stack GetPathToTarget(PathNode currentNode, PathNode targetNode)
    {
        return aStar.AStarSearch(currentNode, targetNode);
    }

    //Searches all nodes on the map for one with a specific type (tank on it, item, etc) and returns the nearest one to the source
    public PathNode GetNearest(PathNode source, PathNode.TouchingObjects objType)
    {
        PathNode nearest = null;

        dijkstras.search(source);

        float minDist = Mathf.Infinity;

        foreach (PathNode node in nodes)
        {
            if (node != source && node.HasType(objType) && node.distance < minDist)
            {
                nearest = node;
                minDist = node.distance;
            }
        }

        //Potentially null
        return nearest;
    }
}
