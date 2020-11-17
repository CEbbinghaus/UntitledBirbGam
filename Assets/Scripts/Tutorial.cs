using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Camera m_MainCamera;

    public Transform[] m_CameraPositions = new Transform[0];

    public string[] m_TextPrompts = new string[] {
        "Collect Food!",
        "Deliver it to your nest for points!",
        "Avoid Bidge...",
        "...and Frisbies!",
        "Go for the high score!"
    };

    public TextMeshProUGUI m_TextPromptDisplay;

    private int m_CurrentIndex = 0;

    private bool m_InTutorial = true;

    // Start is called before the first frame update
    void Awake()
    {
        m_MainCamera = GetComponent<Camera>();
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
                if (m_CurrentIndex == m_CameraPositions.Length)
                {
                    Time.timeScale = 1;
                    m_InTutorial = false;
                    m_MainCamera.enabled = false;
                }
                m_TextPromptDisplay.text = m_TextPrompts[m_CurrentIndex];
            }

            m_MainCamera.transform.position = Vector3.MoveTowards(m_MainCamera.transform.position, m_CameraPositions[m_CurrentIndex].transform.position, 10f);
            m_MainCamera.transform.rotation = Quaternion.RotateTowards(m_MainCamera.transform.rotation, m_CameraPositions[m_CurrentIndex].transform.rotation, 15f);
        }
    }
}
