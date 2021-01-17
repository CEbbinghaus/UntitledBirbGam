using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonOptions : ControllerButton
{
	UIPan UIPan;
	public UIPanElement optionsElement;

	// Copy the info into the UIPan script
	private void Awake()
	{
		UIPan = FindObjectOfType<UIPan>();
		UIPan.options = optionsElement;
	}

	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = optionsElement;
		UIPan.ChangeState(UIPan.options, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}