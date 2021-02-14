using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonQuit : ControllerButton
{
	public override void Press()
	{
		base.Press();
		Application.Quit();
	}
}
