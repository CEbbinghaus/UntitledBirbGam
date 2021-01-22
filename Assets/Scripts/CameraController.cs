﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	[SerializeField] Bounds bounds;
	[SerializeField] Transform target;
	[SerializeField] Rigidbody targetRB;
	[SerializeField]
	internal List<Transform> Targets;

	[Range(0, 1)]
	[SerializeField] float PredictionDivider = 0.2f;
	[SerializeField] float minDistance = 5f;
	[SerializeField] float buffer = 0.5f;
	[SerializeField] float snap = 2;

	Camera camera;

	private void Awake()
	{
		UIManager.onChangeUIOrientation += ChangeMinHeight;
	}

	// Start is called before the first frame update
	void Start()
	{
		camera = GetComponent<Camera>();
		if (targetRB == null)
			targetRB = target.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (target == null)return;

		//var frustumHeight = 2.0f * difference * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

		Vector2 maxSize = Vector2.zero;

		foreach (Transform t in Targets)
		{
			Vector3 dist = (target.position - t.transform.position);

			if (Mathf.Abs(dist.x) > maxSize.x)
				maxSize.x = Mathf.Abs(dist.x);

			if (Mathf.Abs(dist.z) > maxSize.y)
				maxSize.y = Mathf.Abs(dist.z);
		}

		maxSize *= (2 + buffer);
		float height = maxSize.y;
		if (maxSize.x > maxSize.y * camera.aspect)
		{
			height = maxSize.x / camera.aspect;
		}

		if (height <= 0)
		{
			height = 2.0f * minDistance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
		}

		if (height > bounds.extents.z)
		{
			height = bounds.extents.z;
		}
		if (height * camera.aspect > bounds.extents.x)
		{
			height = bounds.extents.x / camera.aspect;
		}

		float width = height * camera.aspect;

		var distance = height * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

		if (distance < minDistance)
			distance = minDistance;

		Vector3 ftp = target.position;

		if (targetRB != null)
			ftp = Vector3.Lerp(target.position, target.position + targetRB.velocity, PredictionDivider);

		Vector3 finalPos = new Vector3(Mathf.Clamp(ftp.x, (bounds.min.x + width) / 2, (bounds.max.x - width) / 2), 0, Mathf.Clamp(ftp.z, (bounds.min.z + height) / 2, (bounds.max.z - height) / 2));

		//print(bdist);

		transform.position = Vector3.Lerp(transform.position, finalPos + Vector3.up * distance, Time.deltaTime * snap);

	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(bounds.center, bounds.extents);
	}

	void ChangeMinHeight(ScreenOrientation orientation)
	{
		switch (orientation)
		{
			case ScreenOrientation.Portrait:
				minDistance = 30;
				break;
			case ScreenOrientation.Landscape:
				minDistance = 20;
				break;
			default:
				goto case ScreenOrientation.Landscape;
		}
	}
}
