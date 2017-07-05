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

        m_mouseManager = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseManager>();

        InvokeRepeating("updateCounters", 2.0f, 1.0f);
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
        woodCount += 10 * resourcesCounter[0];
        ironCount += 2 * resourcesCounter[1];
        stoneCount += 5 * resourcesCounter[2];
        foodCount += 10 * resourcesCounter[3];
        foodCount += 20 * resourcesCounter[4];
        //foodCount -= 2 * einwohnerzahl;

    }

    //Gibt weiter ob genügend Rohstoffe für ein Gebäude vorhanden sind
    public bool checkBuildingCosts(string buildingName)
    {
        switch (buildingName)
        {
            case ("Woodcutter"):
                if(woodCount >= 100)
                {
                    return true;
                }
                return false;

            case ("Ironfeeder"):
                if (woodCount >= 500 && stoneCount >= 250)
                {
                    return true;
                }
                return false;
            case ("Stonefeeder"):
                if(woodCount >= 400)
                {
                    return true;
                }
                return false;
            case ("LivingHouse"):
                if (woodCount >= 400)
                {
                    return true;
                }
                return false;
            case ("Church"):
                if (woodCount >= 800 && stoneCount >= 800 && ironCount >= 400)
                {
                    return true;
                }
                return false;
            case ("Brauerei"):
                if (woodCount >= 200 && stoneCount >= 600 && ironCount >= 200)
                {
                    return true;
                }
                return false;
            case ("Sauerkrauterie"):
                if (woodCount >= 400 && stoneCount >= 400 && ironCount >= 400)
                {
                    return true;
                }
                return false;
            case ("Wahrzeichen"):
                if (woodCount >= 3000 && stoneCount >= 2000 && ironCount >= 2000)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public void reduceMaterials(string buildingName)
    {
        switch (buildingName)
        {
            case ("Woodcutter"):
                woodCount -= 100;
                break;
            case ("Ironfeeder"):
                woodCount -= 500;
                stoneCount -= 250;
                break;
            case ("Stonefeeder"):
                woodCount -= 400;
                break;
            case ("LivingHouse"):
                woodCount -= 400;
                break;
            case ("Brauerei"):
                woodCount -= 200;
                stoneCount -= 600;
                ironCount -= 200;
                break;
            case ("Sauerkrauterie"):
                woodCount -= 400;
                stoneCount -= 400;
                ironCount -= 400;
                break;
            case ("Church"):

                woodCount -= 800;
                stoneCount -= 800;
                ironCount -= 400;
                break;
            case ("Wahrzeichen"):
                woodCount -= 3000;
                stoneCount -= 2000;
                ironCount -= 2000;
                break;
        }
    }
}
