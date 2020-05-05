using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeMovement : MonoBehaviour
{
	public Transform m_PlayerTransform = null;

	private bool m_Fired = false;

	private Rigidbody m_Rigid = null;

	public float m_Speed = 3.0f;

	private MeshRenderer m_MeshRend = null;

	private SphereCollider m_Collider = null;

	public float m_FireTimer = 3.0f;

	private float m_MaxFireTimer = 0.0f;

	private float m_SpawnPosHeight = 0.0f;

	private void Awake()
	{
		m_Rigid = GetComponent<Rigidbody>();
		m_MeshRend = GetComponent<MeshRenderer>();
		m_Collider = GetComponent<SphereCollider>();

		m_MaxFireTimer = m_FireTimer;

		m_SpawnPosHeight = Camera.main.transform.position.y - transform.position.y;
	}

	// Update is called once per frame
	void Update()
    {
		if (m_Fired)
		{
			if (!m_MeshRend.isVisible)
			{
				m_MeshRend.enabled = false;
				m_Collider.enabled = false;

				m_Fired = false;
			}
		}
		else
		{
			m_MeshRend.enabled = true;
			m_Collider.enabled = true;

			transform.position = m_PlayerTransform.right + Camera.main.ViewportToWorldPoint(new Vector3(1.3f, 0.5f, m_SpawnPosHeight));
			transform.LookAt(m_PlayerTransform);
			m_Rigid.velocity = transform.forward * m_Speed;
			m_Fired = true;
		}
		Debug.Log(m_Rigid.velocity.magnitude);
    }

	private void OnCollisionEnter(Collision collision)
	{
		m_MeshRend.enabled = false;
		m_Collider.enabled = false;

		m_Fired = false;
	}
}
