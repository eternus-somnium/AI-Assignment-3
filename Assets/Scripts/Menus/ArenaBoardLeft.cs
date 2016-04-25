using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaBoardLeft : MonoBehaviour 
{
	int offset = 0;
	public GameObject arenaUnitPanel;

	public GameObject AddUnitPanel()
	{
		GameObject newPanel = (GameObject)Instantiate(
			arenaUnitPanel,
			transform.position + new Vector3(0,88+offset,0),
			Quaternion.identity);

		newPanel.transform.SetParent(gameObject.transform);

		offset -= 20;
		return newPanel;
	}
}
