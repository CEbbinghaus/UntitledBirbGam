using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Food : MonoBehaviour
{

	[SerializeField]
	GameObject mesh;

	[SerializeField]
	float bobHeight = .5f;

	[SerializeField]
	float bobSpeed = 1.4f;

	[SerializeField]
	float rotationSpeed = 5f;

	[SerializeField]
	public bool ForcedSpawn = false;

	float offset;

	void Awake()
	{
		offset = Random.Range(0f, 200f) + (float)(transform.position.GetHashCode() % 1000);
		rotationSpeed = (rotationSpeed + Random.Range(-.1f, .1f)) * 10;
		bobSpeed += Random.Range(-.1f, .1f);
	}

	void Update()
	{
		mesh.transform.localPosition = Vector3.up * (((Mathf.Sin(Time.time * bobSpeed + offset) + 1.0f) / 2) * bobHeight);
		mesh.transform.localRotation = Quaternion.Euler(0f, (Time.time * rotationSpeed) % 360, 0f);
	}
}
