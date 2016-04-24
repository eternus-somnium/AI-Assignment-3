using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {

    public bool hp, ammo;

    private int hpRestore = 50;

    PathNode node;

    void Start()
    {
        node = GameObject.Find("ArenaAI").GetComponent<PathFinding>().GetNearestNode(gameObject);

        node.OnHealthStateChanged(true);
    }

    void OnCollisionEnter(Collision col)
    {
        Tank tank = col.gameObject.GetComponent<Tank>();

        if (tank == null)
        {
            Debug.Log("Not tank touched buff somehow");
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
        }
        else if (ammo)
        {

        }

        node.OnHealthStateChanged(false);
        Destroy(gameObject);
    }
}
