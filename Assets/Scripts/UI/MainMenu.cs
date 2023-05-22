using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	SvgManager svgManager;

	private void Start()
	{
		svgManager = FindObjectOfType<SvgManager>();
	}
	public void StartGame()
	{
		try
		{
			print("SvgManager.SvgData.currentSceneName = " + SvgManager.SvgData.currentSceneName); //TEST
			LaunchGame(SvgManager.SvgData.currentSceneName);
		}
		catch
		{
			Debug.LogError("ERROR When trying to read SvgManager.SvgData.currentSceneName");
		}

		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	void LaunchGame(string sceneToLoad)
	{
		if (!string.IsNullOrEmpty(sceneToLoad))
		{
			SceneManager.LoadScene(sceneToLoad);
		}
		else
		{
			print("Warning: no scene to load");//TEST
		}
	}

	public void DeleteSaveGame()
	{
		svgManager.DeleteSavegameDEBUG();
	}
}
