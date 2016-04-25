using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class ShopMenuControls : Interface 
{
	public List<GameObject> 
		sortedUnits,
		availableParts;
	public GameObject
		blockingPanel,
		partsContent,
		selectedPartButton,
		selectedPart,
		currentPart,
		purchaseButton,
		passButton;

	Text 
		descriptionText,
		currentPartName,
		currentPartWeight,
		currentPartAttribute,
		selectedPartName,
		selectedPartWeight,
		selectedPartAttribute,
		selectedPartCost,
		unitName,
		funds;
		

	int currentUnit;

	// Use this for initialization
	void Start () 
	{
        InterfaceStart();

		blockingPanel = GameObject.Find("BlockingPanel");
		partsContent = GameObject.Find("PartsContent");
		purchaseButton = GameObject.Find("PurchaseButton");
		passButton = GameObject.Find("PassButton");

		LocateTextObjects();

		availableParts = partsContent.GetComponent<ButtonListGenerator>().populateButtonList();

		//Sort units by bounty
		sortedUnits = new List<GameObject>();
		sortedUnits.AddRange(g.units);
		sortedUnits = sortedUnits.OrderBy(x=>x.GetComponent<Tank>().bounty).ToList();

		PopulateCurrentUnitData();
		ActivatePartSelection();

	}

	void Update()
	{
		Pause ();
	}

	void LocateTextObjects()
	{
		//Locate text components
		descriptionText = GameObject.Find ("DescriptionPanel/Scroll View/Viewport/Content").GetComponentInChildren<Text>();

		currentPartName = GameObject.Find ("CurrentName").GetComponent<Text>();
		currentPartWeight = GameObject.Find ("CurrentWeight").GetComponent<Text>();
		currentPartAttribute = GameObject.Find ("CurrentAttribute").GetComponent<Text>();

		selectedPartName = GameObject.Find ("ShopName").GetComponent<Text>();
		selectedPartWeight = GameObject.Find ("ShopWeight").GetComponent<Text>();
		selectedPartAttribute = GameObject.Find ("ShopAttribute").GetComponent<Text>();
		selectedPartCost = purchaseButton.GetComponentInChildren<Text>();
		unitName = GameObject.Find("UnitName").GetComponent<Text>();
		funds = GameObject.Find("Funds").GetComponent<Text>();
	}

	public void SelectPart()
	{
		selectedPartButton = eventSystem.GetComponent<EventSystem>().currentSelectedGameObject;
		selectedPart = g.parts[int.Parse(selectedPartButton.name)];

		switch (selectedPart.tag)
		{
		case "Body":
			currentPart = sortedUnits[currentUnit].GetComponent<Tank>().bodySchematic;
			break;
		case "Track":
			currentPart = sortedUnits[currentUnit].GetComponent<Tank>().trackSchematic;
			break;
		case "MainWeapon":
			currentPart = sortedUnits[currentUnit].GetComponent<Tank>().mainWeaponSchematic;
			break;
		case "SecondaryWeapon":
			currentPart = sortedUnits[currentUnit].GetComponent<Tank>().secondaryWeaponSchematic;
			break;
		case "Accessory":
			currentPart = sortedUnits[currentUnit].GetComponent<Tank>().accessorySchematic;
			break;
		}

		PopulateDisplayData(false);
		purchaseButton.GetComponent<Button>().interactable = 
			(selectedPart.GetComponent<Part>().cost <= sortedUnits[currentUnit].GetComponent<Tank>().funds &&
				selectedPartName.text != currentPartName.text);
			
	}

	public void BuyPart()
	{
		//If there is a part selected AND the current unit can afford the part And the current unit doesnt already have the selected part
		if(selectedPart != null)
		{
			//Equip the part
			switch (selectedPart.tag)
			{
			case "Body":
				sortedUnits[currentUnit].GetComponent<Tank>().bodySchematic = selectedPart;
				break;
			case "Track":
				sortedUnits[currentUnit].GetComponent<Tank>().trackSchematic = selectedPart;
				break;
			case "MainWeapon":
				sortedUnits[currentUnit].GetComponent<Tank>().mainWeaponSchematic = selectedPart;
				break;
			case "SecondaryWeapon":
				sortedUnits[currentUnit].GetComponent<Tank>().secondaryWeaponSchematic = selectedPart;
				break;
			case "Accessory":
				sortedUnits[currentUnit].GetComponent<Tank>().accessorySchematic = selectedPart;
				break;
			}

			//Update comparison panel
			currentPart = selectedPart;
			PopulateDisplayData(false);

			//Charge the current unit
			sortedUnits[currentUnit].GetComponent<Tank>().funds -= selectedPart.GetComponent<Part>().cost;
			PopulateCurrentUnitData();

			//Deactivate the selected part button and purchase button
			selectedPartButton.GetComponent<Button>().interactable = false;
			purchaseButton.GetComponent<Button>().interactable = false;
		}
	}

	public void Pass()
	{
		purchaseButton.GetComponent<Button>().interactable = false;
		currentUnit++;
		if(currentUnit == sortedUnits.Count)
			g.LoadLevel(1);
		else
		{
			PopulateCurrentUnitData();
			PopulateDisplayData(true);
			ActivatePartSelection();
		}
	}
		
	void PopulateCurrentUnitData()
	{
		//Populate first unit information
		unitName.text = sortedUnits[currentUnit].name;
		funds.text = sortedUnits[currentUnit].GetComponent<Tank>().funds.ToString();
	}

	void PopulateDisplayData(bool clear)
	{
		if(clear)
		{
			descriptionText.text = "-";

			currentPartName.text = "-";
			currentPartWeight.text = "-";
			currentPartAttribute.text = "-";

			selectedPartName.text = "-";
			selectedPartWeight.text = "-";
			selectedPartAttribute.text = "-";

			selectedPartCost.text = "$ 0";
		}
		else
		{
			descriptionText.text = selectedPart.GetComponent<Part>().description;

			if(currentPart != null)
			{
				currentPartName.text = currentPart.name;
				currentPartWeight.text = currentPart.GetComponent<Part>().weight.ToString();
				currentPartAttribute.text = currentPart.GetComponent<Part>().attribute.ToString();
			}

			selectedPartName.text = selectedPart.name;
			selectedPartWeight.text = selectedPart.GetComponent<Part>().weight.ToString();
			selectedPartAttribute.text = selectedPart.GetComponent<Part>().attribute.ToString();

			selectedPartCost.text = "$ " + selectedPart.GetComponent<Part>().cost.ToString();
		}
	}

	void ActivatePartSelection()
	{
		AI i;
		if((i = sortedUnits[currentUnit].GetComponent<AI>()) == null)
			blockingPanel.SetActive(false);
		else
		{
			blockingPanel.SetActive(false);
			List<int> chosenParts = i.ChoosePart(availableParts);
			foreach(int part in chosenParts)
			{
				EventSystem.current.SetSelectedGameObject(availableParts[part]);

				SelectPart();
				BuyPart();
			}

			availableParts.RemoveAll(x=>x.GetComponent<Button>().IsInteractable() == false);

			Pass();
		}
	}

}
