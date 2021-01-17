using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonCredits : ControllerButton
{
	UIPan UIPan;
	public UIPanElement creditsElement;

	// Copy the info into the UIPan script
	private void Awake()
	{
		UIPan = FindObjectOfType<UIPan>();
		UIPan.options = creditsElement;
	}

	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = creditsElement;
		UIPan.ChangeState(UIPan.options, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}
