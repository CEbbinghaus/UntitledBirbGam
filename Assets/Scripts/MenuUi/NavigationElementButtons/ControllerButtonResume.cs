﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonResume : ControllerButton
{
	public override void Press()
	{
		base.Press();
		ControllerInputPause.instance.SetPaused(false);
	}
}
