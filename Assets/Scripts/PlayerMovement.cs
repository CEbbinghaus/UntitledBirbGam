using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	/// <summary>
	/// The directions the player is moving the character.
	/// </summary>
	private Vector3 m_PlayerMovement = Vector3.zero;

	/// <summary>
	/// The Rigidbody of the player character.
	/// </summary>
	private Rigidbody m_Rigid = null;

	/// <summary>
	/// The speed of the player character.
	/// </summary>
	public float m_Speed = 3;

	//private Transform m_MainCamera = null;
	//
	//private float m_MainCameraYPos = 0.0f;

    void Awake()
    {
		m_Rigid = GetComponent<Rigidbody>();

		//m_MainCamera = Camera.main.transform;
		//m_MainCameraYPos = m_MainCamera.position.y;
    }

    // Update is called once per frame
    void Update()
    {
		m_PlayerMovement = Vector3.zero;

		m_PlayerMovement += Vector3.right * Input.GetAxis("Horizontal");
		m_PlayerMovement += Vector3.forward * Input.GetAxis("Vertical");
    }

	private void FixedUpdate()
	{
		m_Rigid.AddForce((m_PlayerMovement * (m_Speed * 100)) * Time.deltaTime, ForceMode.Impulse);
		//m_MainCamera.position = new Vector3(transform.position.x, m_MainCameraYPos, transform.position.z);
	}
}