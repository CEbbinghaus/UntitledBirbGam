using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

internal class BackgroundMusic : Singleton<BackgroundMusic>
{

	[SerializeField]
	float FadeTime = 5.0f;

	[SerializeField,
		Range(0, 1)
	]
	float MusicVolume = 0.5f;

	[SerializeField]
	AudioSource ChaseMusic;
	[SerializeField]
	AudioSource IdleMusic;

	[SerializeField]
	public bool IsChasing = false;
	float fadeAmount = 0.0f;

	void Awake()
	{
		RegisterInstanceOverwrite(this);
		if (!(ChaseMusic && IdleMusic))
		{
			Debug.LogError("Missing a music track", this);
		}
	}

	public static void SetChasing(bool chasing)
	{
		GetInstance().IsChasing = chasing;
	}

	void Update()
	{
		if (IsChasing)
			fadeAmount += Time.deltaTime / FadeTime;
		else
			fadeAmount -= Time.deltaTime / FadeTime;

		fadeAmount = Mathf.Clamp(fadeAmount, 0, MusicVolume);

		ChaseMusic.volume = fadeAmount;
		IdleMusic.volume = MusicVolume - fadeAmount;
	}
}
