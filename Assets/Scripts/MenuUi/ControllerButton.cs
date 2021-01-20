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
	[HideInInspector]
	public TextMeshProUGUI buttonText;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.targetGraphic.color = button.colors.normalColor;
		button.onClick.AddListener(Press);
		buttonText = GetComponentInChildren<TextMeshProUGUI>();
	}

	public virtual void Press()
	{
		buttonText.fontMaterial.DisableKeyword("GLOW_ON");
		EventSystem.current.SetSelectedGameObject(null);
		Debug.Log($"Pressed {this.GetType()}");
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (ControllerInputMenu.instance.menuState == MenuState.Menu)
		{
			base.OnPointerEnter(eventData);
			ControllerInputMenu.instance.SetSelectedButton(this);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (ControllerInputMenu.instance.menuState == MenuState.Menu)
		{
			base.OnPointerExit(eventData);
			ControllerInputMenu.instance.CancelControllerInput(this);
		}
	}
}
