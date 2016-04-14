using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
	public int 
		health,
		maxHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void EntityUpdate () 
	{
		if(health < 1  && maxHealth != 0) 
			Death();
	
	}

	public virtual void Death(){}
}
