using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources_Counter : MonoBehaviour {

    public Text wood_Counter;
    public Text iron_Counter;
    public Text stone_Counter;
    public Text food_Counter;

    public MouseManager m_mouseManager;

    public int woodCount;
    public int ironCount;
    public int stoneCount;
    public int foodCount;

	// Use this for initialization
	void Start () {
        woodCount = 1000;
        ironCount = 100;
        stoneCount = 100;
        foodCount = 1000;

        InvokeRepeating("updateCounters", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        wood_Counter.text = woodCount.ToString();
        iron_Counter.text = ironCount.ToString();
        stone_Counter.text = stoneCount.ToString();
        food_Counter.text = foodCount.ToString();
	}

    void updateCounters()
    {
        int[] resourcesCounter = m_mouseManager.getResourcesCounter();
        woodCount += 100*resourcesCounter[0];
        ironCount += 10 * resourcesCounter[1];
        stoneCount += 100 * resourcesCounter[2];
        foodCount += 100 * resourcesCounter[3];
    }
    
}
