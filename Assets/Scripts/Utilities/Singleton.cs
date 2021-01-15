using UnityEngine;

internal class Singleton<T> : MonoBehaviour where T : Object
{
	private static T instance;

	protected static T GetInstance()
	{
		if (instance == null)
			Debug.LogWarningFormat("No Instance of {0} Exists", typeof(T).Name);

		return instance;
	}

	protected void RegisterInstance(T instance, bool destroyOnFail = false)
	{
		if (instance == null)
		{
			Debug.LogError("Tried to Register Instance of Null");
			return;
		}

		if (Singleton<T>.instance != null || Singleton<T>.instance == instance)
			Singleton<T>.instance = instance;
		else
		{
			if (destroyOnFail)
				GameObject.Destroy(instance);
			else
				Debug.LogErrorFormat("A Instance of {0} Already Exists", typeof(T).Name);
		}
	}
}
