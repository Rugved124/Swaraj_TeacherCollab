using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneLoader : MonoBehaviour
{
    [SerializeField] VictoryScreen vs;
    [SerializeField] GameManager gm;
    void Start()
    {
        gm = FindObjectOfType <GameManager>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        gm.GoToVictoryScreen();
	}
}
