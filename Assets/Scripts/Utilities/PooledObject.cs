using System;
using UnityEngine;

interface IInitializable
{
	void Initialize(GameObject prefab, uint amount = 10);
	uint GetInitialPoolCount();
}

public abstract class PooledObject<T> : MonoBehaviour, IPoolable, IInitializable where T : PooledObject<T>
{
	static ObjectPool<T> pool = new ObjectPool<T>();

	public virtual void OnActivate() {}
	public virtual void OnDeactivate() {}

	public uint DefaultPoolInstances = 10;

	public uint GetInitialPoolCount()
	{
		return DefaultPoolInstances;
	}

	public void Initialize(GameObject obj, uint amount) //Func<T> generator, uint amount
	{
		pool.Initialize(GetGenerator(obj), amount);
	}

	public static bool ObjectExists(T obj)
	{
		return pool.Exists(obj);
	}

	public static T GetObject()
	{
		return pool.GetObject();
	}

	public T GetInstanceObject()
	{
		T obj = pool.GetObject();
		return obj;
	}

	public static void RemoveObject(T obj)
	{
		pool.RetireObject(obj);
	}

	public void RemoveInstanceObject()
	{
		pool.RetireObject((T)this);
	}

	protected static Func<T> GetGenerator(GameObject prefab)
	{
		return () =>
		{
			var gm = Instantiate(prefab);
			return gm.GetComponent<T>();
		};
	}
}
