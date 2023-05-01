using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void OnEnable()
	{
		NavigationPoint.OnTriggered += HandleOnNavTrigger;
	}

	private void OnDisable()
	{
		NavigationPoint.OnTriggered -= HandleOnNavTrigger;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	void HandleOnNavTrigger(NavigationPoint navTrigger)
	{
		//We go to exitingLevel state if the navigation trigger triggered by the PC is an exit
		if (navTrigger.isExit)
		{
			SceneManager.LoadScene(navTrigger.exitToScene);
		}
	}

}