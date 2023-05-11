using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	InputManager inputManager;

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
		StartCoroutine(coGameOver());
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
			SceneManager.LoadScene(navTrigger.exitToScene);
		}
	}

	IEnumerator coGameOver()
	{
		//FindObjectOfType<UIManager>().FadeToBlack();
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
