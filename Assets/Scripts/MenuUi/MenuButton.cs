using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuButton : EventTrigger
{
	[HideInInspector]
	public int index;
	[HideInInspector]
	public Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.targetGraphic.color = button.colors.normalColor;
		button.onClick.AddListener(Press);
	}

	public abstract void Press();

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		MenuControllerInput.instance.SetHoverButton(this);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		MenuControllerInput.instance.CancelControllerInput(this);
	}
}
