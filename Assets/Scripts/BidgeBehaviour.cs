using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BidgeBehaviour : MonoBehaviour
{
	/// <summary>
	/// The transform of the player.
	/// </summary>
	public Transform m_PlayerCharacterTransform = null;

	/// <summary>
	/// The Nav Mesh Agent
	/// </summary>
	private NavMeshAgent m_Agent = null;

	/// <summary>
	/// A path on the nav mesh.
	/// </summary>
	private NavMeshPath m_NavPath = null;

	/// <summary>
	/// Points on the map for the AI to wander to when it can't reach the player.
	/// </summary>
	public Transform[] m_WanderPoints;

	private RaycastHit m_VisionRaycastHit = new RaycastHit();

	private float m_DeaggroTimer = 10.0f;

    /// <summary>
	/// On startup.
	/// </summary>
    void Awake()
    {
		m_Agent = GetComponent<NavMeshAgent>();
		m_NavPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
		Physics.Raycast(transform.position, (m_PlayerCharacterTransform.position - transform.position), out m_VisionRaycastHit);
		// If Bidge can find a path to the player character and can see them, chase them.
		if (m_VisionRaycastHit.rigidbody != null)
		{
			if (m_VisionRaycastHit.rigidbody.tag == "Player" && m_DeaggroTimer <= 0.0f)
			{
				Debug.Log("See the player!");
				m_Agent.destination = m_PlayerCharacterTransform.position;
				Debug.DrawLine(transform.position, m_VisionRaycastHit.point, new Color(0, 1, 0, 1));
			}
			// Else, wander.
			else
			{
				Debug.Log("Can't see the player!");
				Wander();
				Debug.DrawLine(transform.position, m_VisionRaycastHit.point, new Color(1, 0, 0, 1));
			}
		}
		else
		{
			Wander();
			Debug.DrawLine(transform.position, m_VisionRaycastHit.point, new Color(1, 0, 0, 1));
		}

		if (m_DeaggroTimer > 0.0f)
		{
			m_DeaggroTimer -= Time.deltaTime;
		}
	}

	private void Wander()
	{
		if (Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
			m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
	}

	public void SetDeaggroTimer(float deaggroTimer)
	{
		m_DeaggroTimer = deaggroTimer;
	}
}