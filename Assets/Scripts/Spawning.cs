using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Spawning : Singleton<Spawning>
{

	[SerializeField]
	float Delay = 20.0f;
	float time = 0;

	Food toBeSpawned;

	[Range(0, 1)]
	[SerializeField]
	float PercentageSpawn = 0.3f;

	[SerializeField]
	List<Food> spawnables;
	List<Food> despawned = new List<Food>();

	void Awake()
	{
		RegisterInstance(this, true);

		transform.parent = null;
		DontDestroyOnLoad(this.gameObject);

		spawnables = new List<Food>(Resources.FindObjectsOfTypeAll<Food>());
		SceneManager.sceneLoaded += Util.WrapSceneLoadedEvent(SceneChanged);
	}

	private void SceneChanged()
	{
		spawnables = new List<Food>(Resources.FindObjectsOfTypeAll<Food>());
		if (despawned != null)
			despawned.Clear();

		foreach (Food spawn in spawnables)
		{
			if (spawn.ForcedSpawn)continue;

			if (Random.Range(0f, 1f) > PercentageSpawn)
				spawn.gameObject.SetActive(false);
		}
	}

	public static void RefreshSpawned()
	{
		Spawning instance = GetInstance();
		if (!instance)
		{
			Debug.LogError("No Instance of Spawning found");
			return;
		}

		instance.despawned = instance.spawnables.Where((Food s) =>
		{
			return s != null && !s.gameObject.activeSelf;
		}).ToList();
	}

	void Update()
	{
		if (spawnables.Count <= 0)return;

		if (toBeSpawned != null && time > 0)
		{
			time -= Time.deltaTime;
			return;
		}
		else if (toBeSpawned != null && time <= 0)
		{
			toBeSpawned.gameObject.SetActive(true);
			toBeSpawned = null;
			return;
		}

		if (despawned.Count() > 0 && 1 - (despawned.Count() / (float)spawnables.Count) < PercentageSpawn)
		{
			int index = (Random.Range(0, despawned.Count));
			toBeSpawned = despawned[index];
			if (Util.inViewFrostrum(toBeSpawned.transform.position))
				return;
			despawned.RemoveAt(index);
			time = Delay;
		}
	}
}
