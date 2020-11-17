using System.Collections;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Camera camera;

	//Orient the camera after all movement is completed this frame to avoid jittering
	void LateUpdate()
	{
		if (camera == null)
			camera = Camera.main;
		transform.LookAt(camera.transform.position);
	}
}
