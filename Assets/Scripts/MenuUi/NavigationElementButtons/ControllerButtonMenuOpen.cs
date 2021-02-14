using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonMenuOpen : ControllerButton
{
	public Menu menu;

	public override void Press()
	{
		base.Press();
		MenuInputManager.instance.OpenMenu(menu);
	}
}
