using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private int numberOfArrows = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddArrow()
    {
        numberOfArrows++;
        Debug.Log("number of arrows = " + numberOfArrows);
    }



}
