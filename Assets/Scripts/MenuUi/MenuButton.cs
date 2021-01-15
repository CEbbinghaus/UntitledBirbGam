using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour
{
	public int index;
	//[HideInInspector]
	public Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.targetGraphic.color = button.colors.normalColor;
	}

	public abstract void Press(); 
}
