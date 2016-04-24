using UnityEngine;
using System.Collections;

public class BasicCannon : Weapon 
{
    GameObject bullets;

	// Use this for initialization
	void Start () 
	{
        bullets = GameObject.Find("Bullets");

		WeaponStart();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Fire()
	{
		Vector3 spawnPosition = transform.position + projectile[0].transform.position;
		GameObject shot = (GameObject) Instantiate(projectile[0], spawnPosition, transform.rotation);
        shot.transform.SetParent(bullets.transform);
		shot.GetComponent<Shot>().unit = unit;
	}
}
