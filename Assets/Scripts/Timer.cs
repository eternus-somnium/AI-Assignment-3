using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
	public float timeRemaining;
	bool active;
	GameManager g;
	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(active && timeRemaining > 0)
			timeRemaining -= Time.deltaTime;
		else if(active)
		{
			active = false;
			g.TimerEnd();
		}
	}

	public void StartTimer(float time)
	{
		active = true;
		timeRemaining = time;
	}
}
