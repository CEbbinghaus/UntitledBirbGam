using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsControllerButton : MenuButton
{
	public GameObject creditsObject;

	public override void Press()
	{
		MenuControllerInput.instance.activeSubMenu = creditsObject;
		creditsObject.SetActive(true);
		MenuControllerInput.instance.menuState = MenuState.SubMenu;
	}
}
