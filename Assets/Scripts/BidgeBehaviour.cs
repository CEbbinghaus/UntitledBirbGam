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
		// If Bidge can't find a path to the player character, wander to a point.
		if (!m_Agent.CalculatePath(m_PlayerCharacterTransform.position, m_NavPath))
		{
			if (Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
				m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
		}
		// Else, move to the player.
		else
			m_Agent.destination = m_PlayerCharacterTransform.position;
    }
}