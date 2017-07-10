using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources_Counter : MonoBehaviour {

    public Text wood_Counter;
    public Text iron_Counter;
    public Text stone_Counter;
    public Text food_Counter;
    public Text einwohner_Counter;
    public Text einwohner_Gesamt;

    public MouseManager m_mouseManager;

    public int woodCount;
    public int ironCount;
    public int stoneCount;
    public int foodCount;
    private int einwohnerCount;
    private int einwohnerGesamt;
    

	// Use this for initialization
	void Start () {
        woodCount = 1000;
        ironCount = 100;
        stoneCount = 100;
        foodCount = 5000;
        einwohnerCount = 5;
        einwohnerGesamt = 10;

        m_mouseManager = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseManager>();

        InvokeRepeating("updateCounters", 2.0f, 1.0f);
	}


    public int[] getResourcesCounter()
    {
        return m_mouseManager.getResourcesCounter();
    }

    // Update is called once per frame
    void Update () {
        wood_Counter.text = woodCount.ToString();
        iron_Counter.text = ironCount.ToString();
        stone_Counter.text = stoneCount.ToString();
        food_Counter.text = foodCount.ToString();
        einwohner_Counter.text = einwohnerCount.ToString();
        einwohner_Gesamt.text = einwohnerGesamt.ToString();

	}

    void updateCounters()
    {
        int[] resourcesCounter = m_mouseManager.getResourcesCounter();
        woodCount += 10 * resourcesCounter[0];
        ironCount += 2 * resourcesCounter[6];
        stoneCount += 5 * resourcesCounter[4];
        foodCount += 50 * resourcesCounter[3];
        foodCount += 60 * resourcesCounter[5];
        foodCount -= 1 * einwohnerCount;

    }

    //Gibt weiter ob genügend Rohstoffe für ein Gebäude vorhanden sind
    public bool checkBuildingCosts(string buildingName)
    {
        int einwohnerFree = einwohnerGesamt - einwohnerCount;
        switch (buildingName)
        {
            case ("Woodcutter"):
                if(woodCount >= 100 && einwohnerFree >= 4)
                {
                    return true;
                }
                return false;

            case ("Ironfeeder"):
                if (woodCount >= 500 && stoneCount >= 250 && einwohnerFree >= 6)
                {
                    return true;
                }
                return false;
            case ("Stonefeeder"):
                if(woodCount >= 400 && einwohnerFree >= 6)
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
                if (woodCount >= 1200 && einwohnerFree >= 10)
                {
                    return true;
                }
                return false;
            case ("Brauerei"):
                if (woodCount >= 200 && stoneCount >= 600 && einwohnerFree >= 8)
                {
                    return true;
                }
                return false;
            case ("Bäcker"):
                if (woodCount >= 800 && einwohnerFree >= 5)
                {
                    return true;
                }
                return false;
            case ("Schmiede"):
                if (woodCount >= 800 && stoneCount >= 600 && ironCount >= 200 && einwohnerFree >= 5)
                {
                    return true;
                }
                return false;
            case ("Kaserne"):
                if (woodCount >= 1600 && stoneCount >= 1200 && ironCount >= 600 && einwohnerFree >= 10)
                {
                    return true;
                }
                return false;
            case ("Schule"):
                if (woodCount >= 800 && stoneCount >= 600 && ironCount >= 200 && einwohnerFree >= 5)
                {
                    return true;
                }
                return false;
            case ("Universität"):
                if (woodCount >= 1600 && stoneCount >= 1200 && ironCount >= 600 && einwohnerFree >= 10)
                {
                    return true;
                }
                return false;
            case ("Wahrzeichen"):
                if (woodCount >= 3000 && stoneCount >= 2000 && ironCount >= 2000 && einwohnerFree >= 40)
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
                woodCount -= 200;
                einwohnerCount += 4;
                break;
            case ("Ironfeeder"):
                woodCount -= 500;
                stoneCount -= 250;
                einwohnerCount += 6;
                break;
            case ("Stonefeeder"):
                woodCount -= 400;
                einwohnerCount += 6;
                break;
            case ("LivingHouse"):
                woodCount -= 400;
                einwohnerGesamt += 20;
                break;
            case ("Brauerei"):
                woodCount -= 200;
                stoneCount -= 600;
                einwohnerCount += 8;
                break;
            case ("Bäcker"):
                woodCount -= 800;
                einwohnerCount += 5;
                break;
            case ("Schmiede"):
                woodCount -= 800;
                stoneCount -= 600;
                ironCount -= 200;
                einwohnerCount += 5;
                break;
            case ("Kaserne"):
                woodCount -= 1600;
                stoneCount -= 1200;
                ironCount -= 600;
                einwohnerCount += 10;
                break;
            case ("Schule"):
                woodCount -= 800;
                stoneCount -= 600;
                ironCount -= 200;
                einwohnerCount += 5;
                break;
            case ("Universität"):
                woodCount -= 1600;
                stoneCount -= 1200;
                ironCount -= 600;
                einwohnerCount += 10;
                break;
            case ("Church"):

                woodCount -= 1200;
                einwohnerCount += 10;
                break;
            case ("Wahrzeichen"):
                woodCount -= 3000;
                stoneCount -= 2000;
                ironCount -= 2000;
                einwohnerCount += 40;
                break;
        }
    }

    public void destroyedBuilding(string buildingName)
    {
        switch (buildingName)
        {
            case ("Woodcutter"):
                woodCount += 50;
                einwohnerCount -= 4;
                break;
            case ("Ironfeeder"):
                woodCount += 250;
                stoneCount += 120;
                einwohnerCount -= 6;
                break;
            case ("Stonefeeder"):
                woodCount += 200;
                einwohnerCount -= 6;
                break;
            case ("LivingHouse"):
                woodCount += 200;
                einwohnerGesamt -= 20;
                break;
            case ("Brauerei"):
                woodCount += 100;
                stoneCount += 300;
                einwohnerCount -= 8;
                break;
            case ("Bäcker"):
                woodCount += 400;
                einwohnerCount -= 5;
                break;
            case ("Church"):
                woodCount += 600;
                einwohnerCount -= 10;
                break;
            case ("Schmiede"):
                woodCount += 400;
                stoneCount += 300;
                ironCount += 100;
                einwohnerCount -= 5;
                break;
            case ("Kaserne"):
                woodCount += 800;
                stoneCount += 600;
                ironCount += 300;
                einwohnerCount -= 10;
                break;
            case ("Schule"):
                woodCount += 400;
                stoneCount += 300;
                ironCount += 100;
                einwohnerCount -= 5;
                break;
            case ("Universität"):
                woodCount += 800;
                stoneCount += 600;
                ironCount += 300;
                einwohnerCount -= 10;
                break;
        }
    }
}
