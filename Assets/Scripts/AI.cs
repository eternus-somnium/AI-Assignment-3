using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : Driver {

    public Stack goalPath;

    //Goal is the overall target node that the tank is trying to get to
    public PathNode goal;

    //moveTarget is the next target in the path to move towards the goal
    public PathNode moveTarget; 
    public Tank combatTarget;

    public PathNode curNode;

    Tank tank;

    bool facingTarget = false;

    public int distanceToAttack = 2;

    float fireRate = 0.5f;
    float fireTimer;

	// Use this for initialization
	void Start () {
        p = GameObject.Find("ArenaAI").GetComponent<PathFinding>();

        if (p == null)
        {
            Debug.Log("Either ArenaAI does not exist in current context or it does not have script 'PathFinding' attached");
        }

        tank = gameObject.GetComponent<Tank>();

        fireTimer = fireRate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(tank.active)
		{
	        MakeDecisions();
	        SetTargetsAlongGoalPath();
	        HandleMovement();
	        HandleCombat();
		}

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
            curNode.OnTankOff(tank);
        }

        //Set a tank on the node, doing as soon as the target is set giving the AI a pseudo sense of knowing the direction of enemies
        target.OnTankOn(tank);

        moveTarget = target;
        tank.moveTarget = target;
        facingTarget = false;
        curNode = null;
    }

    private void MakeDecisions()
    {
        //Temporary variable, just putting this here to show that we can swap state from random roaming to chasing other tanks
        bool aggresive = true;
        //Temporary variable to show the ability for tanks to lock on to a single target and chase it
        bool lockOnInitialTarget = true;

        //If no current node and no move target, move to nearest node (handles getting tanks out of start gates)
        if (curNode == null)
        {
            if (moveTarget == null)
            {
                SetMoveTarget(p.GetNearestNode(gameObject));
            }

            return;
        }

        //TODO: Create real goal state machine for tanks (ie. attack, pick up health, move to corner, etc)

        //If there is a combat target already, decide how to handle it
        if (combatTarget != null)
        {
            //lockOnInitialTarget is an example of having a tank always chase its target, it has no other logic than to follow combatTarget
            if (lockOnInitialTarget)
            {
                if (combatTarget.moveTarget != null && combatTarget.moveTarget != goal)
                {
                    if ((float)tank.health / (float)tank.maxHealth <= 0.5f && (float)combatTarget.health / (float)combatTarget.maxHealth > 0.5f)
                    {
                        combatTarget = null;
                        FindHealth();
                    }
                    else
                    {
                        ChaseTarget();
                    }
                }
            }
            
            //Set combat target to null at some point when it is best to (Possibly when low HP, target is dead, etc)

            //Returning here may or may not be the right choice, for now Im thinking we'll make combatTarget null when necessary and dont do other logic until then
            return;
        }

        //Handles determining the next goal of the tank (To move somewhere, to find a combat target, etc)
        if (goal == null && moveTarget == null)
        {
            //TODO: Currently just finding a random node and moving towards it, implement real logic here
            if (!aggresive)
            {
                goal = p.GetRandomGoal();
                goalPath = p.GetPathToTarget(curNode, goal);
            }
            //TODO: Currently just chasing closest tank, implement real logic here
            else
            {
                if ((float)tank.health / (float)tank.maxHealth <= 0.5f)
                {
                    FindHealth();
                }
                else
                {
                    FindEnemey();
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
                    tank.moveTarget = null;
                }
                //Target not reached, move towards target
                else
                {
                    transform.position = Vector3.MoveTowards(ZeroOutY(transform.position), ZeroOutY(moveTarget.transform.position), moveStep);
                }
            }
        }
    }

    private void HandleCombat()
    {
        if (combatTarget != null)
        {
            if (combatTarget.safe)
            {
                combatTarget = null;
                goal = null;
                goalPath = null;
                return;
            }

            if (goalPath != null && goalPath.Count > 4)
            {
                //Don't shoot if still traveling to target and far away
                return;
            }

            tank.RotateTurretTowards(combatTarget.gameObject);

            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0)
            {
                tank.FireMain();
                fireTimer = fireRate;
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

                if (goalPath.Count == ((combatTarget == null) ? 0 : distanceToAttack))
                {
                    //Debug.Log("Moving Towards Final Goal: " + tank.gameObject.name);
                    goal = null;
                    goalPath = null;
                }
            }
        }
    }

    private void FindEnemey()
    {
        goal = pathFinding.GetNearest(curNode, PathNode.TouchingObjects.Tanks);

        if (goal != null)
        {
            if (goal.tanksOnNode.Count > 0)
            {
                //Selecting combat target as first tank listed on node, may want to move to lowest health or highest points in future
                float lowestHealth = Mathf.Infinity;

                for (int i = 0; i < goal.tanksOnNode.Count; ++i)
                {
                    if (goal.tanksOnNode[i].safe || goal.tanksOnNode[i] == tank)
                    {
                        continue;
                    }

                    if (goal.tanksOnNode[i].health < lowestHealth)
                    {
                        lowestHealth = goal.tanksOnNode[i].health;
                        combatTarget = goal.tanksOnNode[i];
                    }
                }

                //Once combat target is selected, it could be chased by setting goal to combatTarget.moveTarget continuously
                //We may need some sort of "re-calculate path everytime it changes" type of logic
            }

            goalPath = pathFinding.GetPathToTarget(curNode, goal);
        }
    }

    private void FindHealth()
    {
        goal = pathFinding.GetNearest(curNode, PathNode.TouchingObjects.Health);

        if (goal != null)
        {
            goalPath = pathFinding.GetPathToTarget(curNode, goal);
        }
    }

    private void ChaseTarget()
    {
        goal = combatTarget.moveTarget;
        goalPath = pathFinding.GetPathToTarget(curNode, goal);
    }

    public void OnDeath()
    {
        if (moveTarget != null)
        {
            moveTarget.OnTankOff(tank);
        }

        curNode = null;
        moveTarget = null;
        combatTarget = null;
        goal = null;
        goalPath = null;

        facingTarget = false;
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
