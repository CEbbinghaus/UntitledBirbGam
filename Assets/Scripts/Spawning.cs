﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Spawning : Singleton<Spawning>
{

	[SerializeField]
	float Delay = 20.0f;
	float time = 0;

	Spawnable toBeSpawned;

	[Range(0, 1)]
	[SerializeField]
	float PercentageSpawn = 0.3f;

	[SerializeField]
	List<Spawnable> spawnables;
	List<Spawnable> despawned;

	void Awake()
	{
		if (GetInstance() != null)
			Destroy(this.gameObject);
		else
			RegisterInstance(this);

		GameObject.DontDestroyOnLoad(this);
		spawnables = new List<Spawnable>((Spawnable[])Resources.FindObjectsOfTypeAll(typeof(Spawnable)));
		SceneManager.sceneLoaded += Util.WrapSceneLoadedEvent(SceneChanged);
	}

	private void SceneChanged()
	{
		spawnables = new List<Spawnable>((Spawnable[])Resources.FindObjectsOfTypeAll(typeof(Spawnable)));
		if (despawned != null)
			despawned.Clear();

		foreach (Spawnable spawn in spawnables)
		{
			if (Random.Range(0f, 1f) > PercentageSpawn)
				gameObject.SetActive(false);
		}
	}

	public static void RefreshSpawned()
	{
		Spawning instance = GetInstance();
		if (!instance)return;

		instance.despawned = instance.spawnables.Where((Spawnable s) =>
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
