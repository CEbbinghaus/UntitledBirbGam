using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BidgeBehaviour : MonoBehaviour
{
	public Transform m_PlayerCharacterTransform = null;

	private NavMeshAgent m_Agent = null;

	private NavMeshPath m_NavPath = null;

	private NavMeshHit m_NavHit = new NavMeshHit();

	private Vector3 m_WanderDirection = Vector3.zero;

	private bool m_CanReachPlayer = false;

    // Start is called before the first frame update
    void Awake()
    {
		m_Agent = GetComponent<NavMeshAgent>();
		m_NavPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
		if (!m_Agent.CalculatePath(m_PlayerCharacterTransform.position, m_NavPath))
		{
			Debug.Log("Couldn't find path!");
			Wander();
		}
		else
		{
			m_Agent.destination = m_PlayerCharacterTransform.position;
		}
    }

	private void Wander()
	{
		m_WanderDirection = Random.insideUnitSphere * m_Agent.radius;
		m_WanderDirection += transform.position;

		NavMesh.SamplePosition(m_WanderDirection, out m_NavHit, m_Agent.radius, -1);
		m_Agent.destination = m_NavHit.position;
	}
}