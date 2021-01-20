using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
	CameraController c;

	public List<Transform> searchItems;

	public PlayerManager player;

	public float magnetDistance = 20f;

	private void Awake()
	{
		c = GetComponent<CameraController>();
	}

	void Update()
	{
		foreach (var item in searchItems)
		{
			if (!item.gameObject.activeSelf) continue;
			//print($"Distance to {item.name}: {Vector3.Distance(player.transform.position, item.position)}");
			if (Vector3.Distance(player.transform.position, item.position) < magnetDistance)
			{
				if (!c.Targets.Contains(item))
				{
					c.Targets.Add(item);
				}
			}
			else
			{
				if (c.Targets.Contains(item))
				{
					c.Targets.Remove(item);
				}
			}
		}
	}
}
