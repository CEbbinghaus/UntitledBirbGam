using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	/// <summary>
	/// The food the player has collected.
	/// </summary>
	private int m_FoodCollected = 0;

	/// <summary>
	/// The maximum amount of food the player can have.
	/// </summary>
	[SerializeField] int m_MaxFoodCollect = 200;

	/// <summary>
	/// How much sandwiches are worth.
	/// </summary>
	[SerializeField] int m_SandwichPoints = 100;

	/// <summary>
	/// The player's current score.
	/// </summary>
	private int m_Score = 0;

	/// <summary>
	/// Player lives.
	/// </summary>
	[SerializeField] int m_Lives = 3;

	[SerializeField] ParticleSystem[] m_Particles = null;

	[SerializeField] float InvulnerabilityTimer = 10.0f;

	[SerializeField]
	AudioInstance SeedPickupSound;

	[SerializeField]
	AudioInstance SandwichPickupSound;

	[SerializeField]
	AudioInstance ScoreSound;

	int collectedSeeds, collectedSandwiches;

	void Start()
	{
		UIManager.instance.ClearFoodUI();
		Spawning.RefreshSpawned();
	}

	// For food and nest interactions
	void OnTriggerEnter(Collider other)
	{
		// Increase the score by the amount of food the player was holding and update scoreboard.
		if (other.CompareTag("Nest"))
		{
			DepositFood();
			return;
		}

		if (m_FoodCollected >= m_MaxFoodCollect)
			return;

		switch (other.tag)
		{
			case "Seed":
				other.gameObject.SetActive(false);

				AudioManager.PlaySound(SeedPickupSound, Vector3.zero);

				m_FoodCollected++;
				collectedSeeds++;

				UIManager.instance.CachedCollectedSeeds = collectedSeeds;
				break;
			case "Sandwich":
				other.gameObject.SetActive(false);

				AudioManager.PlaySound(SandwichPickupSound, Vector3.zero);

				m_FoodCollected += m_SandwichPoints;
				collectedSandwiches++;

				UIManager.instance.CachedCollectedSandwiches = collectedSandwiches;
				break;
		}
	}

	// For world and enemy interactions
	// Move most of this to the bidge and frisbee scripts.
	void OnCollisionEnter(Collision collision)
	{
		if (!m_Particles[2].isPlaying)
		{
			m_Particles[1].Play();
			m_Particles[2].Play();
		}
	}

	void DepositFood()
	{
		if (m_FoodCollected > 0)
		{
			// Play sound
			AudioManager.PlaySound(ScoreSound, Vector3.zero);

			// Update score
			m_Score += m_FoodCollected;
			UIManager.instance.activeElements.scoreText.SetValue(m_Score);
			UIManager.instance.cachedScore += m_Score;

			// Reset collected food
			m_FoodCollected = 0;
			collectedSeeds = 0;
			collectedSandwiches = 0;
			UIManager.instance.ClearFoodUI();

			Spawning.RefreshSpawned();
		}
	}

	public void TakeDamage()
	{
		m_Lives--;
		UIManager.instance.CachedRemainingLives = m_Lives;
		AudioManager.PlaySound("WillHit", transform.position);

		// If the player is out of lives, show the end screen
		if (m_Lives <= 0)
		{
			UIManager.instance.DisplayEndScreen(m_Score);
		}
	}

	/// <summary>
	/// Calculate food encumbrance.
	/// </summary>
	/// <returns>Normalised food collected.</returns>
	public float FoodEncumbrance()
	{
		return (float)m_FoodCollected / m_MaxFoodCollect;
	}
}
