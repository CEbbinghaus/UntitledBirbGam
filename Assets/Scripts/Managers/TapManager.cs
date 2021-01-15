using UnityEngine;

internal class TapManager : Singleton<TapManager>
{
	Touch[] lastTouch;

	void Awake()
	{

		RegisterInstance(this, true);
		//Picking 20 arbitrarily. Hope to god no phone supports and no human tries more
		lastTouch = new Touch[20];
		// for (int i = 0; i < 20; ++i)
		// 	Debug.Log(lastTouch[i].deltaTime); //Input.GetTouch(i);
	}

	void Update()
	{
		foreach (Touch t in Input.touches)
		{
			if (lastTouch[t.fingerId].phase == TouchPhase.Began)
			{
				if (t.phase == TouchPhase.Ended)
				{
					if (t.deltaTime < .5)
					{
						Debug.Log("Touch has been confirmed");
					}
				}
			}
		}
		// Input.GetTouch(0)
	}
}
