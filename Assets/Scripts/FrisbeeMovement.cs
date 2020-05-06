using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeMovement : MonoBehaviour
{
	/// <summary>
	/// The transform of the player.
	/// </summary>
	public Transform m_PlayerTransform = null;

	/// <summary>
	/// If the frisbee has been fired.
	/// </summary>
	private bool m_Fired = false;

	/// <summary>
	/// Rigidbody of the frisbee.
	/// </summary>
	private Rigidbody m_Rigid = null;

	/// <summary>
	/// The speed of the frisbee.
	/// </summary>
	public float m_Speed = 3.0f;

	/// <summary>
	/// The mesh renderer.
	/// </summary>
	private MeshRenderer m_MeshRend = null;

	/// <summary>
	/// The collider.
	/// </summary>
	private MeshCollider m_Collider = null;

	/// <summary>
	/// The height to spawn the frisbee at.
	/// </summary>
	private float m_SpawnPosHeight = 0.0f;

	/// <summary>
	/// The main camera, for quick access.
	/// </summary>
	private Camera m_MainCamera = null;

	/// <summary>
	/// Whether to spawn the frisbee right or left/top or bottom of the screen.
	/// </summary>
	private int m_RandomSpawn = 0;

	/// <summary>
	/// Timer to respawn the frisbee.
	/// </summary>
	public float m_SpawnTimer = 3.0f;

	/// <summary>
	/// For resetting the timer.
	/// </summary>
	private float m_MaxSpawnTimer = 0.0f;

	private void Awake()
	{
		// Get components.
		m_Rigid = GetComponent<Rigidbody>();
		m_MeshRend = GetComponent<MeshRenderer>();
		m_Collider = GetComponent<MeshCollider>();

		m_MaxSpawnTimer = m_SpawnTimer;

		// Get reference to the main camera, for quick access.
		m_MainCamera = Camera.main;

		// Calculate the height at which to spawn the frisbee.
		m_SpawnPosHeight = Camera.main.transform.position.y - transform.position.y;
	}

	void Update()
    {
		// If the frisbee has been fired, check if it's on screen.
		if (m_Fired)
		{
			// If the frisbee is offscreen, turn off it's rendering and collision.
			if (!m_MeshRend.isVisible)
			{
				m_MeshRend.enabled = false;
				m_Collider.enabled = false;
				m_Fired = false;
			}
		}
		// Fire the frisbee if it is time.
		else
		{
			// Fire!
			if (m_SpawnTimer <= 0.0f)
			{
				// Reset variables.
				m_MeshRend.enabled = true;
				m_Collider.enabled = true;
				m_Rigid.useGravity = false;

				transform.position = m_MainCamera.ScreenToWorldPoint(new Vector3(GetRandomSpawnPosition(true), GetRandomSpawnPosition(false), m_SpawnPosHeight));

				// Don't spawn in an object.
				while (Physics.CheckSphere(transform.position, 1))
				{
					// Spawn offscreen and shoot towards where the player is right now.
					transform.position = m_MainCamera.ScreenToWorldPoint(new Vector3(GetRandomSpawnPosition(true), GetRandomSpawnPosition(false), m_SpawnPosHeight), m_MainCamera.stereoActiveEye);
				}
				transform.LookAt(m_PlayerTransform);
				m_Rigid.velocity = transform.forward * m_Speed;
				m_Fired = true;

				m_SpawnTimer += m_MaxSpawnTimer;
			}
			// Decrease timer.
			else
				m_SpawnTimer -= Time.deltaTime;
		}
    }

	private void OnCollisionEnter(Collision collision)
	{
		// If the frisbee hits something, fall down.
		m_Rigid.useGravity = true;
	}

	private float GetRandomSpawnPosition(bool isXAxis)
	{
		// Randomly spawn on the left or right/top or bottom.
		m_RandomSpawn = Random.Range(0, 2);

		// Spawn right/top.
		if (m_RandomSpawn == 1)
		{
			if (isXAxis)
				return 1.1f;
			else
				return Random.Range(1.1f, 1.9f);
		}

		// Spawn left/bottom.
		else
		{
			if (isXAxis)
				return -0.1f;
			else
				return Random.Range(-0.9f, -0.1f);
		}
	}

	public bool GetFired()
	{
		return m_Fired;
	}
}