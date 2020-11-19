using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
	Camera m_TutorialCamera;

    [SerializeField]
	Transform[] m_CameraPositions = new Transform[0];

    private string[] m_TextPrompts = new string[] {
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
    CanvasGroup m_MainCanvasGroup;

	[SerializeField]
    CanvasGroup m_TutorialCanvasGroup;


	void Awake()
	{
		if (m_Camera == null)
			m_TutorialCamera = GetComponent<Camera>();
	}

    private void Start()
    {
        Time.timeScale = 0;
        m_TextPromptDisplay.text = m_TextPrompts[0];
        m_MainCanvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (m_InTutorial)
        {
            if (Input.GetButtonDown("Submit"))
            {
                m_CurrentIndex++;
                if (m_CurrentIndex == m_CameraPositions.Length)
                {
                    Time.timeScale = 1;
                    m_InTutorial = false;
                    m_TutorialCamera.enabled = false;
                    m_MainCanvasGroup.alpha = 1;
                    // TODO: Fade out m_TutorialCanvasGroup over time.
					return;
                }
                m_TextPromptDisplay.text = m_TextPrompts[m_CurrentIndex];
            }

            m_TutorialCamera.transform.position = Vector3.MoveTowards(m_TutorialCamera.transform.position, m_CameraPositions[m_CurrentIndex].transform.position, 10f);
            m_TutorialCamera.transform.rotation = Quaternion.RotateTowards(m_TutorialCamera.transform.rotation, m_CameraPositions[m_CurrentIndex].transform.rotation, 15f);
        }
    }
}
