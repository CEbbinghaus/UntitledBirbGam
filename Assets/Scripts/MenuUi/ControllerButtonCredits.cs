using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerButtonCredits : ControllerButton
{
	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = UIPan.instance.credits;
		UIPan.instance.ChangeState(UIPan.instance.credits, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}
