
using UnityEngine;
using UnityEngine.UI; 

public class PauseButton : MonoBehaviour
{
	public GameManager gameManager;
	public Button resumeButton;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		resumeButton.gameObject.SetActive(false); // make the resume button inactive by default
	}


	void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (!gameManager.isPaused)
			{
				gameManager.PauseGame();
				Debug.Log("PauseGame");

				resumeButton.gameObject.SetActive(true); // activate the resume button
			}
			else
			{
				gameManager.ResumeGame();
				Debug.Log("ResumeGame");

				resumeButton.gameObject.SetActive(false); // hide the resume button
			}
		}
	}

	public void OnResumeButtonClicked()
	{
		gameManager.ResumeGame();
		Debug.Log("ResumeGame");
		resumeButton.gameObject.SetActive(false);
	}
}
