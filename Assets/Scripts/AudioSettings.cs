using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
	public AudioMixer mixer;

	public void SetMasterLevel(float sliderValue)
	{
		mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
	}
	public void SetMusicLevel(float sliderValue)
	{
		mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
	}
	public void SetSFXLevel(float sliderValue)
	{
		mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
	}
}
