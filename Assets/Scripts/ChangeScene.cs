using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startGame()
    {
        StartCoroutine(FadeOut(1f, canvasGroup.alpha, 1));
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void endGame()
    {

    }

    IEnumerator FadeOut(float lerpTime, float start, float end)
    {
        float timeAtStart = Time.time;
        float timeSinceStart;
        float percentageComplete = 0;

        while (percentageComplete < 1) // Keeps looping until the lerp is complete
        {
            timeSinceStart = Time.time - timeAtStart;
            percentageComplete = timeSinceStart / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            canvasGroup.alpha = currentValue;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
