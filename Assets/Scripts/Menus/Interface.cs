using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interface : MonoBehaviour 
{
	public static GameManager g;
	public static bool paused;
	public static GameObject 
		previousPanel,
		previousButton,
		activePanel,
		pausePanel,
		eventSystem;

	Color 
		buttonOn = new Color(30/255f,160/255f,30/255f,1), 
		buttonOff = Color.white;

	// Use this for initialization
	public void InterfaceStart () 
	{
		if(g == null)
			g = GameObject.Find("GameManager").GetComponent<GameManager>();
		eventSystem = GameObject.Find("EventSystem");
	}

	public void Pause()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if(activePanel != previousPanel && previousPanel != null)
			{
				Time.timeScale = 1;
				closePanel();
			}
			else
			{
				Time.timeScale = 0;
				if(pausePanel != null)
					viewPanel(pausePanel);
			}
		}
	}

	//Sets the colors of grouped buttons
	public void GroupedButtonSwitch()
	{
		GameObject button = EventSystem.current.currentSelectedGameObject;
		GameObject panel = button.transform.parent.gameObject;

		if(button.GetComponent<Image>().color == buttonOff)
		{
			//Sets each button to button off color
			foreach(Button b in panel.GetComponentsInChildren<Button>())
			{
				SwitchButtonColor(b.gameObject, false);
			}

			SwitchButtonColor(button,true);
		}
	}

	public void IndividualButtonSwitch()
	{
		GameObject button = EventSystem.current.currentSelectedGameObject;
		SwitchButtonColor(button,!(button.GetComponent<Image>().color == buttonOn));
	}

	public void SwitchButtonColor(GameObject button, bool on)
	{
		Image i = button.GetComponent<Image>();

		if(on)
		{
			i.color = buttonOn;
		}
		else
		{
			i.color = buttonOff;
		}
	}

	//Sets the colors of grouped buttons
	public void TabSwitch(GameObject activatedPanel)
	{
		GameObject tab = EventSystem.current.currentSelectedGameObject;
		GameObject tabParent = tab.transform.parent.gameObject;

		if(tab.GetComponent<Image>().color == buttonOff)
		{
			//Sets each button to button off color
			foreach(Button b in tabParent.GetComponentsInChildren<Button>())
			{
				SwitchButtonColor(b.gameObject, false);
			}
			SwitchButtonColor(tab,true);

			//Switch each tab to inactive
			foreach(CanvasGroup c in activatedPanel.transform.parent.gameObject.GetComponentsInChildren<CanvasGroup>())
			{
				c.gameObject.SetActive(false);
			}
			activatedPanel.SetActive(true);
		}
	}


	public void viewPanel(GameObject panel)
	{
		previousPanel = activePanel;
		previousButton = EventSystem.current.currentSelectedGameObject;

		if(previousPanel != null)
			previousPanel.GetComponent<CanvasGroup>().interactable = false;

		activePanel = panel;
		activePanel.SetActive(true);
		activePanel.GetComponent<CanvasGroup>().interactable = true;


		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(
			panel.transform.GetChild(activePanel.transform.childCount-1).gameObject);
	}

	public void closePanel()
	{
		activePanel.SetActive(false);
		activePanel = previousPanel;
		if(activePanel != null)
		{
			activePanel.GetComponent<CanvasGroup>().interactable = true;

			eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(
				previousButton);
		}
	}
}
