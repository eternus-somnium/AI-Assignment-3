using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour 
{
	public PathFinding pathFinding;
	public Tank t;
	void Start()
	{
		DriverStart();
	}

	public void DriverStart()
	{
		t = GetComponent<Tank>();
	}
}
