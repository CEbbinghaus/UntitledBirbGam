﻿using System;
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
                    if (collected == 0) return;

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
                public Image fill;
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

    public ScreenOrientation DEBUGOrientation = ScreenOrientation.Portrait;

    public bool debugTouch = true;

    [Space(10)]
    public UIElements landscapeElements;
    public UIElements portraitElements;
    public UIElements activeElements;

    [Space(10)]
    public GameObject m_EndScreen = null;
    public UICounter m_FinalScore = null;

    // Cached values and elements
    ScreenOrientation cachedOrientation;
    PlayerManager playerManager;
    Image activeCapacityBar;

    private int cachedCollectedSeeds;
    private int cachedCollectedSandwiches;
    private int cachedRemainingLives;
    public int cachedScore;

    public int CachedCollectedSeeds { get => cachedCollectedSeeds; set { activeElements.foodElements.seeds.UpdateUI(value); cachedCollectedSeeds = value; } }
    public int CachedCollectedSandwiches { get => cachedCollectedSandwiches; set { activeElements.foodElements.sandwiches.UpdateUI(value); cachedCollectedSandwiches = value; } }
    public int CachedRemainingLives { get => cachedRemainingLives; set { activeElements.lifeElements.UpdateUI(value); cachedRemainingLives = value; } }

    // Events
    public static event OnChangeUIOrientation onChangeUIOrientation;
    public delegate void OnChangeUIOrientation(ScreenOrientation orientation);

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
    }

    // Start is called before the first frame update
    private void Start()
    {
        cachedOrientation = DEBUGOrientation;
        if (Input.touchSupported || debugTouch)
        {
            // Update the horizontal UI
            ApplySavedLandscapeUILayout();
            // Enable the pause button
            landscapeElements.pauseIcon.gameObject.SetActive(true);
            portraitElements.pauseIcon.gameObject.SetActive(true);

            // Set the UI Layout
            //switch (Screen.orientation)
            switch (DEBUGOrientation)
            {
                case ScreenOrientation.Portrait:
                    activeElements = portraitElements;
                    ChangeUIOrientation(portraitElements);
                    landscapeElements.container.SetActive(false);
                    break;
                case ScreenOrientation.LandscapeLeft:
                case ScreenOrientation.LandscapeRight:
                    activeElements = landscapeElements;
                    ChangeUIOrientation(landscapeElements);
                    portraitElements.container.SetActive(false);
                    break;
                default:
                    // Treat the game as if it is in landscape mode by default
                    goto case ScreenOrientation.LandscapeRight;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the UI elements if the orientation has changed to abother valid orientation
        //if (Screen.orientation != cachedOrientation)
        if (DEBUGOrientation != cachedOrientation)
        {
            //switch (Screen.orientation)
            switch (DEBUGOrientation)
            {
                case ScreenOrientation.Portrait:
                    ChangeUIOrientation(portraitElements);
                    break;
                case ScreenOrientation.LandscapeLeft:
                case ScreenOrientation.LandscapeRight:
                    ChangeUIOrientation(landscapeElements);
                    break;
            }
        }

        // Update the capacity bar if the target value is different
        if (playerManager && activeCapacityBar.fillAmount != Mathf.Min(playerManager.FoodEncumbrance(), 1))
        {
            activeCapacityBar.fillAmount = Mathf.Lerp(activeCapacityBar.fillAmount, Mathf.Min(playerManager.FoodEncumbrance(), 1), Time.deltaTime);
        }
    }

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

    private void ChangeUIOrientation(UIElements newElements)
    {
        cachedOrientation = DEBUGOrientation;
        activeElements.container.SetActive(false);

        // Syncing values
        newElements.foodElements.capacityBar.fill.fillAmount = activeElements.foodElements.capacityBar.fill.fillAmount;
        newElements.scoreText.QuickSetValue(cachedScore);
        newElements.foodElements.sandwiches.UpdateUI(CachedCollectedSandwiches);
        newElements.foodElements.seeds.UpdateUI(CachedCollectedSeeds);
        newElements.lifeElements.UpdateUI(CachedRemainingLives);


        activeElements = newElements;
        activeElements.container.SetActive(true);

        activeCapacityBar = activeElements.foodElements.capacityBar.fill;
        onChangeUIOrientation?.Invoke(cachedOrientation);
    }

    private void ApplySavedLandscapeUILayout()
    {
        RectTransform foodContainer = landscapeElements.foodElements.container;
        RectTransform capacityBar = landscapeElements.foodElements.capacityBar.container;
        RectTransform joystick = landscapeElements.joystickElements.container;

        // UI is set up to be right-joystick (1) by default.
        // Do this even if the game is in the portrait layout in case the player rotates their device.
        if (PlayerPrefs.GetInt("JoystickPosition") == 0)
        {
            foodContainer.pivot = new Vector2(1, 0);
            foodContainer.anchorMin = new Vector2(1, 0);
            foodContainer.anchorMax = new Vector2(1, 0);
            foodContainer.anchoredPosition = new Vector2(-40, 40);

            landscapeElements.foodElements.seeds.layoutGroup.childAlignment = TextAnchor.MiddleRight;
            landscapeElements.foodElements.sandwiches.layoutGroup.childAlignment = TextAnchor.MiddleRight;

            capacityBar.anchorMin = new Vector2(1, 0);
            capacityBar.anchorMax = new Vector2(1, 0);
            capacityBar.anchoredPosition = new Vector2(-600, 0);

            joystick.pivot = new Vector2(0, 0);
            joystick.anchorMin = new Vector2(0, 0);
            joystick.anchorMax = new Vector2(0, 0);
            joystick.anchoredPosition = new Vector2(150, 125);
        }
        else if (PlayerPrefs.GetInt("JoystickPosition") == 1)
        {
            foodContainer.pivot = new Vector2(0, 0);
            foodContainer.anchorMin = new Vector2(0, 0);
            foodContainer.anchorMax = new Vector2(0, 0);
            foodContainer.anchoredPosition = new Vector2(40, 40);

            landscapeElements.foodElements.seeds.layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            landscapeElements.foodElements.sandwiches.layoutGroup.childAlignment = TextAnchor.MiddleLeft;

            capacityBar.anchorMin = new Vector2(0, 0);
            capacityBar.anchorMax = new Vector2(0, 0);
            capacityBar.anchoredPosition = new Vector2(0, 0);

            joystick.pivot = new Vector2(1, 0);
            joystick.anchorMin = new Vector2(1, 0);
            joystick.anchorMax = new Vector2(1, 0);
            joystick.anchoredPosition = new Vector2(-150, 125);
        }
    }
}
