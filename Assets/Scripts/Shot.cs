using UnityEngine;
using System.Collections;

/* Base code for all shot objects.  All shots disapppear when they move offscreen
 */
public class Shot : MonoBehaviour
{
	public int health;
	public float 
		baseDamage,
		baseSpeed;
	public GameObject unit;
	
	public void ShotStart()
	{
	}

	public void Update()
	{
		transform.Translate(Vector3.forward * baseSpeed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log(c.collider.name);
		if(c.transform.root != unit.transform)
		{						
			Tank victim = c.collider.GetComponent<Tank>();

			if(victim != null)
			{
				if(victim.health-Mathf.RoundToInt(baseDamage) <= 0)
					unit.GetComponent<Tank>().addKill(victim.bounty);
				c.collider.GetComponent<Entity>().health-=Mathf.RoundToInt(baseDamage);
			}

			Destroy(gameObject);
		}
	}

}
