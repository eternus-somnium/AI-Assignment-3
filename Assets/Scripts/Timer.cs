using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
	public float timeRemaining;
	bool active;
	GameManager g;
    Spawner s;

    public float buffRespawnRate = 30f;
    public float buffRespawnTimer;

    public bool buffRespawnTimerActive = false;
	// Use this for initialization
	void Start () 
	{
		g = GameObject.Find("GameManager").GetComponent<GameManager>();
        s = GetComponent<Spawner>();

        buffRespawnTimer = buffRespawnRate;
	}
	
	// Update is called once per frame
	void Update () 
	{
        float deltaTime = Time.deltaTime;

		if(active && timeRemaining > 0)
			timeRemaining -= deltaTime;
		else if(active)
		{
			active = false;
			g.TimerEnd();
		}

        if (buffRespawnTimerActive && buffRespawnTimer >= 0f)
        {
            buffRespawnTimer -= deltaTime;
        }
        else if (buffRespawnTimerActive)
        {
            s.SpawnItems();
            buffRespawnTimer = buffRespawnRate;
        }
	}

	public void StartTimer(float time)
	{
		active = true;
		timeRemaining = time;
	}
}
