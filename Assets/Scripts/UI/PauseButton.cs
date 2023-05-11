
using UnityEngine;
using UnityEngine.UI; 

public class PauseButton : MonoBehaviour
{
	public UIManager uiManager;
	public Button resumeButton;
	public Button homeButton;

	void Start()
	{
		uiManager = FindObjectOfType<UIManager>();
		resumeButton.gameObject.SetActive(false); // make the resume button inactive by default
        homeButton.gameObject.SetActive(false); // make the resume button inactive by default


    }


	void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (!uiManager.isPaused)
			{
				uiManager.PauseGame();
				Debug.Log("PauseGame");

				EnableButtons();
            }
			else
			{
				uiManager.ResumeGame();
				Debug.Log("ResumeGame");

				DisableButtons();
            }
		}
	}

	public void OnResumeButtonClicked()
	{
		uiManager.ResumeGame();
		Debug.Log("ResumeGame");
        DisableButtons();
    }

	public void OnHomeButtonClicked()
	{
		uiManager.ReturnToMainMenu();
		DisableButtons();
	}

	void DisableButtons()
	{
        resumeButton.gameObject.SetActive(false); // hide the resume button
        homeButton.gameObject.SetActive(false);
    }

    void EnableButtons()
    {
        resumeButton.gameObject.SetActive(true); // activate the resume button
        homeButton.gameObject.SetActive(true);
    }
}
