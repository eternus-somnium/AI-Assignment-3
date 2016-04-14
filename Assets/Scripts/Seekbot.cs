using UnityEngine;
using System.Collections;

public class Seekbot : MonoBehaviour 
{
	bool moving = true;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(moving)
		{
			transform.LookAt(GameObject.Find("User(Clone)").transform);
			transform.Translate(Vector3.forward);
		}
	}


	void OnTriggerEnter()
	{
		moving = false;
	}
}
