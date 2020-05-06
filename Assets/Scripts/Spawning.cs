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

	static Spawning instance;

	void Awake()
	{
		if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
			instance = this;
		}
		GameObject.DontDestroyOnLoad(this);
		spawnables = new List<Spawnable>((Spawnable[])Resources.FindObjectsOfTypeAll(typeof(Spawnable)));
		SceneManager.sceneLoaded += SceneChanged;
	}

	public static void RefreshSpawned(){
		if(!instance)return;

		instance.despawned = instance.spawnables.Where((Spawnable s) => {
			return s != null && !s.gameObject.activeSelf;
		}).ToList();
	}

	private void SceneChanged(Scene scene, LoadSceneMode mode)
	{
		spawnables = new List<Spawnable>((Spawnable[])Resources.FindObjectsOfTypeAll(typeof(Spawnable)));
		if(despawned != null)
			despawned.Clear();
	}

	void Update()
	{
		if(spawnables.Count <= 0)return;
		if(toBeSpawned != null && time > 0){
			time -= Time.deltaTime;
			return;
		}else if(toBeSpawned != null && time <= 0){
			toBeSpawned.gameObject.SetActive(true);
			toBeSpawned = null;
			return;
		}

		if(despawned.Count() > 0 && 1 - (despawned.Count() / (float)spawnables.Count) < PercentageSpawn)
		{
			int index =  (Random.Range(0, despawned.Count));
			toBeSpawned = despawned[index];
			despawned.RemoveAt(index);
			time = Delay;
		}
	}    
}
