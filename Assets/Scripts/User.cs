using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User : MonoBehaviour 
{
	Tank t;
	// Use this for initialization
	void Start () 
	{
		t = GetComponent<Tank>();
		GameObject.Find("Main Camera").GetComponent<CameraController>().playerTank = gameObject;
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


}
