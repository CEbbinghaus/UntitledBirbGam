using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ControllerButton : EventTrigger
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

	public virtual void Press()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		ControllerInputMenu.instance.SetHoverButton(this);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		ControllerInputMenu.instance.CancelControllerInput(this);
	}
}
