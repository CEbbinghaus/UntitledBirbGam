using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonCredits : ControllerButton
{
	public GameObject creditsObject;

	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = creditsObject;
		creditsObject.SetActive(true);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}
