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
	/// The text object that displays the score.
	/// </summary>
	[SerializeField] UICounter m_ScoreText = null;

	/// <summary>
	/// The text object that displays the amount the player is carrying.
	/// </summary>
	[SerializeField] UICounter m_FoodCollectedText = null;

	/// <summary>
	/// Player lives.
	/// </summary>
	[SerializeField] int m_Lives = 3;

	[SerializeField] Image EncumbranceBar;

	/// <summary>
	/// The text object that displays the player's lives.
	/// </summary>
	[SerializeField] Image[] m_LifeGraphics;

	[SerializeField] GameObject m_EndScreen = null;

	[SerializeField] UICounter m_FinalScore = null;

	[SerializeField] ParticleSystem[] m_Particles = null;

	[SerializeField] float m_DeaggroTimer = 10.0f;

	[SerializeField]
	FoodHolder sandwichHolder;
	[SerializeField]
	FoodHolder seedHolder;

	int seeds, sandwiches;

	void Awake()
	{
		Time.timeScale = 1;
		Spawning.RefreshSpawned();
		UpdateCollectedUI(seedHolder, seeds);
		UpdateCollectedUI(sandwichHolder, sandwiches);
	}

	void OnTriggerEnter(Collider other)
	{
		// Increase the score by the amount of food the player was holding and update scoreboard.
		if (other.tag == "Nest")
		{
			m_Score += m_FoodCollected;

			if (m_FoodCollected > 0)
				AudioManager.Emit(AudioManager.Event.Score);

			Spawning.RefreshSpawned();

			m_ScoreText?.SetValue(m_Score);

			// Reset food collected.
			m_FoodCollected = 0;
			m_FoodCollectedText?.SetValue(m_FoodCollected);

			seeds = 0;
			sandwiches = 0;

			UpdateCollectedUI(seedHolder, seeds);
			UpdateCollectedUI(sandwichHolder, sandwiches);
			return;
		}

		if (m_FoodCollected >= m_MaxFoodCollect)
			return;

		switch (other.tag)
		{
			case "Seed":
				m_FoodCollected++;
				AudioManager.Emit(AudioManager.Event.SmallPickup);

				seeds++;
				other.gameObject.SetActive(false);

				UpdateCollectedUI(seedHolder, seeds);
				break;
			case "Sandwich":
				m_FoodCollected += m_SandwichPoints;
				AudioManager.Emit(AudioManager.Event.BigPickup);

				sandwiches++;
				other.gameObject.SetActive(false);

				UpdateCollectedUI(sandwichHolder, sandwiches);
				break;
		}
		UpdateEncumberanceBar();
	}

	void UpdateCollectedUI(FoodHolder holder, int collected)
	{
		if (collected == 0)
		{
			for (int i = 0; i < 5; i++)
			{
				holder.food[i].enabled = false;
			}
			holder.counter.enabled = false;
			return;
		}
		if (collected < 5)
		{
			holder.food[collected - 1].enabled = true;
		}
		else
		{
			for (int i = 1; i < 5; i++)
			{
				holder.food[i].enabled = false;
			}
			holder.counter.enabled = true;
			holder.counter.text = "x" + collected;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Bidge":
				BidgeBehaviour behaviour = collision.gameObject.GetComponent<BidgeBehaviour>();

				if (behaviour.m_DeaggroTimer > 0)return;

				behaviour.SetDeaggroTimer(m_DeaggroTimer);

				goto case "Frisbee";
			case "Frisbee":
				if (collision.gameObject.GetComponent<FrisbeeMovement>()?.GetFired() == false)
					return;

				m_Lives--;
				m_LifeGraphics[m_Lives].enabled = false;
				break;
		}

		{
			//m_FoodCollected = m_FoodCollected - (int)(m_FoodCollected * 0.1);
			//m_FoodCollectedText.Value = m_FoodCollected;
			// if (!m_Particles[0].isPlaying)
			// 	m_Particles[0].Play();
		}

		if (!m_Particles[2].isPlaying)
		{
			m_Particles[1].Play();
			m_Particles[2].Play();
		}

		if (m_Lives <= 0)
		{
			m_EndScreen.SetActive(true);
			Time.timeScale = 0.0f;
			m_FinalScore.Value = m_Score;
		}
	}

	void UpdateEncumberanceBar()
	{
		if (EncumbranceBar)
			EncumbranceBar.fillAmount = Mathf.Min(FoodEncumbrance(), 1);
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
