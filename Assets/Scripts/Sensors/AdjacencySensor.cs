using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AdjacencySensor : MonoBehaviour 
{
	public List<SensedAgent> sensedAgents = new List<SensedAgent>();
	public float adjacencySensorRange;
	public bool showAdjacencySensor;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		sensor();
	}

	void sensor()
	{
		Vector3 aSensorRange = new Vector3(adjacencySensorRange*2,.1f,adjacencySensorRange*2);
		
		if(gameObject.transform.localScale != aSensorRange)
			gameObject.transform.localScale = aSensorRange;
		
		if(gameObject.GetComponent<MeshRenderer>().enabled != showAdjacencySensor)
			gameObject.GetComponent<MeshRenderer>().enabled = showAdjacencySensor;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != transform.parent.gameObject &&
			other.gameObject.tag != "Wall" &&
			(sensedAgents.FirstOrDefault(o => o.agent == other.gameObject) == null))
				sensedAgents.Add (new SensedAgent(other.gameObject, this.gameObject));
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Wall")
			sensedAgents.Remove (sensedAgents.FirstOrDefault(o => o.agent == other.gameObject));
	}
}

public class SensedAgent
{
	public GameObject 
		agent,
		sensor;
	float 
		distance,
		relativeHeading;

	public SensedAgent(GameObject a, GameObject s)
	{
		agent = a;
		sensor = s;
	}

	public float Distance()
	{
		return distance = Vector3.Distance(agent.transform.position, sensor.gameObject.transform.position);
	}

	public float RelativeHeading()
	{
		Vector3 v = Vector3.Normalize(agent.transform.position-sensor.transform.position);
		if(Vector3.Dot(v, sensor.transform.right) > 0)
			return relativeHeading = Vector3.Angle(sensor.transform.forward, v);
		else
			return relativeHeading = 360-Vector3.Angle(sensor.transform.forward, v);
	}

	public void refresh()
	{
		Distance();
		RelativeHeading();
	}
}

