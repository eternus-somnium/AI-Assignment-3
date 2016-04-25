using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArenaBoardRight : Interface 
{
	Tank playerTank;
	GameObject 
		playerHealth,
		playerAmmo;

	// Use this for initialization
	void Start () 
	{
		playerHealth = GameObject.Find("HealthText");
		playerAmmo = GameObject.Find("AmmoText");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerTank == null)
		{
			if(g.units.Length > 0)
				playerTank = g.units[0].GetComponent<Tank>();
		}
		else
		{
			playerHealth.GetComponent<Text>().text = playerTank.health + "/" + playerTank.maxHealth;
			playerAmmo.GetComponent<Text>().text = playerTank.ammo + "/" + playerTank.maxAmmo;
		}
	}
}
