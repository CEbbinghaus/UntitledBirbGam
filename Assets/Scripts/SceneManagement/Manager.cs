using System;
using CustomSceneManagement;
using UnityEngine;

internal class Manager : Singleton<Manager>
{
	void Awake()
	{
		RegisterInstance(this);
	}

	internal static bool Exists()
	{
		Manager instance = GetInstance();
		return instance != null;
	}

	internal static void EnsureLoadedManagers()
	{
		if (!Exists())
		{
			SceneManager.EditorLoadScene = SceneManager.GetCurrentScene().path;
			SceneManager.LoadSceneById(-1, true);
		}
	}
}
