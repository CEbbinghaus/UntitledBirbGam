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
        }
        [Serializable] public class FoodElements
        {
            [Serializable] public class FoodHolder
            {
                public RectTransform container;
                public HorizontalLayoutGroup layoutGroup;
                public List<Image> icons = new List<Image>(5);
                public TextMeshProUGUI overflowCounter;
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

        public GameObject container;
        public RectTransform pauseIcon;
        public UICounter scoreText;
        public RectTransform joystick;
        public LifeElements lifeElements;
        public FoodElements foodElements;
    }

    public static UIManager instance;

    public bool debugTouch = true;

    [Space(10)]
    public UIElements landscapeElements;
    public UIElements portraitElements;
    public UIElements activeElements;

    [Space(10)]
    public GameObject m_EndScreen = null;
    public UICounter m_FinalScore = null;

    // Cached values and elements
    DeviceOrientation cachedOrientation;
    PlayerManager playerManager;
    Image activeCapacityBar;

    private void Awake()
    {
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
        cachedOrientation = Input.deviceOrientation;
        if (Input.touchSupported || debugTouch)
        {
            // Update the horizontal UI
            ApplySavedLandscapeUILayout();
            // Enable the pause button
            landscapeElements.pauseIcon.gameObject.SetActive(true);
            //portraitElements.pauseIcon.gameObject.SetActive(true);
            
            // Set the UI Layout
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                    activeElements = portraitElements;
                    ChangeUIOrientation(portraitElements);
                    portraitElements.container.SetActive(true);
                    break;
                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
                    activeElements = landscapeElements;
                    ChangeUIOrientation(landscapeElements);
                    landscapeElements.container.SetActive(true);
                    break;
                default:
                    // Treat the game as if it is in landscape mode by default
                    goto case DeviceOrientation.LandscapeRight;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the UI elements if the orientation has changed to abother valid orientation
        if (Input.deviceOrientation != cachedOrientation)
        {
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                    ChangeUIOrientation(portraitElements);
                    break;
                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
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

    public void UpdateLivesUI(int livesRemaining)
    {
        for (int i = livesRemaining; i < 3; i++)
        {
            activeElements.lifeElements.icons[i].enabled = false;
        }
    }

    public void UpdateFoodUI(UIElements.FoodElements.FoodHolder holder, int collected)
    {
        if (collected == 0)
        {

            return;
        }
        if (collected < 5)
        {
            for (int i = 1; i < 5; i++)
            {
                holder.icons[collected - 1].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i < 5; i++)
            {
                holder.icons[i].gameObject.SetActive(false);
            }
            holder.overflowCounter.gameObject.SetActive(true);
            holder.overflowCounter.text = $"x{collected}";
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
        cachedOrientation = Input.deviceOrientation;
        activeElements.container.SetActive(false);
        activeElements = newElements;
        activeElements.container.SetActive(true);

        activeCapacityBar = activeElements.foodElements.capacityBar.fill;
    }

    private void ApplySavedLandscapeUILayout()
    {
        // UI is set up to be right-joystick (1) by default. Swaps to left-joystick (0) if the PlayerPrefs calls for it.
        // Do this even if the game is in the vertical layout in case the player rotates their device.
        if (PlayerPrefs.GetInt("JoystickPosition") == 0)
        {
            RectTransform foodContainer = landscapeElements.foodElements.container;
            foodContainer.pivot = new Vector2(1, 0);
            foodContainer.anchorMin = new Vector2(1, 0);
            foodContainer.anchorMax = new Vector2(1, 0);
            foodContainer.anchoredPosition = new Vector2(-40, 40);

            landscapeElements.foodElements.seeds.layoutGroup.childAlignment = TextAnchor.MiddleRight;
            landscapeElements.foodElements.sandwiches.layoutGroup.childAlignment = TextAnchor.MiddleRight;

            RectTransform capacityBar = landscapeElements.foodElements.capacityBar.container;
            capacityBar.anchorMin = new Vector2(1, 0);
            capacityBar.anchorMax = new Vector2(1, 0);
            capacityBar.anchoredPosition = new Vector2(-600, 0);

            RectTransform joystick = landscapeElements.joystick;
            joystick.pivot = new Vector2(0, 0);
            joystick.anchorMin = new Vector2(0, 0);
            joystick.anchorMax = new Vector2(0, 0);
            joystick.anchoredPosition = new Vector2(150, 125);
        }
    }
}
