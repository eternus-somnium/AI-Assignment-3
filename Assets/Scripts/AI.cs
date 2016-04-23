using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

    PathFinding pathFinding;

    public Stack goalPath;

    //Goal is the overall target node that the tank is trying to get to
    public PathNode goal;

    //moveTarget is the next target in the path to move towards the goal
    public PathNode moveTarget; 
    GameObject combatTarget;

    public PathNode curNode;

    Tank tank;

    bool facingTarget = false;

	// Use this for initialization
	void Start () {
        pathFinding = GameObject.Find("ArenaAI").GetComponent<PathFinding>();

        if (pathFinding == null)
        {
            Debug.Log("Either ArenaAI does not exist in current context or it does not have script 'PathFinding' attached");
        }

        tank = gameObject.GetComponent<Tank>();
	}
	
	// Update is called once per frame
	void Update () {
        MakeDecisions();
        SetTargetsAlongGoalPath();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(goalPath.Count);
        }
	}

    //Handles resetting any required values when changing targets, and setting the target
    private void SetMoveTarget(PathNode target)
    {
        if (target == curNode)
        {
            return;
        }

        //Tank no longer on current node (used for pathfinding tanks/items)
        if (curNode != null)
        {
            curNode.OnTankOff();
        }

        //Set a tank on the node, doing as soon as the target is set giving the AI a pseudo sense of knowing the direction of enemies
        target.OnTankOn();

        moveTarget = target;
        facingTarget = false;
        curNode = null;
    }

    private void MakeDecisions()
    {
        //Temporary variable, just putting this here to show that we can swap state from random roaming to chasing other tanks
        bool aggresive = false;

        //If no current node and no move target, move to nearest node (handles getting tanks out of start gates)
        if (curNode == null)
        {
            if (moveTarget == null)
            {
                SetMoveTarget(pathFinding.GetNearestNode(gameObject));
            }

            return;
        }

        //TODO: Create real goal state machine for tanks (ie. attack, pick up health, move to corner, etc)
        if (goal == null && moveTarget == null)
        {
            //TODO: Currently just finding a random node and moving towards it, implement real logic here
            if (!aggresive)
            {
                goal = pathFinding.GetRandomGoal();
                goalPath = pathFinding.GetPathToTarget(curNode, goal);
            }
            //TODO: Currently just chasing closest tank, implement real logic here
            else
            {
                goal = pathFinding.GetNearest(curNode, PathNode.TouchingObjects.Tanks);

                if (goal != null)
                {
                    goalPath = pathFinding.GetPathToTarget(curNode, goal);
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (moveTarget != null)
        {
            //TODO: Update turnstep to have actual turning value, couldnt find it
            float turnStep = 0.25f * Time.deltaTime;
            float moveStep = tank.speed * Time.deltaTime;

            //If not facing target, turn towards it before moving to it
            //Using facingTarget as a way to avoid unecessarily recalculating dot product when already facing target for efficiency
            //Second part of this check (Distance) ensures it is not already on that node, thus causing it to try to look at something below it
            if (!facingTarget || DistanceNoY(transform.position, moveTarget.transform.position) < 0.25f) 
            {
                float dotProduct = Vector3.Dot(transform.forward, (ZeroOutY(moveTarget.transform.position) - ZeroOutY(transform.position)).normalized);

                //Still need to turn more
                if (dotProduct < 0.99f)
                {
                    //Debug.Log(dotProduct + tank.gameObject.name);
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, ZeroOutY(moveTarget.transform.position - transform.position), turnStep, 0.0f));
                }
                //Now facing target
                else
                {
                    facingTarget = true;
                }
            }
            //Already facing target, start moving towards it
            else
            {
                facingTarget = true;

                //Target has been reached
                if (DistanceNoY(transform.position, moveTarget.transform.position) < 0.5f)
                {
                    //Debug.Log("Tank: " + tank.gameObject.name + " hit target");
                    curNode = moveTarget;
                    moveTarget = null;
                }
                //Target not reached, move towards target
                else
                {
                    transform.position = Vector3.MoveTowards(ZeroOutY(transform.position), ZeroOutY(moveTarget.transform.position), moveStep);
                }
            }
        }
    }

    private void SetTargetsAlongGoalPath()
    {
        if (goal != null && goalPath != null && goalPath.Count >= 1)
        {
            if (moveTarget == null)
            {
                SetMoveTarget((PathNode)goalPath.Pop());

                if (goalPath.Count == 0)
                {
                    //Debug.Log("Moving Towards Final Goal: " + tank.gameObject.name);
                    goal = null;
                    goalPath = null;
                }
            }
        }
    }

    private Vector3 ZeroOutY(Vector3 vec)
    {
        //Set Y to actual "0" of map (currently 1.5)
        return new Vector3(vec.x, 0.5f, vec.z);
    }

    private float DistanceNoY(Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(ZeroOutY(vec1), ZeroOutY(vec2));
    }
}
