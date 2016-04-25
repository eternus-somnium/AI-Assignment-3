using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class ButtonListGenerator : Interface 
{
	public GameObject prototypeButton;


	public List<GameObject> populateButtonList()
	{
		int i;
		List<GameObject> partButtons = new List<GameObject>();

		for(i=0; i < g.units.Length*2;i++)
		{
			GameObject newButton = (GameObject)Instantiate(prototypeButton,gameObject.transform.position,Quaternion.identity);
			newButton.transform.SetParent(gameObject.transform);
			newButton.GetComponent<RectTransform>().position += new Vector3(0,-20-40*i,0);

			int partNumber = Random.Range(0,g.parts.Length);
			newButton.name = partNumber.ToString();
			newButton.transform.GetChild(0).GetComponent<Text>().text = g.parts[partNumber].name;

			newButton.GetComponent<Button>().interactable = true;

			partButtons.Add(newButton);
		}

		gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, -g.units.Length*60);
		prototypeButton.SetActive(false);

		return partButtons;
	}
}
