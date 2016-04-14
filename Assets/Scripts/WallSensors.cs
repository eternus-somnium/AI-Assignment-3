using UnityEngine;
using System.Collections;

public class WallSensors : MonoBehaviour 
{
	public float[] wallSensorReadings = new float[4]; //0=forward, 1=right, 2=left, 3=back
	public float wallSensorRange;
	public bool showWallSensors;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		Sensors();
		if(showWallSensors) VisualizeWallSensors();
	}

	void Sensors()
	{
		//Create rays
		Ray fWallSensor = new Ray(transform.position, transform.forward);
		Ray rWallSensor = new Ray(transform.position, transform.right+transform.forward);
		Ray lWallSensor = new Ray(transform.position, -transform.right+transform.forward);

		RaycastHit h;
		
		//Set raycast results
		//Forward
		if(Physics.Raycast(fWallSensor, out h, wallSensorRange) && h.transform.gameObject.tag == "Wall")
			wallSensorReadings[0] = Vector3.Distance(gameObject.transform.position, h.point);
		else wallSensorReadings[0] = wallSensorRange;

		//Right
		if(Physics.Raycast(rWallSensor, out h, wallSensorRange) && h.transform.gameObject.tag == "Wall")
			wallSensorReadings[1] = Vector3.Distance(gameObject.transform.position, h.point);
		else wallSensorReadings[1] = wallSensorRange;

		//Left
		if(Physics.Raycast(lWallSensor, out h, wallSensorRange) && h.transform.gameObject.tag == "Wall")
			wallSensorReadings[2] = Vector3.Distance(gameObject.transform.position, h.point);
		else wallSensorReadings[2] = wallSensorRange;
	}


	void VisualizeWallSensors()
	{
		Debug.DrawRay(transform.position, transform.forward*wallSensorRange, Color.black);
		Debug.DrawRay(transform.position, Vector3.Normalize(transform.forward + transform.right)*wallSensorRange, Color.black);
		Debug.DrawRay(transform.position, Vector3.Normalize(transform.forward - transform.right)*wallSensorRange, Color.black);
	}
}
