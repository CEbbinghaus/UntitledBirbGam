using UnityEngine;
using UnityEngine.UI;

public class CapacityBar : MonoBehaviour
{
	public Image image;
	PlayerManager playerManager;

	void Start()
	{
		if (image == null)
			image = transform.GetChild(0).GetComponent<Image>();

		playerManager = FindObjectOfType<PlayerManager>();

		if (!playerManager)
		{
			Debug.LogError("Missing PlayerManager!");
			this.enabled = false;
		}
	}

	void Update()
	{

		float foodEncumbrance = Mathf.Min(playerManager.FoodEncumbrance(), 1);

		// Update the capacity bar if the target value is different
		if (image.fillAmount != foodEncumbrance)
			image.fillAmount = Mathf.Lerp(image.fillAmount, foodEncumbrance, Time.deltaTime);

	}
}
