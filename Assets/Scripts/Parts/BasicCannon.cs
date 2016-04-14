using UnityEngine;
using System.Collections;

public class BasicCannon : Weapon 
{

	// Use this for initialization
	void Start () 
	{
		WeaponStart();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Fire()
	{
		Vector3 spawnPosition = transform.position;
		GameObject shot = (GameObject) Instantiate(projectile[0], spawnPosition, transform.rotation);
		shot.GetComponent<Shot>().unit = unit;
	}
}
