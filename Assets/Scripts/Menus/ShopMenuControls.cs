using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopMenuControls : Interface 
{
	GameObject 
		mainPanel,
		onlinePanel, 
		notecardPanel,
		slideshowPanel,
		timerPanel;

	// Use this for initialization
	void Start () 
	{
        InterfaceStart();

		//Highlight default options
		//SwitchButtonColor(GameObject.Find("Practice"),true);
		//SwitchButtonColor(GameObject.Find("Classroom"),true);

		//Locate panels
		mainPanel = GameObject.Find ("MainPanel");
		onlinePanel = GameObject.Find ("OnlinePanel");
		notecardPanel = GameObject.Find ("NotecardPanel");
		slideshowPanel = GameObject.Find ("SlideshowPanel");
		timerPanel = GameObject.Find ("TimerPanel");
		activePanel = mainPanel;

		//Hide option panels
		if(onlinePanel != null) onlinePanel.SetActive (false);
		if(notecardPanel != null) notecardPanel.SetActive (false);
		if(slideshowPanel != null) slideshowPanel.SetActive (false);
		if(timerPanel != null) timerPanel.SetActive (false);
	}

	void Update()
	{
		Pause ();
	}

//Main menu
	public void BeginButton(string scene)
	{
		IndividualButtonSwitch();
		SceneManager.LoadScene(scene);
	}
}
