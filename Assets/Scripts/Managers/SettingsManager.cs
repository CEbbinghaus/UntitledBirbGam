using UnityEngine;
using UnityEngine.Audio;

internal class SettingsManager : Singleton<SettingsManager>
{
	public AudioMixer mixer;

	static float _masterVolume;
	static public float MasterVolume
	{
		get
		{
			return _masterVolume;
		}
		set
		{
			_masterVolume = Mathf.Clamp(value, 0.0000001f, 1f);
			PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
			GetInstance().mixer.SetFloat("MasterVolume", Mathf.Log10(_masterVolume) * 20);
		}
	}

	static float _musicVolume;
	static public float MusicVolume
	{
		get
		{
			return _musicVolume;
		}
		set
		{
			_musicVolume = Mathf.Clamp(value, 0.0000001f, 1f);
			PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
			GetInstance().mixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolume) * 20);
		}
	}

	static float _sfxVolume;
	static public float SFXVolume
	{
		get
		{
			return _sfxVolume;
		}
		set
		{
			_sfxVolume = Mathf.Clamp(value, 0.0000001f, 1f);
			PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
			GetInstance().mixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolume) * 20);
		}
	}

	// Have to split these up for some goddamn reason. Doesn't set them at start otherwise
	void Awake()
	{
		RegisterInstance(this);
		mixer = Resources.Load("Main") as AudioMixer;
	}

	void Start()
	{
		// First load. Set defaults
		if (!PlayerPrefs.HasKey("InitialSetup"))
		{
			PlayerPrefs.SetString("InitialSetup", "yes");
			PlayerPrefs.SetInt("JoystickPosition", 0);
			MusicVolume = 0.5f;
			SFXVolume = 0.5f;
		}
		_masterVolume = PlayerPrefs.GetFloat("MasterVolume");
		_musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		_sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
	}
}
