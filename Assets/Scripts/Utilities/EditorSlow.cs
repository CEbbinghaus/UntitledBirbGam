using UnityEngine;

public class EditorSlow : MonoBehaviour
{
	[SerializeField] float TimeSpeed = 1.0f;
	void Update()
	{
		if (Application.isEditor || Time.timeScale == TimeSpeed)
			Time.timeScale = TimeSpeed;
	}
}
