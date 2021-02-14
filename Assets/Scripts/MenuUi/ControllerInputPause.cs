using System;
using UnityEngine;

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


public class ControllerInputPause : MonoBehaviour
{
    public static ControllerInputPause instance;

    Menu pauseMenu;

    bool paused;

    [NonSerialized] public bool canPause;

    private void Awake()
    {
        instance = this;
        pauseMenu = GetComponent<Menu>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && canPause)
            TogglePause();
    }

    void ChangeState(PauseState state)
    {
        switch (state)
        {
            case PauseState.None:
                break;
            case PauseState.Rising:
                MenuInputManager.instance.CloseMenu();
                break;
            case PauseState.Falling:
                MenuInputManager.instance.OpenMenu(pauseMenu);
                break;
            default:
                break;
        }
        pauseMenu.panningData.fade.blocksRaycasts = ((int)state - 1) == 1;
    }

    public void SetPaused(bool pauseState)
    { 
        paused = pauseState;
        ChangeState(pauseState ? PauseState.Falling : PauseState.Rising);
    }

    public void TogglePause()
    {
        SetPaused(!paused);
    }
}
