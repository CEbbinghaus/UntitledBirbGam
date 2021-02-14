using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonMenuClose : ControllerButton
{
	public override void Press()
	{
		base.Press();
		MenuInputManager.instance.CloseMenu();
	}
}
