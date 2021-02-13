using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerButtonToMenu : ControllerButton
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
		SceneManager.LoadScene("Opening Scene", LoadSceneMode.Single);
	}

	public override void Press()
	{
		base.Press();
		Time.timeScale = 1;
		StartCoroutine(FadeOut(1f, canvasGroup.alpha, 1));
	}
}
