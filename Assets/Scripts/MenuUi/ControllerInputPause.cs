using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PauseState
{
    None,
    Rising,
    Falling
}

[Serializable]
public class PauseCurve
{
    public AnimationCurve movementCurve;
    public AnimationCurve fadeCurve;
    public float duration;

    public PauseCurve() { }
    public PauseCurve(float _duration)
    {
        duration = _duration;
    }
}


public class ControllerInputPause : ControllerMenuInputGeneric
{
    public static ControllerInputPause instance;

    float progress;
    PauseState pauseState;
    bool paused;

    float cachedXPos;
    [SerializeField]
    new RectTransform transform;
    int screenHeight;

    [SerializeField]
    PauseCurve fallCurve = new PauseCurve(2);
    [SerializeField]
    PauseCurve riseCurve = new PauseCurve(0.3f);

    [SerializeField]
    CanvasGroup pauseFade;

    public Transform pauseMenuButtonsContainer;
    ControllerButtonPauseOptions options;
    public bool canPause;

    ControllerInputPause()
    {
        menuState = MenuState.Null;
    }

    private void Awake()
    {
        instance = this;
        screenHeight = Screen.height;
        transform = GetComponent<RectTransform>();
        cachedXPos = transform.anchoredPosition.x;
        transform.anchoredPosition = new Vector2(cachedXPos, screenHeight);
    }

    private void Start()
    {
        int setIndex = 0;
        foreach (Transform child in pauseMenuButtonsContainer)
        {
            ControllerButton button = child.GetComponent<ControllerButton>();
            if (button)
            {
                button.index = setIndex;
                activeMenuButtons.Add(button);
                setIndex++;
                if (button is ControllerButtonPauseOptions)
                    options = button as ControllerButtonPauseOptions;
            }
        }
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Pause") && canPause)
            TogglePause();

        switch (pauseState)
        {
            case PauseState.None:
                break;
            case PauseState.Rising:
                SetPosition(riseCurve);
                break;
            case PauseState.Falling:
                SetPosition(fallCurve);
                break;
        }

        base.Update();
    }

    void SetPosition(PauseCurve pauseCurve)
    {
        // Calculate how far into the curve we are
        progress = Mathf.Clamp(progress + (Time.unscaledDeltaTime / pauseCurve.duration), 0, 1);
        // Set the position
        transform.anchoredPosition = new Vector3(cachedXPos, pauseCurve.movementCurve.Evaluate(progress) * screenHeight);
        pauseFade.alpha = pauseCurve.fadeCurve.Evaluate(progress);
        Time.timeScale = pauseCurve.fadeCurve.Evaluate(1 - progress);
        // Stop if at completion
        if (progress == 1)
        {
            progress = 0;
            pauseState = PauseState.None;
            menuState = pauseCurve == fallCurve ? MenuState.Menu : MenuState.Null;
        }
    }

    void ChangeState(PauseState _state)
    {
        pauseState = _state;
        progress = 0;
        pauseFade.blocksRaycasts = ((int)_state - 1) == 1;
    }

    public void SetPaused(bool pauseState)
    {
        paused = pauseState;
        ChangeState(pauseState ? PauseState.Falling : PauseState.Rising);
        if (currentIndex != -1)
            CancelControllerInput(activeMenuButtons[currentIndex]);
    }

    public void TogglePause()
    {
        SetPaused(!paused);
    }

    public override void ExitSubMenu() {
        options.optionsElement.state = UIPanState.MovingOffscreen;
        activeSubMenu = null;
        SetSelectedButton(currentIndex);
        menuState = MenuState.Menu;
    }
}
