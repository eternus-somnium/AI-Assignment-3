using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour 
{
//Main menu options
	public int 
		selectedMode = 1,
		selectedEnvironment = 1;

	public bool 
		online,
		notecards,
		slideshow,
		lTimer,
		hTimer;

	public string 
		slidshowFolderPath,
		notecardFolderPath;

	public int 
		lTime, 
		hTime;

//Eyeline Metrics
	//	Computer,walls,celing,floor,furniture,slides,notes,audiencefrontleft,audiencefrontright,audiencebackleft,audiencebackright,other
	public int eyelineCategories = 17;
	public float[] eyelineMeasurements; 
// Volume metrics
	public List<float> volumeReadings;

	void Awake() 
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		eyelineMeasurements = new float[eyelineCategories];
		volumeReadings = new List<float>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
