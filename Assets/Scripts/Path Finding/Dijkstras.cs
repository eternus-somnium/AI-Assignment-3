using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dijkstras : MonoBehaviour {

    private PathNode[] nodes;

	// Use this for initialization
	void Start () {
        nodes = GameObject.Find("PathNodes").GetComponentsInChildren<PathNode>();
	}

    public Stack search(PathNode source)
    {
        Stack paths = new Stack();
        List<PathNode> unvisitedNodes = new List<PathNode>();

        int index = 0;

        foreach (PathNode node in nodes)
        {
            node.distance = Mathf.Infinity;
            node.dijkstraCameFrom = null;

            unvisitedNodes.Add(node);
        }

        source.distance = 0;

        while (unvisitedNodes.Count > 0)
        {
            float minDist = Mathf.Infinity;
            PathNode curNode = null;

            foreach(PathNode node in unvisitedNodes)
            {
                if (node.distance <= minDist)
                {
                    curNode = node;
                    minDist = node.distance;
                }
            }

            unvisitedNodes.Remove(curNode);

            //unvisitedNodes.RemoveAll(x => x.gameObject.name == curNode.gameObject.name);

            foreach (PathNode neighbor in curNode.neighbors)
            {
                if (neighbor == null || !unvisitedNodes.Contains(neighbor))
                {
                    continue;
                }

                float curDistance = curNode.distance + 1;

                if (curDistance < neighbor.distance)
                {
                    neighbor.distance = curDistance;
                    neighbor.dijkstraCameFrom = curNode;
                }
            }
        }

        return paths;
    }
}
