using System;
using System.Collections;
using CustomSceneManagement;
using UnityEngine;
using UnityEngine.XR;

public class SplashScene : MonoBehaviour
{

	[SerializeField]
	[Tooltip("Root Under which all Pools will be placed")]
	Transform PoolRoot;

	[SerializeField]
	[Tooltip("Root Under which all Managers will be placed")]
	Transform ManagerRoot;

	[SerializeField]
	[Tooltip("Root Object to attach everything to. Stays active throughout the entire game")]
	Transform root;

	[SerializeField]
	[Tooltip("Speed at which the Splach Screen follows the players Eyes")]
	float slerpSpeed = 5.0f;

	bool SceneLoaded = false;
	bool FinishedLoading = false;
	bool FinishedSplashScreen = false;

	void Awake()
	{
		print("Game Loading up. Initializing Managers");
		DontDestroyOnLoad(root);
		if (SceneManager.EditorLoadScene != null)
		{
			// Coroutine
			Util.WaitCoroutine(LoadElements());
			StartCoroutine(StartGame());
		}
		else
			StartCoroutine(LoadElements());
	}

	void Update()
	{
		if (SceneLoaded || !FinishedLoading)return;

		if (FinishedSplashScreen ||
			Input.GetTouch(0).phase == TouchPhase.Began ||
			Input.GetAxis("Submit") > 0)
			StartCoroutine(StartGame());
	}

	void LateUpdate()
	{
		InputDevices.GetDeviceAtXRNode(XRNode.CenterEye).TryGetFeatureValue(CommonUsages.centerEyeRotation, out Quaternion rotation);

		if (rotation != null)
			transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rotation, Time.deltaTime * slerpSpeed);
	}

	void SplashDone()
	{
		FinishedSplashScreen = true;
	}

	IEnumerator LoadElements()
	{
		PoolRoot.parent = root;
		ManagerRoot.parent = root;
		PoolContainer.pool = PoolRoot;

		yield return null;

		MonoBehaviour[] managers = Resources.LoadAll<MonoBehaviour>("Managers");

		foreach (MonoBehaviour manager in managers)
		{
			GameObject obj = Instantiate(manager.gameObject);
			obj.transform.parent = ManagerRoot;
		}

		yield return null;

		MonoBehaviour[] scripts = Resources.LoadAll<MonoBehaviour>("Pools");

		scripts = scripts.Filter((obj) => obj is IInitializable);

		foreach (MonoBehaviour script in scripts)
		{
			GameObject obj = script.gameObject;
			IInitializable init = (IInitializable)script;
			init.Initialize(obj, init.GetInitialPoolCount());

			yield return null;
		}
		FinishedLoading = true;
		yield return null;
	}

	IEnumerator StartGame()
	{
		SceneLoaded = true;
		if (SceneManager.EditorLoadScene != null)
		{
			SceneManager.LoadSceneByName(SceneManager.EditorLoadScene);
			SceneManager.EditorLoadScene = null;
		}
		else
			SceneManager.LoadNextScene();
		yield return null;
	}
}
