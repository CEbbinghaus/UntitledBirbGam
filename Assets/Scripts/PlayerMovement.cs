using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	/// <summary>
	/// The players Velocity
	/// </summary>
	Vector3 m_velocity = Vector3.zero;
	
	/// <summary>
	/// The directions the player is moving.
	/// </summary>
	Vector3 m_movementDirection = Vector3.zero;

	/// <summary>
	/// The Rigidbody of the player character.
	/// </summary>
	private Rigidbody m_Rigid = null;

	/// <summary>
	/// The amount of Dampening on the character
	/// </summary>
	[Range(0, 1)]
	public float m_Damp = .8f;

	/// <summary>
	/// The speed of the player character.
	/// </summary>
	public float m_Speed = 3;

	/// <summary>
	/// The Rotational speed of the player character.
	/// </summary>
	public float m_RotSpeed = 1;
	
	/// <summary>
	/// The player manager on the player character, for getting the food collected.
	/// </summary>
	private PlayerManager m_PM;

	/// <summary>
	/// How much encumbrance affects the player's movement.
	/// </summary>
	public float m_EncumbranceModifier = 10.0f;

	private float m_RotationTime = 0.0f;

	/// <summary>
	/// On startup.
	/// </summary>
    void Awake()
    {
		// Get the rigidbody.
		m_Rigid = GetComponent<Rigidbody>();

		m_PM = GetComponent<PlayerManager>();
    }

    /// <summary>
	/// Update.
	/// </summary>
    void Update()
    {
		m_velocity = m_Rigid.velocity;

		Vector3 playerDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

		m_movementDirection = Vector3.Lerp(m_movementDirection, playerDir, Time.deltaTime * m_RotSpeed).normalized;

		m_velocity += m_movementDirection * ((m_Speed * 100) - (FoodEncumbrance() * m_EncumbranceModifier)) * Time.deltaTime;


		m_velocity *= m_Damp;

		// // Get the inputs.
		// m_PlayerMovement += Vector3.right * Input.GetAxis("Horizontal");
		// m_PlayerMovement += Vector3.forward * Input.GetAxis("Vertical");


		Vector3 nvel = m_velocity.normalized;
		transform.LookAt(transform.position + nvel, Vector3.up);
    }

	/// <summary>
	/// Physics update.
	/// </summary>
	private void FixedUpdate()
	{
		m_Rigid.velocity = m_velocity;
	}
	
	/// <summary>
	/// Calculate food encumbrance.
	/// </summary>
	/// <returns>Normalised food collected.</returns>
	public float FoodEncumbrance()
	{
		return (float)m_PM.GetFoodCollected() / m_PM.m_MaxFoodCollect;
	}
}