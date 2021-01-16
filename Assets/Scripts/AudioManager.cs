using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

internal class AudioManager : EntityPoolManager<string, AudioInstance>
{

	ObjectPool<AudioInstance> instances;

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

	bool errorSkip;

	public static void SetChasing(bool chasing)
	{
		((AudioManager)GetInstance()).IsChasing = chasing;
	}

	new void Awake()
	{
		print("Testing");
		base.Awake();
		Initialize();
		if (!(ChaseMusic && IdleMusic))
		{
			errorSkip = true;
			Debug.LogError("Missing a music track", this);
		}
	}

	void Initialize()
	{
		AudioInstance[] instances = Resources.LoadAll<AudioInstance>("Audio/Sources");

		foreach (AudioInstance instance in instances)
		{
			RegisterEntityPrefab(instance.GetType(), instance.gameObject);
		}
	}

	public static AudioInstance PlaySound(AudioInstance sound, Vector3 position)
	{
		AudioInstance instance = GetObject(sound.GetType());
		instance.transform.position = position;
		return instance;
	}

	public static AudioInstance PlaySound(string name, Vector3 position)
	{
		AudioInstance instance = GetObject(name);
		instance.transform.position = position;
		return instance;
	}

	void Update()
	{
		if (!errorSkip)
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
}

/*
	internal enum Event
	{
		Chase,
		SmallPickup,
		BigPickup,
		Score
	}

	[SerializeField]
	GameObject AudioSourcePrefab;

	[Header("Audio Clips")]

	[SerializeField]
	AudioClip BackgroundSound;
	[Range(0, 1)][SerializeField]
	float BackgroundVolume = 1.0f;

	[SerializeField]
	AudioClip ChaseSound;
	[Range(0, 1)][SerializeField]
	float ChaseVolume = 1.0f;

	[SerializeField]
	AudioClip SeedSound;
	[Range(0, 1)][SerializeField]
	float SeedVolume = 1.0f;

	[SerializeField]
	AudioClip SandwitchSound;
	[Range(0, 1)][SerializeField]
	float SandwitchVolume = 1.0f;

	[SerializeField]
	AudioClip ScoreSound;
	[Range(0, 1)][SerializeField]
	float ScoreVolume = 1.0f;

	ObjectPool<AudioSource> SourcePool = new ObjectPool<AudioSource>();

	string[] names = { "Background", "Chase", "Seed", "Sandwitch", "Score" };

	AudioSource[] sources;

	void Awake()
	{
		Transform noDestroy = this.transform.parent;

		if (noDestroy == null)
			noDestroy = transform;

		DontDestroyOnLoad(noDestroy);

		if (GetInstance() != this)
		{
			Destroy(this.gameObject);
			return;
		}
		RegisterInstance(this);

		SourcePool.Initialize(ObjectPool.GetGenerator<AudioSource>(AudioSourcePrefab), (uint)names.Length);

		sources = new AudioSource[names.Length];

		System.Type t = typeof(AudioManager);
		int i = 0;
		foreach (var name in names)
		{
			var src = gameObject.AddComponent<AudioSource>();
			src.playOnAwake = false;

			var SoundProp = t.GetField(name + "Sound");
			var VolProp = t.GetField(name + "Volume");

			src.clip = (AudioClip)SoundProp.GetValue(this);
			src.volume = (float)VolProp.GetValue(this);
			if (name == "Background" || name == "Chase")
			{
				src.loop = true;
				src.Play();
			}
			sources[i++] = src;
		}
	}

	public static void Emit(Event e)
	{
		var instance = GetInstance();
		if (!instance)return;

		AudioSource src = instance.sources[(int)e + 1];

		src.Play();
	}

	public static AudioSource GetSource(Event e)
	{
		var instance = GetInstance();
		if (!instance)return null;
		return instance.sources[(int)e + 1];
	}
*/
