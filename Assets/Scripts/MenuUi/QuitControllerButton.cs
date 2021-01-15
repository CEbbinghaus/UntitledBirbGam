using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitControllerButton : MenuButton
{
	public override void Press()
	{
		Application.Quit();
		MenuControllerInput.instance.menuState = MenuState.Null;
	}
}
