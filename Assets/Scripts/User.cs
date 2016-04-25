using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : Driver 
{
	float setMoveTargetTimer=0;

	void Start () 
	{
		DriverStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(tank.active)
		{
			MoveController();
			TurretController();
			WeaponController();
			SetMoveTarget();
		}

        //Set all enemy tank's combat target to player (for testing)
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");

            foreach (GameObject t in tanks)
            {
                AI ai = tank.GetComponent<AI>();

                if (ai != null)
                {
					ai.combatTarget = gameObject.GetComponent<Tank>();
                }
            }
        }
	}

	void MoveController()
	{	
		float moveDirection = Input.GetAxis("Vertical");
		float turnDirection = Input.GetAxis("Horizontal");
		tank.Move(moveDirection, turnDirection);
	}

	void TurretController()
	{
		float turretRotation = Input.GetAxis("Mouse X")*10;
		tank.RotateTurret(turretRotation);
	}

	void WeaponController()
	{
		if(Input.GetButtonDown("Fire1"))
			tank.FireMain();
		if(Input.GetButtonDown("Fire2"))
			tank.FireSecondary();
	}

    //Sets move target as nearest node every second so that AI can track player
    void SetMoveTarget()
    {
		if(setMoveTargetTimer > 2)
		{
	        if (tank.moveTarget != null)
	        {
	            tank.moveTarget.OnTankOff(tank);
	        }

	        tank.moveTarget = pathFinding.GetNearestNode(gameObject);

	        tank.moveTarget.OnTankOn(tank);
			setMoveTargetTimer = 0;
		}
		else
			setMoveTargetTimer += Time.deltaTime;
    }
}
