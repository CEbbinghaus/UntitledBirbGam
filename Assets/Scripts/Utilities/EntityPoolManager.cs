using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

interface IIdentifiable<T>
{
	T GetType();
}

class EntityPoolManager<Key, Value> : Singleton<EntityPoolManager<Key, Value>> where Value : Component, IIdentifiable<Key>
{
	static Dictionary<Key, ObjectPool<Value>> pools = new Dictionary<Key, ObjectPool<Value>>();
	static Dictionary<Key, GameObject> prefabLib = new Dictionary<Key, GameObject>();

	static HashSet<Value> spawnedEntities = new HashSet<Value>();

	protected void Awake()
	{
		RegisterInstance(this, true);
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged += Util.WrapSceneChangedEvent(OnSceneLoad);
	}

	protected static void RegisterEntityPrefab(Key key, GameObject prefab)
	{
		if (prefabLib.ContainsKey(key))
		{
			Debug.LogError("Trying to add Duplicate Key: " + key);
			return;
		}
		prefabLib.Add(key, prefab);

		pools.Add(key, new ObjectPool<Value>(1, GenerateObjectGenerator(key), key.ToString() + " Pool"));
	}

	protected static void OnSceneLoad()
	{
		foreach (Value straggler in spawnedEntities)
		{
			if (straggler.gameObject.activeSelf)
				RemoveObject(straggler);
		}
	}

	static Func<Value> GenerateObjectGenerator(Key type)
	{
		return () =>
		{
			if (!prefabLib.ContainsKey(type))
				return null;

			var gm = Instantiate(prefabLib[type]);
			var be = gm.GetComponent<Value>();
			spawnedEntities.Add(be);
			return be;
		};
	}

	// public static Value GetObject<T>()where T : Value
	// {
	// 	Type type = typeof(T);

	// 	if (!pools.ContainsKey(type))
	// 	{
	// 		Debug.LogError("Entity Pool does not Exist");
	// 		return null;
	// 	}
	// 	return pools[type].GetObject();
	// }

	public static Value GetObject(Key t)
	{
		if (!pools.ContainsKey(t))
		{
			Debug.LogError("Entity Pool does not Exist for " + t.GetType());

			return null;
		}
		return pools[t].GetObject();
	}

	public static Value GetDuplicateObject(Value entity)
	{
		Key t = entity.GetType();
		if (!pools.ContainsKey(t))
		{
			Debug.LogError("Entity Pool does not Exist for " + t.GetType());
			return null;
		}
		return pools[t].GetObject();
	}

	public static void RemoveObject(Value entity)
	{
		Key t = entity.GetType();
		if (!pools.ContainsKey(t))
		{
			Debug.LogError("Entity Pool does not Exist");
			return;
		}
		pools[t].RetireObject(entity);
	}
}
