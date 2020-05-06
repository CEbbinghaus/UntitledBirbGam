using System.Collections;
using System.Collections.Generic;
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
	public Text m_ScoreText = null;

	/// <summary>
	/// The text object that displays the amount the player is carrying.
	/// </summary>
	public Text m_FoodCollectedText = null;

	/// <summary>
	/// Player lives.
	/// </summary>
	public int m_Lives = 3;

	/// <summary>
	/// The text object that displays the player's lives.
	/// </summary>
	public Text m_LivesText = null;

	private void Awake()
	{
		m_LivesText.text = m_Lives.ToString();
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
			other.gameObject.SetActive(false);
			m_FoodCollectedText.text = m_FoodCollected.ToString();
		}
		else if (other.tag == "Sandwich")
		{
			m_FoodCollected += m_SandwichPoints;
			other.gameObject.SetActive(false);
			m_FoodCollectedText.text = m_FoodCollected.ToString();
		}
		// Increase the score by the amount of food the player was holding and update scoreboard.
		else if (other.tag == "Nest")
		{
			m_Score += m_FoodCollected;
			m_ScoreText.text = m_Score.ToString();
			// Reset food collected.
			m_FoodCollected = 0;
			m_FoodCollectedText.text = m_FoodCollected.ToString();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		// Player collided with Bidge.
		if (collision.gameObject.tag == "Bidge")
		{
			m_Lives--;
			m_LivesText.text = m_Lives.ToString();
		}
		// Player collided with the frisbee, which is active for collision.
		else if (collision.gameObject.tag == "Frisbee" && collision.gameObject.GetComponent<FrisbeeMovement>().GetFired() == true)
		{
			m_Lives--;
			m_LivesText.text = m_Lives.ToString();
		}

		if (m_Lives <= 0)
		{
			// Player has no lives, put up end screen.
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