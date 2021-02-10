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
	/// Continue to move forwards even if no keys are pressed
	/// </summary>
	public bool ContinousMovement = true;

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

	[SerializeField]
	bl_Joystick joystick;

	void Awake()
	{
		// Get the rigidbody.
		m_Rigid = GetComponent<Rigidbody>();

		m_PM = GetComponent<PlayerManager>();

		if (joystick == null)
			Debug.LogWarning("Joystick needs to be assigned for mobile to work");

		OrientationManager.onChangeUIOrientation += UpdateActiveJoystick;
	}

	void Update()
	{
		Vector3 playerDir = Vector3.forward;

		if (joystick != null && Application.isMobilePlatform)
			playerDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
		else
			playerDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

		m_movementDirection = Vector3.Lerp(m_movementDirection, playerDir, Time.deltaTime * m_RotSpeed);

		if (ContinousMovement)
			m_movementDirection.Normalize();

		// // Get the inputs.
		// m_PlayerMovement += Vector3.right * Input.GetAxis("Horizontal");
		// m_PlayerMovement += Vector3.forward * Input.GetAxis("Vertical");

		Vector3 nvel = m_velocity.normalized;
		transform.LookAt(transform.position + nvel, Vector3.up);
	}

	private void FixedUpdate()
	{
		if (float.IsNaN(transform.position.x) || float.IsNaN(transform.position.y) || float.IsNaN(transform.position.z))return;
		m_velocity = m_Rigid.velocity;

		m_velocity += (m_movementDirection * ((m_Speed * 100) - (m_PM.FoodEncumbrance() * m_EncumbranceModifier))) * Time.fixedDeltaTime;

		m_velocity *= m_Damp;

		m_Rigid.velocity = m_velocity;
	}

	public void UpdateActiveJoystick(ScreenOrientation orientation)
	{
		joystick = UIManager.instance.activeElements.joystickElements.joystick;
	}
}
