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

	public int m_MaxFoodCollect = 200;

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
	/// When the player collides with a trigger (food or the nest).
	/// </summary>
	/// <param name="other">The object the player collided with.</param>
	private void OnTriggerEnter(Collider other)
	{
		// Increment the food collected by 1.
		if (other.tag == "Food")
		{
			m_FoodCollected++;
			other.gameObject.SetActive(false);
			//m_FoodCollectedText.text = m_FoodCollected.ToString();
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

	/// <summary>
	/// Get the food the player has collected.
	/// </summary>
	/// <returns></returns>
	public int GetFoodCollected()
	{
		return m_FoodCollected;
	}
}