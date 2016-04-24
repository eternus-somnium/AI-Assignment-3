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
		if(t.active)
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

            foreach (GameObject tank in tanks)
            {
                AI ai = tank.GetComponent<AI>();

                if (ai != null)
                {
                    ai.combatTarget = t;
                }
            }
        }
	}

	void MoveController()
	{	
		float moveDirection = Input.GetAxis("Vertical");
		float turnDirection = Input.GetAxis("Horizontal");
		t.Move(moveDirection, turnDirection);
	}

	void TurretController()
	{
		float turretRotation = Input.GetAxis("Mouse X")*10;
		t.RotateTurret(turretRotation);
	}

	void WeaponController()
	{
		if(Input.GetButtonDown("Fire1"))
			t.FireMain();
		if(Input.GetButtonDown("Fire2"))
			t.FireSecondary();
	}

    //Sets move target as nearest node every second so that AI can track player
    void SetMoveTarget()
    {
		if(setMoveTargetTimer > 2)
		{
	        if (t.moveTarget != null)
	        {
	            t.moveTarget.OnTankOff(t);
	        }

	        t.moveTarget = p.GetNearestNode(gameObject);

	        t.moveTarget.OnTankOn(t);
			setMoveTargetTimer = 0;
		}
		else
			setMoveTargetTimer += Time.deltaTime;
    }
}
