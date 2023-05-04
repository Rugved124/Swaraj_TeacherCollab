
using UnityEngine;
using UnityEngine.UI; 

public class PauseButton : MonoBehaviour
{
	public UIManager uiManager;
	public Button resumeButton;

	void Start()
	{
		uiManager = FindObjectOfType<UIManager>();
		resumeButton.gameObject.SetActive(false); // make the resume button inactive by default
	}


	void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (!uiManager.isPaused)
			{
				uiManager.PauseGame();
				Debug.Log("PauseGame");

				resumeButton.gameObject.SetActive(true); // activate the resume button
			}
			else
			{
				uiManager.ResumeGame();
				Debug.Log("ResumeGame");

				resumeButton.gameObject.SetActive(false); // hide the resume button
			}
		}
	}

	public void OnResumeButtonClicked()
	{
		uiManager.ResumeGame();
		Debug.Log("ResumeGame");
		resumeButton.gameObject.SetActive(false);
	}
}
