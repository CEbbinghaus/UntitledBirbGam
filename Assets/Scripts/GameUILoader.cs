using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUILoader : MonoBehaviour
{
	[SerializeField]
	RectTransform foodContainer;
	[SerializeField]
	List<HorizontalLayoutGroup> pickupContainers;
	[SerializeField]
	public RectTransform capacityBar;

	[SerializeField]
	public RectTransform joystick;

	[SerializeField]
	public RectTransform pauseButton;

	private void Start()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld || Application.isEditor)
		{
			if (PlayerPrefs.GetInt("JoystickPosition") == 0)
			{
				print("Swapping UI");
				foodContainer.pivot = new Vector2(1, 0);
				foodContainer.anchorMin = new Vector2(1, 0);
				foodContainer.anchorMax = new Vector2(1, 0);
				foodContainer.anchoredPosition = new Vector2(-40, 40);
				pickupContainers.ForEach(c => c.childAlignment = TextAnchor.MiddleRight);
				foodContainer.pivot = new Vector2(1, 0);
				capacityBar.anchorMin = new Vector2(1, 0);
				capacityBar.anchorMax = new Vector2(1, 0);
				capacityBar.anchoredPosition = new Vector2(-600, 0);

				joystick.pivot = new Vector2(0, 0);
				joystick.anchorMin = new Vector2(0, 0);
				joystick.anchorMax = new Vector2(0, 0);
				joystick.anchoredPosition = new Vector2(150, 125);
			}
			pauseButton.gameObject.SetActive(true);
		}
	}
}
