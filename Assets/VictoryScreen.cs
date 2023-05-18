using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
	public UIManager uiManager;
	public Button mainMenu;
    // Start is called before the first frame update
    void Start()
    {
		uiManager = GetComponent<UIManager>();
    }
	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
