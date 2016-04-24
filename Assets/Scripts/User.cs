using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour 
{
	Tank t;
    PathFinding pathFinding;

	void Start () 
	{
		t = GetComponent<Tank>();
		GameObject.Find("Main Camera").GetComponent<CameraController>().playerTank = gameObject;

        pathFinding = GameObject.Find("ArenaAI").GetComponent<PathFinding>();

        InvokeRepeating("SetMoveTarget", 0, 2.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(t.active)
		{
			MoveController();
			TurretController();
			WeaponController();
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
        if (t.moveTarget != null)
        {
            t.moveTarget.OnTankOff(t);
        }

        t.moveTarget = pathFinding.GetNearestNode(gameObject);

        t.moveTarget.OnTankOn(t);
    }
}
