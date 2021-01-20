using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
	public Slider BGMSlider;
	public AudioMixerGroup BGMMix;
	public Slider SFXSlider;
	public AudioMixerGroup SFXMix;
	public RectTransform toggle;

	private void Start()
	{
		BGMSlider.onValueChanged.AddListener(delegate { SettingsManager.MusicVolume = BGMSlider.value / 10f; }) ;
		SFXSlider.onValueChanged.AddListener(delegate { SettingsManager.SFXVolume = SFXSlider.value / 10f; });
	}

	private void OnEnable()
	{
		// First load. Set defaults
		if (!PlayerPrefs.HasKey("InitialSetup"))
		{
			PlayerPrefs.SetString("InitialSetup", "yes");
			PlayerPrefs.SetInt("JoystickPosition", 0);
			SettingsManager.MusicVolume = 0.5f;
			SettingsManager.SFXVolume = 0.5f;
		}
		if (toggle)
		{
			toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x + PlayerPrefs.GetInt("JoystickPosition") * toggle.sizeDelta.x, toggle.anchoredPosition.y);
		}
		BGMSlider.value = PlayerPrefs.GetFloat("MusicVolume") * 10;
		SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume") * 10;
	}

	/// <summary>
	/// Set joystick side. 0 for left, 1 for right. Clamped to handle errors
	/// </summary>
	/// <param name="side"></param>
	public void SetJoystickSide(int side)
	{
		if (side == Mathf.Abs(PlayerPrefs.GetInt("JoystickPosition") - 1))
		{
			if (side == 0)
			{
				toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x - toggle.sizeDelta.x, toggle.anchoredPosition.y);
			}
			if (side == 1)
			{
				toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x + toggle.sizeDelta.x, toggle.anchoredPosition.y);
			}
			PlayerPrefs.SetInt("JoystickPosition", side);
		}
	}

	public void SaveOptions() => PlayerPrefs.Save();
}
