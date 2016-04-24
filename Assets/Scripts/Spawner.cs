using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour 
{
	public int units;
	public GameObject tankPrefab;
	public GameObject[] hangers;
	public LeftPanel leftPanel;

	public void FindHangers()
	{
		hangers = GameObject.FindGameObjectsWithTag("Hanger");
		leftPanel = GameObject.Find("LeftPanel").GetComponent<LeftPanel>();
	}
	public GameObject[] SetupTanks()
	{
		int i;
		GameObject[] tanks = new GameObject[units];
		for(i=0;i<units;i++)
		{
			tanks[i] = (GameObject)Instantiate(
				tankPrefab,
				Vector3.zero,
				Quaternion.identity);

			tanks[i].name = "Tank: " + i;
			Tank tankScript = tanks[i].GetComponent<Tank>();
			tankScript.color = GenerateColor(i);
			tankScript.hangerPosition = hangers[i].transform.position;
			tankScript.hangerRotation = hangers[i].transform.rotation;

			if(i == 0)
				tanks[i].AddComponent<User>();
			else
				tanks[i].AddComponent<AI>();
		}
		return tanks;
	}
	public void SpawnTank(GameObject unit, bool rebuild)
	{
		Tank tankScript = unit.GetComponent<Tank>();
		tankScript.health = tankScript.maxHealth;
		unit.transform.position = tankScript.hangerPosition;
		if(rebuild)
			tankScript.startRound();
		unit.transform.rotation = tankScript.hangerRotation;
		tankScript.setBountyBoardValue();

	}

	public Color GenerateColor(float i)
	{
		
		float 
			spread = 255*3/units,
			r,
			g,
			b;
		i = (i+1)*spread;

		r = Mathf.Min(255, i);
		i-= r;
		g = i>0 ? Mathf.Min(255, i) : 0;
		i-=g;
		b = i>0 ? Mathf.Min(255, i) : 0;

		return new Color(r/255,g/255,b/255);
	}
}
