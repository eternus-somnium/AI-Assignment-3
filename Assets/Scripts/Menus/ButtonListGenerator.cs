using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ButtonListGenerator : Interface 
{
	public GameObject prototypeButton;


	// Use this for initialization
	void Start () 
	{
		populateButtonList();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void populateButtonList()
	{
		int i;
		//GetComponent<RectTransform>().
		for(i=0; i < g.units.Length * 2;i++)
		{
			GameObject newButton = (GameObject)Instantiate(prototypeButton,gameObject.transform.position,Quaternion.identity);
			newButton.transform.SetParent(gameObject.transform);
			newButton.GetComponent<RectTransform>().position += new Vector3(0,-50-40*i,0);

			newButton.name = g.parts[Random.Range(0,g.parts.Length-1)].name;
			newButton.transform.GetChild(0).GetComponent<Text>().text = newButton.name;

			newButton.GetComponent<Button>().interactable = true;
		}
	}
}
