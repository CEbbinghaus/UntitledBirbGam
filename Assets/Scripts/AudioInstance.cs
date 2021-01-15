using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioInstance : MonoBehaviour, IPoolable, IIdentifiable<string>
{
	[SerializeField]
	string Name = "";

	AudioSource source;

	public string GetType()
	{
		return Name;
	}

	void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (!source.isPlaying)
			AudioManager.RemoveObject(this);
	}

	public void OnActivate()
	{
		source.Play();
	}

	public void OnDeactivate() {}
}
