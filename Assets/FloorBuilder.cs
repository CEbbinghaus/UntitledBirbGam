using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
{
	[ContextMenu("Build")]
	void Build()
	{
		Vector3 pos = transform.position;
		for (int i = 0; i < 11; i++)
		{
			for (int ii = 0; ii < 11; ii++)
			{
				GameObject go = Instantiate(gameObject, new Vector3(pos.x - 10 * i, pos.y, pos.z - 10 * ii), Quaternion.identity, transform.parent);
				go.name = $"{gameObject.name} ({i:D2}, {ii:D2})";
			}
		}
	}
}
