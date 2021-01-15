using UnityEngine;

public class EnsureLoaded : MonoBehaviour
{
	void Awake()
	{
		Manager.EnsureLoadedManagers();
	}
}
