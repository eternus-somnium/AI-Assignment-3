using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ResultMenuControls : Interface 
{
	GameObject 
	mainPanel,
	eyelinePanel, 
	notecardPanel,
	slideshowPanel,
	timerPanel;

	// Use this for initialization
	void Start () 
	{
		InterfaceStart();

		//Highlight default options
		TabSwitch(GameObject.Find("EyelinePanel"));
		//SwitchButtonColor(GameObject.Find("Eyeline"),true);

	}

	void Update()
	{
		Pause ();
	}

	//Main menu
	public void MainMenuButton()
	{
		IndividualButtonSwitch();
		SceneManager.LoadScene(0);
	}
}

