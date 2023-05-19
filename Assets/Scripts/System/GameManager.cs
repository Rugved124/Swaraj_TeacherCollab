using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject fadeToBlack;

	InputManager inputManager;
	string sceneToLoad;

	void Start()
	{
		inputManager = GetComponent<InputManager>();
	}

	public void SwitchOffInputManager()
	{
		inputManager.DisableInput();
	}

	public void SwitchOnInputManager()
	{
		inputManager.EnableInput();
	}

	public void GameOver()
	{
		StartCoroutine(CoGameOver());
	}

	private void OnEnable()
	{
		NavigationPoint.OnTriggered += HandleOnNavTrigger;
	}

	private void OnDisable()
	{
		NavigationPoint.OnTriggered -= HandleOnNavTrigger;
	}

	void HandleOnNavTrigger(NavigationPoint navTrigger)
	{
		//We go to exitingLevel state if the navigation trigger triggered by the PC is an exit
		if (navTrigger.isExit)
		{
			sceneToLoad = navTrigger.exitToScene;
			SceneManager.LoadScene(sceneToLoad);
		}
	}

	IEnumerator CoGameOver()
	{
		fadeToBlack.SetActive(true);
		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(!string.IsNullOrEmpty(sceneToLoad) ? sceneToLoad : SceneManager.GetActiveScene().name);
	}

	public void GoToVictoryScreen()
	{
		SceneManager.LoadScene(3);
	}
}
