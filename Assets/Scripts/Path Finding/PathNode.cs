using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {

    public GameObject up = null, down = null, left = null, right = null;
    public PathNode[] neighbors;

    public PathNode cameFrom;
    public float g, h, f;

    private AStar astar;

    void Start()
    {
        astar = GameObject.Find("ArenaAI").GetComponent<AStar>();

        neighbors = new PathNode[4];

        int i = 0;
        if (up != null)
        {
            neighbors[i++] = up.GetComponent<PathNode>();
        }
        if (down != null)
        {
            neighbors[i++] = down.GetComponent<PathNode>();
        }
        if (left != null)
        {
            neighbors[i++] = left.GetComponent<PathNode>();    
        }
        if (right != null)
        {
            neighbors[i++] = right.GetComponent<PathNode>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
        }
    }

    /*
    void OnMouseDown()
    {
        if (astar.SelectionMode() == "start")
        {
            astar.SelectStart(gameObject);

            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (astar.SelectionMode() == "target")
        {
            astar.SelectTarget(gameObject);
                
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }*/

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            astar.SelectStart(gameObject);

            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            astar.SelectTarget(gameObject);

            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
