using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

	PauseButton pauseButton;
	public bool isPaused { get; private set; }


	private void Awake()
	{
		pauseButton = FindObjectOfType<PauseButton>();
	}

	// Start is called before the first frame update
	void Start()
    {
		isPaused = false;

	}

	// Update is called once per frame
	void Update()
    {
        
    }



	public void PauseGame()
	{
		Time.timeScale = 0;
		isPaused = true;
		// Add code to show pause menu, if desired
	}
	public void ResumeGame()
	{
		Time.timeScale = 1;
		isPaused = false;
		// Add code to hide pause menu, if desired
	}


	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
