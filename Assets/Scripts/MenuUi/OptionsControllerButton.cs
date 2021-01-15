using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsControllerButton : MenuButton
{
	public GameObject optionsObject;

	public override void Press()
	{
		MenuControllerInput.instance.activeSubMenu = optionsObject;
		optionsObject.SetActive(true);
		MenuControllerInput.instance.menuState = MenuState.SubMenu;
	}
}
