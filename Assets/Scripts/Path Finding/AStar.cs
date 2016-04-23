using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    public bool selectMode = true;
    private bool selectStart, selectTarget;
    private bool moveModeReady = false;

    public PathNode start, target;

    private PathNode[] nodes;

    PathNode next;

    private Stack path;

    void Start()
    {
        nodes = GameObject.Find("PathNodes").GetComponentsInChildren<PathNode>();

        path = new Stack();
    }

    public void SelectStart(GameObject _start)
    {
        if (start != null)
        {
            start.gameObject.GetComponent<Renderer>().material.color = new Color(0.58f, 0.58f, 0.58f);
        }

        start = _start.GetComponent<PathNode>();
    }

    public void SelectTarget(GameObject _target)
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Renderer>().material.color = new Color(0.58f, 0.58f, 0.58f);
        }

        target = _target.GetComponent<PathNode>();
    }

    void Update()
    {
        if (selectMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                next = null;
                path = AStarSearch(start, target);
            }

            if (path.Count > 0)
            {
                if (next != null && next != target && next != start)
                {
                    next.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }

                next = (PathNode)path.Pop();
            }
        }
    }

    public Stack AStarSearch(PathNode start, PathNode target)
    {
        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> openSet = new List<PathNode>();

        openSet.Add(start);

        foreach (PathNode node in nodes)
        {
            //Reset Colors
            if (node != start && node != target)
            {
                node.gameObject.GetComponent<Renderer>().material.color = new Color(0.58f, 0.58f, 0.58f);
            }

            node.cameFrom = null;
            node.g = float.MaxValue;
            node.f = float.MaxValue;
        }

        start.f = HeuristicEstimate(start, target);

        while (openSet.Count != 0)
        {
            PathNode current = null;

            //Select node from openset with lowest f value
            foreach (PathNode node in openSet)
            {
                if (current == null)
                {
                    current = node;
                }
                else if (node.f < current.f)
                {
                    current = node;
                }
            }

            if (current == target)
            {
                return ReconstructPath(target); //Return the path here
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (PathNode neighbor in current.neighbors)
            {
                if (neighbor == null || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentative_g = current.g + Vector3.Distance(current.gameObject.transform.position, neighbor.gameObject.transform.position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentative_g >= neighbor.g)
                {
                    continue;
                }

                neighbor.cameFrom = current;
                neighbor.g = tentative_g;
                neighbor.f = neighbor.g + HeuristicEstimate(neighbor, target);
            }
        }

        return null; //Failure
    }

    float HeuristicEstimate(PathNode start, PathNode target)
    {
        return Vector3.Distance(start.gameObject.transform.position, target.gameObject.transform.position);
    }

    Stack ReconstructPath(PathNode target)
    {
        Stack path = new Stack();

        while (target != null)
        {
            path.Push(target);

            target = target.cameFrom;
        }

        return path;
    }

    public string SelectionMode()
    {
        if (selectStart)
        {
            return "start";
        }
        else if (selectTarget)
        {
            return "target";
        }

        return "";
    }
}
