using UnityEngine;

internal class Singleton<T> : MonoBehaviour where T : Object
{
	private static T instance;

	protected static T GetInstance()
	{
		if (instance == null)
		{
			Debug.LogWarning($"No instance of {typeof(T).Name} exists.");// Creating one instead.");
																 //instance = new GameObject(typeof(T).Name).AddComponent(typeof(T)) as T;
		}

		return instance;
	}

	protected void RegisterInstance(T instance, bool destroyOnFail = false)
	{
		if (instance == null)
		{
			Debug.LogError("Tried to Register Instance of Null");
			return;
		}

		if (Singleton<T>.instance == null || Singleton<T>.instance == instance)
			Singleton<T>.instance = instance;
		else
		{
			if (destroyOnFail)
				GameObject.Destroy(instance);
			else
				Debug.LogErrorFormat("A Instance of {0} Already Exists", typeof(T).Name);
		}
	}

	protected void RegisterInstanceOverwrite(T instance)
	{
		Singleton<T>.instance = instance;
	}
}
