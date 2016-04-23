using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {

    public GameObject up = null, down = null, left = null, right = null;
    public PathNode[] neighbors;

    public enum TouchingObjects { Tanks, HasItem, Health, Buffs };

    public PathNode cameFrom, dijkstraCameFrom;
    public float g, h, f;

    public float distance;

    public int dijkstraIndex;

    int tanks;

    bool hasTank, hasItem, hasHealth, hasBuff;

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

    public void OnTankOn()
    {
        tanks++;
        hasTank = true;
    }

    public void OnTankOff()
    {
        tanks--;

        if (tanks <= 0)
        {
            hasTank = false;
        }
    }

    public void OnHealthStateChanged(bool state)
    {
        hasHealth = state;
    }

    public void OnBuffStateChanged(bool state)
    {
        hasBuff = state;
    }

    public bool HasType(TouchingObjects type)
    {
        switch (type)
        {
            case (TouchingObjects.Tanks):
                return hasTank;
            case (TouchingObjects.HasItem):
                return hasItem;
            case (TouchingObjects.Health):
                return hasHealth;
            case (TouchingObjects.Buffs):
                return hasBuff;
            default:
                return false;
        }
    }

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
