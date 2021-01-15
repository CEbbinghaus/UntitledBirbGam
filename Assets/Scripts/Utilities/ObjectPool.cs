using System;
using System.Collections.Generic;
using UnityEngine;

static class PoolContainer
{
	static internal Transform pool;
}

internal interface IPoolable
{
	void OnActivate();
	void OnDeactivate();
}

public class ObjectPool
{
	public static Func<T> GetGenerator<T>(GameObject prefab)
	{
		return () =>
		{
			var gm = GameObject.Instantiate(prefab);
			return gm.GetComponent<T>();
		};
	}
}

/// <summary>
/// Object pool to Reuse Objects that respawn
/// </summary>
/// <typeparam name="T">Object Type to Reuse</typeparam>
public class ObjectPool<T> : ObjectPool where T : Component
{
	Func<T> generator;
	List<T> pool = new List<T>();
	Transform poolTransform;

	uint totalGenerated = 0;
	bool initialized = false;

	static void InitializePoolContainer()
	{
		if (PoolContainer.pool != null)
			return;

		PoolContainer.pool = new GameObject("Pools").transform;
		GameObject.DontDestroyOnLoad(PoolContainer.pool);
	}

	bool EnsureInitialized()
	{
		if (!initialized)
			throw new Exception("Tried to Access Pool before it was Initialized");
		return initialized;
	}

	/// <summary>
	/// Constructor to create a Object pool
	/// </summary>
	/// <param name="amount">Amount of Objects to Pre-Generate</param>
	/// <param name="generator">Geenerator returning Instance of Object</param>
	public ObjectPool(uint amount, Func<T> generator, string name = null) : this(name)
	{
		Initialize(generator, amount);
	}

	/// <summary>
	/// Constructor to create a Object pool
	/// </summary>
	/// <param name="amount">Amount of Objects to Pre-Generate</param>
	/// <param name="generator">Geenerator returning Instance of Object</param>
	public ObjectPool(string name = null) {}

	public void Initialize(Func<T> generator, uint amount, string name = null)
	{
		InitializePoolContainer();

		poolTransform = new GameObject((name ?? typeof(T).Name) + " Pool").transform;
		poolTransform.SetParent(PoolContainer.pool);

		initialized = true;
		this.generator = generator;
		for (uint i = 0; i < amount; ++i)
		{
			var obj = GenerateObject();
			obj.gameObject.SetActive(false);
			pool.Add(obj);
		}
	}

	T GenerateObject()
	{
		var obj = generator();
		obj.transform.SetParent(poolTransform);
		++totalGenerated;
		return obj;
	}

	/// <summary>
	/// Returns a Object from the pool. if none exist it generates a new one
	/// </summary>
	/// <returns>Instance of T</returns>
	public T GetObject()
	{
		if (!EnsureInitialized())return default(T);

		T element = null;
		if (pool.IsEmpty())
			element = GenerateObject();
		else
			element = pool.Shift();

		element.gameObject.SetActive(true);
		if (element is IPoolable)
			(element as IPoolable).OnActivate();
		return element;
	}

	/// <summary>
	/// Checks if a Instance exists within the pool
	/// </summary>
	/// <param name="obj">Object to search for</param>
	/// <returns>Bool if the object exists</returns>
	public bool Exists(T obj)
	{
		if (!EnsureInitialized())return false;

		return pool.Contains(obj);
	}

	/// <summary>
	/// Removes the Object from the game and adds it back to the pool to be reused
	/// </summary>
	/// <param name="obj">Object to Remove</param>
	public void RetireObject(T obj)
	{
		if (!EnsureInitialized())return;

		if (pool.Contains(obj))
		{
			Debug.LogError("Tried to Add a Duplicate of: " + typeof(T).Name);
			return;
		}

		obj.gameObject.SetActive(false);
		if (obj is IPoolable)
			(obj as IPoolable).OnDeactivate();
		pool.Add(obj);
	}
}
