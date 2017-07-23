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
    public int einwohnerCount;
    private int einwohnerGesamt;
    private int goldCount;
    

	// Use this for initialization
	void Start () {
        woodCount = 1000;
        ironCount = 200;
        stoneCount = 200;
        foodCount = 5000;
        goldCount = 0;
        einwohnerCount = 5;
        einwohnerGesamt = 10;

        m_mouseManager = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseManager>();

        InvokeRepeating("updateCounters", 2.0f, 1.0f);
	}

    public void addGold(int number)
    {
        goldCount = number;
    }


    //Methode wird von PopUpManager aufgerufen.
    //wahr, wenn 3 oder mehr Kasernen gebaut
    //false wenn 2 oder weniger
    //dient dazu zu überprüfen, ob Nachbardorf eingenommen werden kann
    public bool getKaserneDorfEinnehmbar()
    {
        if(m_mouseManager.getResourcesCounter()[10] >= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Methode wird von PopUpManager aufgerufen.
    //wahr, wenn genug rohstoffe vorhanden
    //false wenn nicht
    //dient dazu zu überprüfen, ob Nachbardorf eingenommen werden kann
    public bool getUniDorfEinnehmbar()
    {
        if (woodCount >= 2000 && stoneCount >= 1000 && foodCount >= 2000)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Methode wird von PopUpManger aufgerufen
    //zieht Rohstoffe ab
    public void removeResources()
    {
        woodCount -= 2000;
        stoneCount -= 1000;
        foodCount -= 2000;
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

    //gibt die Zufriedenheit der Einwohner bezüglich Essen zurück;
    public float getSatisfactionFood()
    {
        float satisfactionFood = Mathf.Round(foodCount / einwohnerCount);
        if (satisfactionFood > 100) satisfactionFood = 100;
        return satisfactionFood;
    }


    //gibt die Zufriedenheit der Einwohner zurück
    public float getSatisfaction()
    {
        float satisfaction = 0f;
        float satisfactionFood = Mathf.Round(foodCount / einwohnerCount);
        float satisfactionKapelle = 0;
        float satisfactionSchuleSchmiede = 0;
        float satisfactionKaserneUni = 0;
        if (satisfactionFood > 100) satisfactionFood = 100;
        // zufriedenheit wird durch Kapelle gesteigert
        if(m_mouseManager.getResourcesCounter()[2] >= 1)
        {
            satisfactionKapelle = 100;
        }
        else
        {
            satisfactionKapelle = 0;
        }
        //zufriedenheit wird durch Schule oder Schmiede gesteigert
        if (m_mouseManager.getResourcesCounter()[7] >= 1 || m_mouseManager.getResourcesCounter()[8] >= 1)
        {
            satisfactionSchuleSchmiede = 100;
        }
        else
        {
            satisfactionSchuleSchmiede = 0;
        }
        //Zufriedenheit wird durch Kaserne oder Universität gesteigert
        if (m_mouseManager.getResourcesCounter()[9] >= 1 || m_mouseManager.getResourcesCounter()[10] >= 1)
        {
            satisfactionKaserneUni = 100;
        }
        else
        {
            satisfactionKaserneUni = 0;
        }
        // Berechnung wenn uni baubar 4:1:1:1
        if(m_mouseManager.getResourcesCounter()[7] >= 1 || m_mouseManager.getResourcesCounter()[8] >= 1){
            satisfaction = Mathf.Round(((4 * satisfactionFood) + satisfactionKapelle + satisfactionSchuleSchmiede + satisfactionKaserneUni) / 7);
        }
        else{
            //Berechnung wenn Schule baubar Food 3:1:1
            if(m_mouseManager.getResourcesCounter()[6] >= 1)
            {
                satisfaction = Mathf.Round(((3 * satisfactionFood) + satisfactionKapelle + satisfactionSchuleSchmiede) / 5);
            }
            else
            {
                //berechnung sonst - Food Kapelle 2:1
                satisfaction = Mathf.Round(((2 * satisfactionFood) + satisfactionKapelle) / 3);
            }
        }
        return satisfaction;
    }

    void updateCounters()
    {
        int[] resourcesCounter = m_mouseManager.getResourcesCounter();
        if (getSatisfaction() < 50)
        {
            woodCount += 7 * resourcesCounter[0]; //10
            ironCount += 3 * resourcesCounter[6]; //2
            stoneCount += 5 * resourcesCounter[4]; //5
        }
        else
        {
            if(getSatisfaction() < 25)
            {
                woodCount += 5 * resourcesCounter[0]; //10
                ironCount += 2 * resourcesCounter[6]; //2
                stoneCount += 4 * resourcesCounter[4]; //5
            }
            else
            {
                woodCount += 10 * resourcesCounter[0]; //10
                ironCount += 4 * resourcesCounter[6]; //2
                stoneCount += 7 * resourcesCounter[4]; //5
            }
        }
        foodCount += 50 * resourcesCounter[3];
        foodCount += 60 * resourcesCounter[5];
        if(foodCount> 0)
        {

            foodCount -= 2 * einwohnerCount;
        }
        else
        {
            foodCount = 0;
        }

    }

    //Gibt weiter ob genügend Rohstoffe für ein Gebäude vorhanden sind
    public bool checkBuildingCosts(string buildingName)
    {
        int einwohnerFree = einwohnerGesamt - einwohnerCount;
        switch (buildingName)
        {
            case ("Woodcutter"):
                if(woodCount >= 200 && einwohnerFree >= 4)
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
                float satisfaction = getSatisfaction();
                if (woodCount >= 3000 && stoneCount >= 2000 && ironCount >= 2000 && goldCount >= 2000 && einwohnerFree >= 40 && satisfaction >= 75)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }


    //zieht die entsprechenden Rohstoffe ab, wenn ein Gebäude gebaut wurde
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
                goldCount -= 2000;
                einwohnerCount += 40;
                break;
        }
    }


    //wird aufgerufen wenn ein Gebäude zerstört wurde
    //gibt Teil der Rohstoffe zurück
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
