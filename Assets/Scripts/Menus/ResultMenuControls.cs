using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ResultMenuControls : Interface 
{
	public GameObject resultsUnitPanel;
	List<GameObject> sortedUnits;
	int offset = 0;

	// Use this for initialization
	void Start () 
	{
		InterfaceStart();

		//Sort units by bounty
		sortedUnits = new List<GameObject>();
		sortedUnits.AddRange(g.units);
		sortedUnits = sortedUnits.OrderByDescending(x=>x.GetComponent<Tank>().bounty).ToList();

		//Display sorted list

		foreach(GameObject unit in sortedUnits)
		{
			createEntry(unit.GetComponent<Tank>());
		}
	}
		
	void createEntry(Tank unit)
	{
		GameObject newEntry = (GameObject)Instantiate(
			resultsUnitPanel,
			transform.position + new Vector3(0,75+offset,0),
			Quaternion.identity);

		newEntry.transform.SetParent(gameObject.transform);
		newEntry.GetComponentsInChildren<Image>()[1].color = unit.color;
		newEntry.GetComponentsInChildren<Text>()[0].text = unit.name;
		newEntry.GetComponentsInChildren<Text>()[1].text = unit.bounty.ToString();
		newEntry.GetComponentsInChildren<Text>()[2].text = unit.funds.ToString();

		offset -= 20;
	}

	//Main menu
	public void MainMenuButton()
	{
		IndividualButtonSwitch();
		SceneManager.LoadScene(0);
	}
}

