using UnityEngine;
using System.Collections;

public class LeftPanel : MonoBehaviour 
{
	int offset = 0;
	public GameObject prototypePanel;

	void Start()
	{
	}

	public GameObject AddUnitPanel()
	{
		GameObject newPanel = (GameObject)Instantiate(
			prototypePanel,
			transform.position + new Vector3(0,88+offset,0),
			Quaternion.identity);

		newPanel.transform.SetParent(gameObject.transform);

		offset -= 20;
		return newPanel;
	}
}
