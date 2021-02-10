using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeMovement : MonoBehaviour
{
	bool m_Fired = false;

	public Rigidbody rb = null;

	MeshCollider collider = null;

	float SpawnHeight = 0.0f;

	Camera MainCamera = null;

	int m_RandomSpawn = 0;

	float m_MaxSpawnTimer = 0.0f;

	public int screenOffset = 50;

	public Transform Player = null;

	public float SpawnTimer = 3.0f;

	public float Speed = 3.0f;

	private void Awake()
	{
		// Get components.
		rb = GetComponent<Rigidbody>();
		collider = GetComponent<MeshCollider>();

		m_MaxSpawnTimer = SpawnTimer;

		// Get reference to the main camera, for quick access.
		MainCamera = Camera.main;

		// Calculate the height at which to spawn the frisbee.
		SpawnHeight = transform.position.y;
	}

	void Update()
	{

		if (Util.inViewFrostrum(transform.position))return;

		if (SpawnTimer <= 0.0f)
		{
			// Reset variables.
			collider.enabled = true;
			rb.useGravity = false;

			transform.position = GetRandomSpawnPosition(screenOffset) + Vector3.up * SpawnHeight;
			//transform.position = MainCamera.ScreenToWorldPoint(new Vector3(GetRandomSpawnPosition(true), GetRandomSpawnPosition(false), SpawnHeight));

			// Don't spawn in an object.
			// int i = 0;
			// while (Physics.CheckSphere(transform.position, 1) && i++ < 1000)
			// {
			// 	// Spawn offscreen and shoot towards where the player is right now.
			// 	//transform.position = MainCamera.ScreenToWorldPoint(new Vector3(GetRandomSpawnPosition(true), GetRandomSpawnPosition(false), SpawnHeight), MainCamera.stereoActiveEye);
			// }

			transform.rotation = Quaternion.identity;
			Vector3 velocity = Player.GetComponent<Rigidbody>().velocity;
			rb.velocity = ((Player.position + velocity) - transform.position).normalized * Speed;
			m_Fired = true;

			SpawnTimer = m_MaxSpawnTimer;
		}
		else
			SpawnTimer -= Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Bidge":
				collision.gameObject.GetComponent<BidgeBehaviour>().StopChasing();
				break;
			case "Player":
				if (GetFired())
					collision.gameObject.GetComponent<PlayerManager>().TakeDamage();
				break;
		}
		// Clean up to make frisbee Collision look a bit better
		rb.useGravity = true;
	}

	private Vector3 GetRandomSpawnPosition(int offset)
	{

		int x = (Random.Range(0, 1) == 1) ? Random.Range(-offset, 0) : Random.Range(Screen.width, Screen.width + offset);
		int y = (Random.Range(0, 1) == 1) ? Random.Range(-offset, 0) : Random.Range(Screen.height, Screen.height + offset);

		Ray ray = MainCamera.ScreenPointToRay(new Vector3(x, y, 0));

		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0;
		if (hPlane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}

		return Vector2.zero;
	}

	public bool GetFired()
	{
		return m_Fired;
	}
}
