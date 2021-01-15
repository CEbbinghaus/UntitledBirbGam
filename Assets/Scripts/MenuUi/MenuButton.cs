using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour
{
	public int index;

	public Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
	}
	public abstract void Press(); 
}
