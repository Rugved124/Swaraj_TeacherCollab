using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables; 

public class PCTutorialManager : MonoBehaviour
{

    public PlayableDirector director;
    public GameObject tut1trigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == tut1trigger)
        {
            director.Play(); 

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
