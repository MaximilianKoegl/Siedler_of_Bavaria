using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

    public GameObject forrest;
    public GameObject house;
    public GameObject house_two;
    public GameObject house_three;
    private GameObject selectedHouse;

    public Building_Menu m_building_menu;

    private string house_name;
    private int[] resources_counter = new int[] { 0, 0, 0, 0 };
    private int counter_position;

    //Gibt Anzahl der momentan gebauten Gebäude weiter, als Rohstoff Multiplikator
    public int[] getResourcesCounter()          
    {
        return resources_counter;
    }

    //Wählt Haus Index, aufgrund des ausgewählten Hauses im BuildingMenu aus, und übergibt es SelectedHouse 
    void onHouseSwitch()
    {
        int m = m_building_menu.getBuildingNumber();

        switch (m)                                                      
        {
            case (1): selectedHouse = house;
                house_name = "Woodcutter";
                counter_position = 0;

                break;
            case (2): selectedHouse = house_two;
                house_name = "Ironfeeder";
                counter_position = 1;
                break;
            case (3): selectedHouse = house_three;
                house_name = "Stonefeeder";
                counter_position = 2;
                break;
            default: selectedHouse = null;
                house_name = "";
                break;
                
        }
    }
    // Use this for initialization
    void Start () {
   
	}
	
	// Update is called once per frame
	void Update () {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        //Überprüft ob auf die Bayernfläche geklickt wird
        if (!EventSystem.current.IsPointerOverGameObject()) {   

            //Startet Raycast 
            if (Physics.Raycast(ray, out hitInfo))                                                                                             
            {
                GameObject ourHitObject = hitInfo.collider.transform.gameObject;

                onHouseSwitch();

                

                    if (Input.GetMouseButtonDown(0))
                    {
                        MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
                        GameObject parentHitObject = ourHitObject.transform.parent.gameObject;
                        
                    //Wird ausgeführt falls ein Haus ausgewählt wurde zum bauen
                    if (selectedHouse != null)
                    {
                        //Falls auf ein Hexagon geklickt, dort noch nichts gebaut ist und das Entferntool nicht geklickt wurde, kann ein Haus gebaut werden
                        if (ourHitObject.transform.name == "Hexagon" && parentHitObject.transform.childCount <= 1 && !m_building_menu.getDestroyBool())
                        {

                            GameObject house_go = (GameObject)Instantiate(selectedHouse, parentHitObject.transform.position, Quaternion.identity);

                            //Erhöht Anzahl der bestimmen Hausart
                            resources_counter[counter_position] += 1;

                            //Setzt Namen des Hauses
                            house_go.name = house_name;

                            //Fügt dem Ortsobjekt das Haus als Child hinzu
                            house_go.transform.parent = parentHitObject.transform;

                        }
                    }

                        //Überprüft ob auf Hexagon geklickt wurde, ob etwas auf dem Hexagon gebaut ist und ob das Entferntool aktiviert wurde
                        if(ourHitObject.transform.name == "Hexagon" && parentHitObject.transform.childCount >= 2 && m_building_menu.getDestroyBool())
                        {
                        GameObject clickedHouse = parentHitObject.transform.GetChild(1).gameObject;

                        Destroy(clickedHouse);
                        }
                    }
                
            }


        }

	}
}
