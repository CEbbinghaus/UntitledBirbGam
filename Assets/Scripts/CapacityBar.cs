using UnityEngine;
using UnityEngine.UI;

public class CapacityBar : MonoBehaviour
{
    public Image image;
    PlayerManager playerManager;

    void Start()
    {
        image = GetComponent<Image>();
        playerManager = FindObjectOfType<PlayerManager>();
        if (!playerManager)
        {
            Debug.LogError("Missing PlayerManager!");
            this.enabled = false;
        }
    }

    void Update()
    {
        // Update the capacity bar if the target value is different
        if (image.fillAmount != Mathf.Min(playerManager.FoodEncumbrance(), 1))
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, Mathf.Min(playerManager.FoodEncumbrance(), 1), Time.deltaTime);
        }
    }
}
