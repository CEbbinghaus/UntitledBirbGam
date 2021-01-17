using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonOptions : ControllerButton
{

	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = UIPan.instance.options;
		UIPan.instance.ChangeState(UIPan.instance.options, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}