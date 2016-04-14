using UnityEngine;
using System.Collections;

public class Weapon : Part 
{
	int i;
	public GameObject unit;
	public GameObject[] projectile;
	public float[] 
		projectileSpeed,
		projectileDamage;



	// Use this for initialization
	public void WeaponStart () 
	{
		unit = transform.root.gameObject;
		for(i=0;i<projectile.Length;i++)
		{
			Shot s = projectile[i].GetComponent<Shot>();
			s.baseSpeed = projectileSpeed[i];
			s.baseDamage = projectileDamage[i];
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public virtual void Fire(){}
}
