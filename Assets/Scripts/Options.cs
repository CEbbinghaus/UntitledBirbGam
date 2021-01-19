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

	private void Awake()
	{
		BGMSlider.onValueChanged.AddListener(delegate { SetBGMVol(); });
		SFXSlider.onValueChanged.AddListener(delegate { SetSFXVol(); });

		// First load. Set defaults
		if (PlayerPrefs.HasKey("InitialSetup"))
		{
			PlayerPrefs.SetString("InitialSetup", "yes");
			PlayerPrefs.SetInt("joystick", 0);
			PlayerPrefs.SetFloat("bgm", 0.5f);
			PlayerPrefs.SetFloat("sfx", 0.5f);
		}
		// Load from prefs
		else
		{
			print(toggle.sizeDelta.x);
			toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x + PlayerPrefs.GetInt("joystick") * toggle.sizeDelta.x, toggle.anchoredPosition.y);
			BGMSlider.value = PlayerPrefs.GetFloat("bgm") * 10;
			SFXSlider.value = PlayerPrefs.GetFloat("sfx") * 10;
		}
	}

	void SetBGMVol()
	{
		PlayerPrefs.SetFloat("bgm", BGMSlider.value / 10f);
	}
	void SetSFXVol() => PlayerPrefs.SetFloat("sfx", SFXSlider.value / 10f);

	/// <summary>
	/// Set joystick side. 0 for left, 1 for right. Clamped to handle errors
	/// </summary>
	/// <param name="side"></param>
	public void SetJoystickSide(int side)
	{
		if (side == Mathf.Abs(PlayerPrefs.GetInt("joystick") - 1))
		{
			if (side == 0)
			{
				toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x - toggle.sizeDelta.x, toggle.anchoredPosition.y);
			}
			if (side == 1)
			{
				toggle.anchoredPosition = new Vector2(toggle.anchoredPosition.x + toggle.sizeDelta.x, toggle.anchoredPosition.y);
			}
			PlayerPrefs.SetInt("joystick", side);
		}
	}

	public void SaveOptions() => PlayerPrefs.Save();
}
