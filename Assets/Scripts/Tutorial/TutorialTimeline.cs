using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialTimeline : MonoBehaviour
{
    public PlayableDirector director;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("PC"))
        {
            gameManager.SwitchOffInputManager();    
            hasPlayed = true;
            director.Play();

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing && hasPlayed)
        {
            hasPlayed = false;
            gameManager.SwitchOnInputManager(); 
        } 
    }
}
