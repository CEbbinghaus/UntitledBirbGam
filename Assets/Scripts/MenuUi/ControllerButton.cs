using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ControllerButton : EventTrigger
{
	[HideInInspector]
	public int index;
	[HideInInspector]
	public Button button;
	//[HideInInspector]
	public TextMeshProUGUI buttonText;

	ControllerMenuInputGeneric controller;

	protected virtual void Start()
	{
		button = GetComponent<Button>();
		button.targetGraphic.color = button.colors.normalColor;
		button.onClick.AddListener(Press);
		buttonText = GetComponentInChildren<TextMeshProUGUI>();
		controller = FindObjectOfType<ControllerMenuInputGeneric>();
	}

	public virtual void Press()
	{
		buttonText.fontMaterial.DisableKeyword("GLOW_ON");
		EventSystem.current.SetSelectedGameObject(null);
		Debug.Log($"Pressed {this.GetType()}");
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (controller.menuState == MenuState.Menu)
		{
			base.OnPointerEnter(eventData);
			controller.SetSelectedButton(this);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (controller.menuState == MenuState.Menu)
		{
			base.OnPointerExit(eventData);
			controller.CancelControllerInput(this);
		}
	}
}
