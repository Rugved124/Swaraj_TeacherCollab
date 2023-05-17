using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimeline : MonoBehaviour
{
	[SerializeField] float durationNoControls = 2f;

	PlayableDirector director;
	GameManager gameManager;
	bool hasPlayed;

	// Start is called before the first frame update
	void Start()
	{
		director = GetComponent<PlayableDirector>();
		gameManager = FindAnyObjectByType<GameManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (hasPlayed) return;

		if (collision.gameObject.layer == LayerMask.NameToLayer("PC"))
		{
			StartCoroutine(CoPreventControls());
			director.Play();
			hasPlayed = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (director.state != PlayState.Playing && hasPlayed)
		{
			//hasPlayed = false;
		}
	}

	IEnumerator CoPreventControls()
	{
		gameManager.SwitchOffInputManager();
		yield return new WaitForSeconds(durationNoControls);
		gameManager.SwitchOnInputManager();
	}
}
