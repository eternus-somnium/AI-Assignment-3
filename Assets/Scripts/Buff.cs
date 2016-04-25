using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {

    public bool hp, ammo;

    private int hpRestore = 50;
    private int ammoRestore = 10;

    PathNode node;

    void Start()
    {
        node = GameObject.Find("ArenaAI").GetComponent<PathFinding>().GetNearestNode(gameObject);

        if (hp)
        {
            node.OnHealthStateChanged(true);
        }
        else if (ammo)
        {
            node.OnAmmoStateChanged(true);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Tank tank = col.gameObject.GetComponent<Tank>();

        if (tank == null)
        {
            //Debug.Log("Not tank touched buff somehow");
            return;
        }

        if (hp)
        {
            if (tank.health + hpRestore <= tank.maxHealth)
            {
                tank.health += hpRestore;
            }
            else
            {
                tank.health = tank.maxHealth;
            }

            node.OnHealthStateChanged(false);
        }
        else if (ammo)
        {
            if (tank.ammo + ammoRestore <= tank.maxAmmo)
            {
                tank.ammo += ammoRestore;
            }
            else
            {
                tank.ammo = tank.maxAmmo;
            }

            node.OnAmmoStateChanged(false);
        }

      
        Destroy(gameObject);
    }
}
