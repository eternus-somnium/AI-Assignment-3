﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public int state = 0; //0=menu, 1=arena, 2=shop, 3= results
	public float roundTime, shopTime;

	MapGenerator g;
	Spawner s;
	Timer t;

	public GameObject[] 
		units,
		parts;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		s = GetComponent<Spawner>();
		g = GetComponent<MapGenerator>();
		t = GetComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void FixedUpdate()
	{
	}

	void OnLevelWasLoaded(int level)
	{
		switch(level)
		{
		case 0:
			break;
		case 1:
			Invoke("SetupRound",.1f);
			break;
		case 2:
			break;
		case 3:
			break;
		}
	}

	public void TimerEnd()
	{
		if(SceneManager.GetActiveScene().name == "Arena")
			EndRound();
		else if(SceneManager.GetActiveScene().name == "Shop")
			;
	}

	void SetupRound()
	{
		g.generateMap();

		s.FindHangers();
		if(units.Length == 0)
			units = s.SetupTanks();

		foreach(GameObject u in units)
		{
			s.SpawnTank(u, true);
		}
		t.StartTimer(roundTime*60);
	}

	void EndRound()
	{
		foreach(GameObject u in units)
			u.GetComponent<Tank>().EndRound();
		SceneManager.LoadScene("Shop");
	}
}