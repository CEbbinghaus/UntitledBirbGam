using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControllerButton : MenuButton
{
	public CanvasGroup canvasGroup;

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

	public override void Press()
	{
		Time.timeScale = 1;
		StartCoroutine(FadeOut(1f, canvasGroup.alpha, 1));
	}
}
