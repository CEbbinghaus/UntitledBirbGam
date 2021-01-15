using System.Collections;
using System.Collections.Generic;
using CustomSceneManagement;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
	public void LoadMainMenu()
	{
		SceneManager.LoadSceneById(0);
	}
}
