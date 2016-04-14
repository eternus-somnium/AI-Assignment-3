using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject playerTank;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerTank != null)
			transform.position = new Vector3(playerTank.transform.position.x,
											 25,
											 playerTank.transform.position.z);
	}
}
