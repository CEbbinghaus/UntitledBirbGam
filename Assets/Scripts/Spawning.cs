using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawning : MonoBehaviour
{
	public float Delay = 20.0f;
	float time = 0;

	Spawnable toBeSpawned;

	[Range(0, 1)]
	public float PercentageSpawn = 0.3f;

	public List<Spawnable> spawnables;
	List<Spawnable> despawned;

	void Awake()
	{
		GameObject.DontDestroyOnLoad(this);
		spawnables = new List<Spawnable>((Spawnable[])Resources.FindObjectsOfTypeAll(typeof(Spawnable)));
		SceneManager.sceneLoaded += SceneChanged;
	}

	public void RefreshSpawned(){
		despawned = spawnables.Where((Spawnable s) => {
			return !s.gameObject.activeSelf;
		}).ToList();
	}

	private void SceneChanged(int level)
	{
		despawned.Clear();
	}

	void Update()
	{
		if(toBeSpawned != null && time > 0){
			time -= Time.deltaTime;
			return;
		}else if(toBeSpawned != null && time <= 0){
			toBeSpawned.gameObject.SetActive(true);
			toBeSpawned = null;
			return;
		}

		RefreshSpawned();

		if(despawned.Count() > 0 && 1 - (despawned.Count() / (float)spawnables.Count) < PercentageSpawn)
		{
			toBeSpawned = despawned.First();
			time = Delay;
		}
	}    
}
