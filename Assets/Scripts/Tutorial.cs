using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[SerializeField]
	Camera m_TutorialCamera;

	[SerializeField]
	GameObject m_MainCameraObject;

	[SerializeField]
	Transform[] m_CameraPositions = new Transform[0];

	private readonly string[] m_TextPrompts = new string[]
	{
		"Collect Food!",
		"Deliver it to your nest for points!",
		"Avoid Bidge...",
		"...and Frisbies!",
		"Go for the high score!"
	};

	[SerializeField]
	TextMeshProUGUI m_TextPromptDisplay;

	private int m_CurrentIndex = 0;

	private bool m_InTutorial = true;

	[SerializeField]
	float turnSpeed = 15.0f;

	[SerializeField]
	float moveSpeed = 10.0f;

	[SerializeField]
	float FadeOutSpeed = .5f;

	[SerializeField]
	CanvasGroup m_MainCanvasGroup = null;

	[SerializeField]
	CanvasGroup m_TutorialCanvasGroup = null;

	void Awake()
	{
		if (m_TutorialCamera == null)
			m_TutorialCamera = GetComponent<Camera>();
		m_MainCameraObject = Camera.main.gameObject;
	}

	private void Start()
	{
		Time.timeScale = 0;
		m_TextPromptDisplay.text = m_TextPrompts[0];
		m_MainCanvasGroup.alpha = 0;
		m_MainCameraObject.SetActive(false);
	}

	private void Update()
	{
		if (m_InTutorial)
		{
			if (Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(0))
			{
				m_CurrentIndex++;
				m_TextPromptDisplay.text = m_TextPrompts[m_CurrentIndex];
				if (m_CurrentIndex == m_CameraPositions.Length)
				{
					Time.timeScale = 1;
					m_InTutorial = false;
					m_TutorialCamera.enabled = false;
					m_MainCanvasGroup.alpha = 1;
					m_MainCameraObject.SetActive(true);
					return;
				}
			}

			m_TutorialCamera.transform.position = Vector3.MoveTowards(m_TutorialCamera.transform.position, m_CameraPositions[m_CurrentIndex].transform.position, moveSpeed);
			m_TutorialCamera.transform.rotation = Quaternion.RotateTowards(m_TutorialCamera.transform.rotation, m_CameraPositions[m_CurrentIndex].transform.rotation, turnSpeed);
		}
		else if (m_TutorialCanvasGroup.alpha > 0)
		{
			m_TutorialCanvasGroup.alpha -= Time.deltaTime * FadeOutSpeed;
			// Disable the tutorial script once it's finished fading
			if (m_TutorialCanvasGroup.alpha == 0)
				this.enabled = false;
		}
	}
}
