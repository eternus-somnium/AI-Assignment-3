using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour 
{
	public int units;
	public GameObject tankPrefab;
	public GameObject[] hangers;

    public GameObject HPUpPrefab;
    public GameObject AmmoUpPrefab;

	public void FindHangers()
	{
		hangers = GameObject.FindGameObjectsWithTag("Hanger");
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

	//Sets the tank's health and ammo to max and places it in its hanger
	public void SpawnTank(GameObject unit, bool rebuild)
	{
		Tank tankScript = unit.GetComponent<Tank>();
		tankScript.health = tankScript.maxHealth;
		tankScript.ammo = tankScript.maxAmmo;
        tankScript.safe = true;
		unit.transform.position = tankScript.hangerPosition;
		if(rebuild)
			tankScript.startRound();
		unit.transform.rotation = tankScript.hangerRotation;
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

    public void SpawnItems()
    {
        HPUpPrefab = (GameObject)Resources.Load("Prefabs/HPUp");

        int numHPToSpawn = 5;
        int numAmmoToSpawn = 5;

        for (int i = 0; i < numHPToSpawn; ++i)
        {
            GameObject hpInstance = Instantiate(HPUpPrefab);

            hpInstance.transform.position = new Vector3(Random.Range(0, 10) * 10, 2f, Random.Range(0, 10) * 10);
        }
    }
}
