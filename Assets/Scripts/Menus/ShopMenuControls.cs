using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class ShopMenuControls : Interface 
{
	List<GameObject> sortedUnits;
	GameObject
		selectedPartButton,
		selectedPart;

	Text 
		descriptionText,
		currentPartName,
		currentPartWeight,
		currentPartAttribute,
		shopPartName,
		shopPartWeight,
		shopPartAttribute,
		shopPartCost;
		

	public int currentUser;

	// Use this for initialization
	void Start () 
	{
        InterfaceStart();

		//Locate text components
		descriptionText = GameObject.Find ("DescriptionPanel").GetComponentInChildren<Text>();

		currentPartName = GameObject.Find ("CurrentName").GetComponent<Text>();
		currentPartWeight = GameObject.Find ("CurrentWeight").GetComponent<Text>();
		currentPartAttribute = GameObject.Find ("CurrentAttribute").GetComponent<Text>();

		shopPartName = GameObject.Find ("ShopName").GetComponent<Text>();
		shopPartWeight = GameObject.Find ("ShopWeight").GetComponent<Text>();
		shopPartAttribute = GameObject.Find ("ShopAttribute").GetComponent<Text>();
		shopPartCost = GameObject.Find("PurchaseButton").GetComponentInChildren<Text>();

		//Sort units by bounty
		sortedUnits.AddRange(g.units);
		sortedUnits = sortedUnits.OrderBy(x=>x.GetComponent<Tank>().bounty).ToList();


	}

	void Update()
	{
		Pause ();
	}

	public void SelectPart()
	{
		selectedPartButton = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
		GameObject p = g.parts[int.Parse(selectedPartButton.name)];

		descriptionText.text = p.GetComponent<Part>().description;

		shopPartName.text = p.name;
		shopPartWeight.text = p.GetComponent<Part>().weight.ToString();
		shopPartAttribute.text = p.GetComponent<Part>().attribute.ToString();

		shopPartCost.text = "$ " + p.GetComponent<Part>().cost.ToString();
	}

	public void BuyPart()
	{
	}

	public void Pass()
	{
		
	}
		

}
