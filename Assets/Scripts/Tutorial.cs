using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[SerializeField]
	Camera m_Camera;

	[SerializeField]
	Transform[] m_CameraPositions = new Transform[0];

	string[] m_TextPrompts = new string[]
	{
		"Collect Food!",
		"Deliver it to your nest for points!",
		"Avoid Bidge...",
		"...and Frisbies!",
		"Go for the high score!"
	};

	[SerializeField]
	TextMeshProUGUI m_TextPromptDisplay;

	int m_CurrentIndex = 0;

	bool m_InTutorial = true;

	// Start is called before the first frame update
	void Awake()
	{
		if (m_Camera == null)
			m_Camera = GetComponent<Camera>();
	}

	private void Start()
	{
		Time.timeScale = 0;
		m_TextPromptDisplay.text = m_TextPrompts[0];
	}

	private void Update()
	{
		if (m_InTutorial)
		{
			if (Input.GetButtonDown("Submit"))
			{
				m_CurrentIndex++;
				m_TextPromptDisplay.text = m_TextPrompts[m_CurrentIndex];
				if (m_CurrentIndex == m_CameraPositions.Length)
				{
					Time.timeScale = 1;
					m_InTutorial = false;
					m_Camera.enabled = false;
					return;
				}
			}

			m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, m_CameraPositions[m_CurrentIndex].transform.position, 10f);
			m_Camera.transform.rotation = Quaternion.RotateTowards(m_Camera.transform.rotation, m_CameraPositions[m_CurrentIndex].transform.rotation, 15f);
		}
	}
}
