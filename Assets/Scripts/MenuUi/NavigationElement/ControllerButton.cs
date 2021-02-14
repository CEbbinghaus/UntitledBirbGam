using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ControllerButton : NavigationElement
{
	[HideInInspector]
	public Button button;
	[HideInInspector]
	public TextMeshProUGUI buttonText;

	protected virtual void Start()
	{
		button = GetComponent<Button>();
		print(button);
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

	public override void SelectElement()
	{
		buttonText.fontMaterial.EnableKeyword("GLOW_ON");
		button.Select();
	}

	public override void DeselectElement()
	{
		buttonText.fontMaterial.DisableKeyword("GLOW_ON");
	}
}
