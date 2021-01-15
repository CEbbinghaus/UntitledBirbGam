using System;
using System.Collections;
using UnityEngine;

public static class Util
{
	public static void WaitCoroutine(IEnumerator func)
	{
		while (func.MoveNext())
		{
			if (func.Current != null)
			{
				IEnumerator num;
				try
				{
					num = (IEnumerator)func.Current;
				}
				catch (InvalidCastException)
				{
					if (func.Current.GetType() == typeof(WaitForSeconds))
						Debug.LogWarning("Skipped call to WaitForSeconds. Use WaitForSecondsRealtime instead.");
					return; // Skip WaitForSeconds, WaitForEndOfFrame and WaitForFixedUpdate
				}
				WaitCoroutine(num);
			}
		}
	}

	public static bool inViewFrostrum(Vector3 point, Camera camera = null)
	{
		if (camera == null)
			camera = Camera.main;

		if (camera == null)
			return false;

		Vector3 ViewportPos = camera.WorldToViewportPoint(point);
		if (ViewportPos.x > 0 && ViewportPos.x < 1 && ViewportPos.y > 0 && ViewportPos.y < 1)
		{
			return ViewportPos.z > 0;
		}
		return false;
	}

	public static void CopyTransform(Transform from, Transform to)
	{
		to.position = from.position;
		to.rotation = from.rotation;
	}

	public static UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> WrapSceneChangedEvent(Action function)
	{
		return (UnityEngine.SceneManagement.Scene a, UnityEngine.SceneManagement.Scene b) => function();
	}

	public static UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> WrapSceneLoadedEvent(Action function)
	{
		return (UnityEngine.SceneManagement.Scene a, UnityEngine.SceneManagement.LoadSceneMode b) => function();
	}

	public static LayerMask GetCollisionMaskOf(GameObject go)
	{
		int myLayer = go.layer;
		int layerMask = 0;
		for (int i = 0; i < 32; i++)
		{
			if (!Physics.GetIgnoreLayerCollision(myLayer, i))
			{
				layerMask = layerMask | 1 << i;
			}
		}
		return layerMask;
	}

	public static Color ColorFromHSL(float h, float s, float l)
	{
		h = h > 1 ? h / 360 : h;
		s = s > 1 ? s / 100 : s;
		l = l > 1 ? l / 100 : l;

		s *= l < .5 ? l : 1 - l;
		return Color.HSVToRGB(h, 2 * s / (l + s), l + s);
	}
}
