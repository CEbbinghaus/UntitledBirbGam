using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
	/// <summary>
	/// The food the player has collected.
	/// </summary>
	private int m_FoodCollected = 0;

	/// <summary>
	/// The maximum amount of food the player can have.
	/// </summary>
	public int m_MaxFoodCollect = 200;

	/// <summary>
	/// How much sandwiches are worth.
	/// </summary>
	public int m_SandwichPoints = 100;

	/// <summary>
	/// The player's current score.
	/// </summary>
	private int m_Score = 0;
	
	/// <summary>
	/// The text object that displays the score.
	/// </summary>
	public UICounter m_ScoreText = null;

	/// <summary>
	/// The text object that displays the amount the player is carrying.
	/// </summary>
	public UICounter m_FoodCollectedText = null;

	/// <summary>
	/// Player lives.
	/// </summary>
	public int m_Lives = 3;

	/// <summary>
	/// The text object that displays the player's lives.
	/// </summary>
	public Image[] m_LifeGraphics;

	public GameObject m_EndScreen = null;

	public TextMeshProUGUI m_FinalScore = null;

	public ParticleSystem[] m_Particles = null;

	public FoodHolder sandwichHolder;
	public FoodHolder seedHolder;

	int seeds, sandwiches;

	private void Awake()
	{
		Time.timeScale = 1.0f;
	}

	/// <summary>
	/// When the player collides with a trigger (food or the nest).
	/// </summary>
	/// <param name="other">The object the player collided with.</param>
	private void OnTriggerEnter(Collider other)
	{
		// Increment the food collected by 1.
		if (other.tag == "Seed")
		{
			m_FoodCollected++;
			seeds++;
			other.gameObject.SetActive(false);
			if (seeds < 5)
			{
				seedHolder.food[seeds - 1].enabled = true;
			}
			else
			{
				for (int i = 1; i < 5; i++)
				{
					seedHolder.food[i].enabled = false;
				}
				seedHolder.counter.enabled = true;
				seedHolder.counter.text = "x" + seeds.ToString();
			}
		}
		else if (other.tag == "Sandwich")
		{
			m_FoodCollected += m_SandwichPoints;
			sandwiches++;
			other.gameObject.SetActive(false);
			if (sandwiches < 5)
			{
				sandwichHolder.food[sandwiches - 1].enabled = true;
			}
			else
			{
				for (int i = 1; i < 5; i++)
				{
					sandwichHolder.food[i].enabled = false;
				}
				sandwichHolder.counter.enabled = true;
				sandwichHolder.counter.text = "x" + sandwiches.ToString();
			}
		}
		// Increase the score by the amount of food the player was holding and update scoreboard.
		else if (other.tag == "Nest")
		{
			m_Score += m_FoodCollected;
			m_ScoreText.Value = m_Score;
			// Reset food collected.
			m_FoodCollected = 0;
			if(m_FoodCollectedText)
				m_FoodCollectedText.Value = m_FoodCollected;

			seeds = 0;
			sandwiches = 0;
			for (int i = 0; i < 5; i++)
			{
				sandwichHolder.food[i].enabled = false;
				seedHolder.food[i].enabled = false;
			}
			sandwichHolder.counter.enabled = false;
			seedHolder.counter.enabled = false;


			// Find an object that spawns food and refresh spawns when the player reaches the nest.
			FindObjectOfType<Spawning>().RefreshSpawned();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		print(collision.gameObject.tag);
		// Player collided with Bidge.
		if (collision.gameObject.tag == "Bidge")
		{
			m_Lives--;
			m_LifeGraphics[m_Lives].enabled = false;
		}
		// Player collided with the frisbee, which is active for collision.
		else if (collision.gameObject.tag == "Frisbee" && collision.gameObject.GetComponent<FrisbeeMovement>().GetFired() == true)
		{
			m_Lives--;
			m_LifeGraphics[m_Lives].enabled = false;
		}
		// Player collided with an object, lose all food collected.
		else
		{
			m_FoodCollected = m_FoodCollected - (int)(m_FoodCollected * 0.1);
			m_FoodCollectedText.Value = m_FoodCollected;
			if (!m_Particles[0].isPlaying)
				m_Particles[0].Play();
		}

		if (!m_Particles[1].isPlaying && !m_Particles[2].isPlaying)
		{
			m_Particles[1].Play();
			m_Particles[2].Play();
		}

		if (m_Lives <= 0)
		{
			m_EndScreen.SetActive(true);
			Time.timeScale = 0.0f;
			m_FinalScore.text = m_Score.ToString();
		}
	}

	/// <summary>
	/// Get the food the player has collected.
	/// </summary>
	/// <returns></returns>
	public int GetFoodCollected()
	{
		return m_FoodCollected;
	}
}