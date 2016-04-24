using UnityEngine;
using System.Collections;

public class HangerDoor : MonoBehaviour {

    void OnCollisionExit(Collision col)
    {
        Tank tank = col.gameObject.GetComponent<Tank>();

        if (tank != null)
        {
            tank.safe = false;
        }
    }
}
