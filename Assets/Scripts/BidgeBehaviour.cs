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
			if (m_VisionRaycastHit.rigidbody.tag == "Player")
			{
				Debug.Log("See the player!");
				m_Agent.destination = m_PlayerCharacterTransform.position;
			}
			// Else, wander.
			else
			{
				Debug.Log("Can't see the player!");
				if (Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
					m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
			}
		}
    }
}