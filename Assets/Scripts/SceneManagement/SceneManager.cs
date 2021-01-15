using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using SCM = UnityEngine.SceneManagement.SceneManager;

namespace CustomSceneManagement
{

	enum GameScene
	{
		MainMenu = 0,
		Game = 1
	}

	internal class SceneManager
	{

		public static string EditorLoadScene = null;

		public static bool IsMainMenu => GetCurrentSceneIndex() == (int)GameScene.MainMenu;
		public static bool IsGame => GetCurrentSceneIndex() == (int)GameScene.Game;

		public static int GetCurrentSceneIndex()
		{
			return SCM.GetActiveScene().buildIndex - 1;
		}

		public static Scene GetCurrentScene()
		{
			return SCM.GetActiveScene();
		}

		public static int GetSceneCount()
		{
			return SCM.sceneCountInBuildSettings - 1;
		}

		public static void LoadSceneById(int id, bool force = false)
		{
			if (id >= GetSceneCount())
				throw new Exception("Scene Index outisde of Range");

			int index = 1 + id;
			Scene scene = SCM.GetSceneByBuildIndex(index);
			if (force)
				if (!scene.isLoaded)
					SCM.LoadScene(index);
				else
					SCM.LoadScene(scene.name);
			else
			if (!scene.isLoaded)
				SCM.LoadSceneAsync(index);
			else
				SCM.LoadSceneAsync(scene.name);
		}

		public static void LoadNextScene()
		{
			int index = GetCurrentSceneIndex();
			if (index < GetSceneCount() - 1)
			{
				LoadSceneById(index + 1);
			}
			else
			{
				Debug.LogError("At Last Scene. Cannot Load Next");
				//TODO: Maybe load MainMenu™
			}

		}

		public static void LoadSceneByName(String name, bool force = false)
		{
			Scene scene = SCM.GetSceneByName(name);
			if (force)
				if (!scene.isLoaded)
					SCM.LoadScene(name);
				else
					SCM.LoadScene(scene.name);
			else
			if (!scene.isLoaded)
				SCM.LoadSceneAsync(name);
			else
				SCM.LoadSceneAsync(scene.name);
		}

		public static void ReloadCurrentScene()
		{
			SCM.LoadScene(SCM.GetActiveScene().buildIndex);
		}
	}
}
