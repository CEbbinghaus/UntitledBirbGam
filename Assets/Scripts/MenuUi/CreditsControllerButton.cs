using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsControllerButton : MenuButton
{
	public GameObject creditsObject;

	public override void Press()
	{
		creditsObject.SetActive(true);
	}
}
