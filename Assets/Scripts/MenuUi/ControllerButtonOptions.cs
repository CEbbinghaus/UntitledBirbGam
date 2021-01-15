using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonOptions : ControllerButton
{
	public GameObject optionsObject;

	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = optionsObject;
		optionsObject.SetActive(true);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}
