using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
	public UIManager uiManager;
	public Button mainMenu;
	[SerializeField] float victoryScreenTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(AutoReturnMainMenu());
    }
	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	IEnumerator AutoReturnMainMenu()
	{
		yield return new WaitForSeconds(victoryScreenTime);
		SceneManager.LoadScene(0);
	}
}
