using UnityEngine;
using System.Collections;

public class PathFinding : MonoBehaviour {

    AStar aStar;

    private PathNode[] nodes;

	// Use this for initialization
	void Start () {
        aStar = gameObject.GetComponent<AStar>();

        if (aStar == null)
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

    public PathNode GetRandomMoveTarget()
    {
        return nodes[Random.Range(0, nodes.Length)];
    }

    //Returns the stack "path" of a specific target, uses AStar search
    public Stack GetPathToTarget(PathNode currentNode, PathNode targetNode)
    {
        return aStar.AStarSearch(currentNode, targetNode);
    }
}
