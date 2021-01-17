using UnityEngine;

public class TrackObject : MonoBehaviour
{
	[SerializeField]
	Transform target;

	void Update()
	{
		if (target != null)
			transform.position = target.position;
	}
}
