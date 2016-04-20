using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : Interface 
{

	// Use this for initialization
	void Start () 
	{
		InterfaceStart();
	
	}

	public void BeginButton()
	{
		IndividualButtonSwitch();
	}
}
