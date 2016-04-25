using UnityEngine;
using System.Collections;

public class Driver : MonoBehaviour 
{
	public PathFinding pathFinding;
	public Tank tank;
	public GameManager gameManager;

	void Start()
	{
		DriverStart();
	}

	public void DriverStart()
	{
		tank = GetComponent<Tank>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
}
