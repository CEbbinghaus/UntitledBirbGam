using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(Rigidbody))]
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
	/// The player manager on the player character, for getting the food collected.
	/// </summary>
	private PlayerManager m_PM;

	/// <summary>
	/// How much encumbrance affects the player's movement.
	/// </summary>
	public float m_EncumbranceModifier = 10.0f;

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
		// Reset player inputs vector.
		m_PlayerMovement = Vector3.zero;

		// Get the inputs.
		m_PlayerMovement += Vector3.right * Input.GetAxis("Horizontal");
		m_PlayerMovement += Vector3.forward * Input.GetAxis("Vertical");
		transform.LookAt(transform.position + m_PlayerMovement, Vector3.up);
    }

	/// <summary>
	/// Physics update.
	/// </summary>
	private void FixedUpdate()
	{
		// Make sure the character moves only when there is input from the player.
		if (Vector3.Magnitude(m_PlayerMovement) > 0.0f)
		{
			// Add force to the player to move them.
			// Speed is multiplied by 100 for significant movement.
			m_Rigid.AddForce((m_PlayerMovement * ((m_Speed * 100) - (FoodEncumbrance() * m_EncumbranceModifier))) * Time.deltaTime, ForceMode.Impulse);
			Debug.Log(FoodEncumbrance() * m_EncumbranceModifier);
		}
	}

	public float FoodEncumbrance()
	{
		return (float)m_PM.GetFoodCollected() / m_PM.m_MaxFoodCollect;
	}
}