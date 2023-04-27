using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the spawn position of the PC (which checkpoint of the level) according to saved data
/// </summary>
public class PCPositioner : MonoBehaviour
{
	PCController pcController;
	NavigationPoint mySpawnpoint;

	private void Awake()
	{
		pcController = GetComponent<PCController>();
	}

	void Start()
	{
		if (SceneManager.GetActiveScene().name == SvgManager.SvgData.currentSceneName && FindSpawnPoint())
		{
			Reposition();
		}

		enabled = false; //because this script is useless after initialization
	}

	bool FindSpawnPoint()
	{
		int checkptID = SvgManager.SvgData.currCheckpointID;

		NavigationPoint[] ntArray = FindObjectsOfType<NavigationPoint>();

		foreach (NavigationPoint nt in ntArray)
		{
			if (nt.iD == checkptID)
			{
				mySpawnpoint = nt;
				break;
			}
		}

		//DEBUG
		if (mySpawnpoint == null)
		{
			Debug.LogErrorFormat("<color=red>ERROR: No entry point with ID " + checkptID + " was found in the current scene (" + SvgManager.SvgData.currentSceneName + ")</color>");//DEBUG
			return false;
		}

		//Executed only if we didn't return false just above
		return true;
	}

	void Reposition()
	{
		Vector2 spawnPos = new Vector2(mySpawnpoint.transform.position.x, mySpawnpoint.transform.position.y);
		pcController.InstantReposition(spawnPos, mySpawnpoint.mustPCFaceLeft);
	}
}
