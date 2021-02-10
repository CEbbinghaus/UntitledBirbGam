using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Serializable] public class UIElements
	{
		[Serializable] public class LifeElements
		{
			public RectTransform container;
			public List<Image> icons = new List<Image>();

			public void UpdateUI(int livesRemaining)
			{
				for (int i = livesRemaining; i < 3; i++)
				{
					icons[i].enabled = false;
				}
			}
		}

		[Serializable] public class FoodElements
		{
			[Serializable] public class FoodHolder
			{
				public RectTransform container;
				public HorizontalLayoutGroup layoutGroup;
				public List<Image> icons = new List<Image>(5);
				public TextMeshProUGUI overflowCounter;

				public void UpdateUI(int collected)
				{
					if (collected == 0)return;

					int maxSprites = icons.Count;

					// Draw sprites equal to the number of items
					if (collected < maxSprites)
					{
						for (int i = 1; i < maxSprites; i++)
						{
							icons[collected - 1].gameObject.SetActive(true);
						}
					}
					// Draw the shorthand x[number] version 
					else
					{
						icons[0].gameObject.SetActive(true);
						for (int i = 1; i < maxSprites; i++)
						{
							icons[i].gameObject.SetActive(false);
						}
						overflowCounter.gameObject.SetActive(true);
						overflowCounter.text = $"x{collected}";
					}
				}
			}

			[Serializable] public class FoodCapacityBar
			{
				public RectTransform container;
				public CapacityBar bar;
			}

			public RectTransform container;
			public FoodHolder seeds;
			public FoodHolder sandwiches;
			public FoodCapacityBar capacityBar;
		}

		[Serializable] public class JoystickElements
		{
			public RectTransform container;
			public bl_Joystick joystick;
		}
		public GameObject container;
		public RectTransform pauseIcon;
		public UICounter scoreText;
		public JoystickElements joystickElements;
		public LifeElements lifeElements;
		public FoodElements foodElements;
	}

	public static UIManager instance;

	[Space(10)]
	public UIElements landscapeElements;
	public UIElements portraitElements;
	public UIElements activeElements;

	[Space(10)]
	public GameObject m_EndScreen = null;
	public UICounter m_FinalScore = null;

	// Cached values and elements
	PlayerManager playerManager;

	private int cachedCollectedSeeds = 0;
	private int cachedCollectedSandwiches = 0;
	private int cachedRemainingLives = 3;
	public int cachedScore = 0;

	public int CachedCollectedSeeds { get => cachedCollectedSeeds; set { activeElements.foodElements.seeds.UpdateUI(value); cachedCollectedSeeds = value; } }
	public int CachedCollectedSandwiches { get => cachedCollectedSandwiches; set { activeElements.foodElements.sandwiches.UpdateUI(value); cachedCollectedSandwiches = value; } }
	public int CachedRemainingLives { get => cachedRemainingLives; set { activeElements.lifeElements.UpdateUI(value); cachedRemainingLives = value; } }

	private void Awake()
	{
		// Singleton
		if (instance)
		{
			this.enabled = false;
		}
		else
		{
			instance = this;
		}

		playerManager = FindObjectOfType<PlayerManager>();
		if (playerManager == null)
		{
			Debug.LogError("No player found!");
		}

		//OrientationManager.onChangeUIOrientation += UpdateOrientation;
	}

	// Start is called before the first frame update
	private void Start()
	{
		if (Input.touchSupported)
		{
			// Update the horizontal UI
			ApplySavedLandscapeUILayout();
			// Enable the pause button
			landscapeElements.pauseIcon.gameObject.SetActive(true);
			portraitElements.pauseIcon.gameObject.SetActive(true);

			// Set the UI Layout
			// 	switch (Screen.orientation)
			// 	//switch (DEBUGOrientation)
			// 	{
			// 		case ScreenOrientation.Portrait:
			// 			activeElements = portraitElements;
			// 			UpdateOrientation(ScreenOrientation.Portrait);
			// 			landscapeElements.container.SetActive(false);
			// 			break;
			// 		case ScreenOrientation.LandscapeLeft:
			// 		case ScreenOrientation.LandscapeRight:
			activeElements = landscapeElements;
			portraitElements.container.SetActive(false);
			// 			UpdateOrientation(ScreenOrientation.Landscape);
			// 			break;
			// 		default:
			// 			// Treat the game as if it is in landscape mode by default
			// 			goto case ScreenOrientation.LandscapeRight;
			// 	}
		}
		activeElements = landscapeElements;
		portraitElements.container.SetActive(false);
	}

	// public void UpdateOrientation(ScreenOrientation orientation)
	// {
	// 	UIElements newElements;

	// 	switch (Screen.orientation)
	// 	{
	// 		case ScreenOrientation.Portrait:
	// 			newElements = portraitElements;
	// 			break;
	// 		case ScreenOrientation.LandscapeLeft:
	// 		case ScreenOrientation.LandscapeRight:
	// 			newElements = landscapeElements;
	// 			break;
	// 		default:
	// 			return;
	// 	}

	// 	activeElements.container.SetActive(false);

	// 	// Syncing values
	// 	newElements.foodElements.capacityBar.bar.image.fillAmount = activeElements.foodElements.capacityBar.bar.image.fillAmount;
	// 	newElements.scoreText.QuickSetValue(cachedScore);
	// 	newElements.foodElements.sandwiches.UpdateUI(CachedCollectedSandwiches);
	// 	newElements.foodElements.seeds.UpdateUI(CachedCollectedSeeds);
	// 	newElements.lifeElements.UpdateUI(CachedRemainingLives);

	// 	activeElements = newElements;
	// 	activeElements.container.SetActive(true);
	// }

	public void ClearFoodUI()
	{
		activeElements.foodElements.sandwiches.icons.ForEach(image => image.gameObject.SetActive(false));
		activeElements.foodElements.sandwiches.overflowCounter.gameObject.SetActive(false);
		activeElements.foodElements.seeds.icons.ForEach(image => image.gameObject.SetActive(false));
		activeElements.foodElements.seeds.overflowCounter.gameObject.SetActive(false);
	}

	public void DisplayEndScreen(int score)
	{
		m_EndScreen.SetActive(true);
		m_FinalScore.SetValue(score);
		Time.timeScale = 0.0f;
	}

	/// <summary>
	/// Updates which side the joystick is on
	/// </summary>
	private void ApplySavedLandscapeUILayout()
	{
		RectTransform foodContainer = landscapeElements.foodElements.container;
		RectTransform capacityBar = landscapeElements.foodElements.capacityBar.container;
		RectTransform joystick = landscapeElements.joystickElements.container;

		// UI is set up to be right-joystick (1) by default.
		// Do this even if the game is in the portrait layout in case the player rotates their device.
		if (PlayerPrefs.GetInt("JoystickPosition") == 0)
		{
			foodContainer.pivot = Vector2.right;
			foodContainer.anchorMin = Vector2.right;
			foodContainer.anchorMax = Vector2.right;

			foodContainer.anchoredPosition = new Vector2(-40, 40);

			landscapeElements.foodElements.seeds.layoutGroup.childAlignment = TextAnchor.MiddleRight;
			landscapeElements.foodElements.sandwiches.layoutGroup.childAlignment = TextAnchor.MiddleRight;

			capacityBar.anchorMin = Vector2.right;
			capacityBar.anchorMax = Vector2.right;
			capacityBar.anchoredPosition = new Vector2(-600, 0);

			joystick.pivot = Vector2.zero;
			joystick.anchorMin = Vector2.zero;
			joystick.anchorMax = Vector2.zero;
			joystick.anchoredPosition = new Vector2(150, 125);
		}
		else if (PlayerPrefs.GetInt("JoystickPosition") == 1)
		{
			foodContainer.pivot = Vector2.zero;
			foodContainer.anchorMin = Vector2.zero;
			foodContainer.anchorMax = Vector2.zero;
			foodContainer.anchoredPosition = new Vector2(40, 40);

			landscapeElements.foodElements.seeds.layoutGroup.childAlignment = TextAnchor.MiddleLeft;
			landscapeElements.foodElements.sandwiches.layoutGroup.childAlignment = TextAnchor.MiddleLeft;

			capacityBar.anchorMin = Vector2.zero;
			capacityBar.anchorMax = Vector2.zero;
			capacityBar.anchoredPosition = Vector2.zero;

			joystick.pivot = Vector2.right;
			joystick.anchorMin = Vector2.right;
			joystick.anchorMax = Vector2.right;
			joystick.anchoredPosition = new Vector2(-150, 125);
		}
	}
}
