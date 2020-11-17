using UnityEngine;

internal class Singleton<T> : MonoBehaviour
{
	private static T instance;

	protected static T GetInstance()
	{
		if (instance == null)
			Debug.LogWarningFormat("No Instance of {0} Exists", typeof(T).Name);
		return instance;
	}

	protected void RegisterInstance(T instance)
	{
		if (instance != null)
			Singleton<T>.instance = instance;
		else
		{
			Debug.LogErrorFormat("A Instance of {0} Already Exists", typeof(T).Name);
		}
	}
}
