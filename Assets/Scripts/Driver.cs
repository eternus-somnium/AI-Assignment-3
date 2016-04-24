using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour 
{
	public PathFinding p;
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
