using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
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

	/// <summary>
	/// On startup.
	/// </summary>
    void Awake()
    {
		// Get the rigidbody.
		m_Rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
	/// Update.
	/// </summary>
    void Update()
    {
		// Reset player inputs vector.
		m_PlayerMovement = Vector3.zero;

		// Get the inputs.
		m_PlayerMovement += Vector3.right * Input.GetAxis("Horizontal");
		m_PlayerMovement += Vector3.forward * Input.GetAxis("Vertical");
    }

	/// <summary>
	/// Physics update.
	/// </summary>
	private void FixedUpdate()
	{
		// Add force to the player to move them.
		// Speed is multiplied by 100 for significant movement.
		m_Rigid.AddForce((m_PlayerMovement * (m_Speed * 100)) * Time.deltaTime, ForceMode.Impulse);
	}
}