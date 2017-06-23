using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Photon.MonoBehaviour {

    public GameObject forrest;
    public GameObject house;
    public GameObject house_two;
    public GameObject house_three;
    private GameObject selectedHouse;

    public Building_Menu m_building_menu;
    public Resources_Counter m_resources_counter;
    public PopUpManager m_popup_manager;

    private string house_name;
    private int[] resources_counter = new int[] { 0, 0, 0, 0 };
    private int counter_position;

    //Empfängt und sendet die Daten für die gebauten Häuser

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Serializing");
        if (stream.isWriting)
        {
            Debug.Log("Writing " + Input.mousePosition);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector3 pos = (Vector3)stream.ReceiveNext();
            addObjects(pos);
            Debug.Log("Receiving Reading " + pos);
        }
    }

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


    // initialization of GameObjects
    void Start () {
        //get All Objects with Tag "BuildingMenuManager", first object is taken, component "Building_Menu" as building_menu

        m_building_menu = GameObject.FindGameObjectWithTag("BuildingMenuManager").GetComponent<Building_Menu>();
        
        m_resources_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();
        
        m_popup_manager = GameObject.FindGameObjectWithTag("PopUpManager").GetComponent<PopUpManager>();
    }
	
	// Update is called once per frame
	void Update () {
        addObjects(Input.mousePosition);
    }


    void addObjects(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hitInfo;

        //Überprüft ob auf die Bayernfläche geklickt wird
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            //Startet Raycast 
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject ourHitObject = hitInfo.collider.transform.gameObject;
                onHouseSwitch();
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject parentHitObject = ourHitObject.transform.parent.gameObject;

                    //Wird ausgeführt falls ein Haus ausgewählt wurde zum bauen
                    if (selectedHouse != null)
                    {
                        //Falls auf ein Hexagon geklickt, dort noch nichts gebaut ist und das Entferntool nicht geklickt wurde, kann ein Haus gebaut werden
                        if (ourHitObject.transform.name == "Hexagon" && parentHitObject.transform.childCount <= 1 && !m_building_menu.getDestroyBool() && m_resources_counter.checkBuildingCosts(house_name))
                        {

                            buildHouse(parentHitObject);
                            m_resources_counter.reduceMaterials(house_name);
                            m_building_menu.deactivateBuildMode();

                            if (resources_counter[counter_position] == 1)
                            {
                                m_popup_manager.onFirstTimeBuild(house_name);
                            }

                        }
                    }

                    //Überprüft ob Entferntool aktiviert wurde
                    if (m_building_menu.getDestroyBool())
                    {
                        destroyBuilding(ourHitObject, parentHitObject);

                    }

                    if (!m_building_menu.getDestroyBool())
                    {
                        presentBuildingInfo(ourHitObject);
                    }
                }
            }
        }
    }

    void presentBuildingInfo(GameObject hitObject)
    {
        switch (hitObject.tag)
        {
            case ("Woodcutter"):
                m_popup_manager.onFirstTimeBuild("Woodcutter");
                Debug.Log("Woodcutter clicked");

                break;
            case ("Ironfeeder"):
                m_popup_manager.onFirstTimeBuild("Ironfeeder");
                Debug.Log("Ironfeeder clicked");

                break;
            case ("Stonefeeder"):
                m_popup_manager.onFirstTimeBuild("Ironfeeder");
                Debug.Log("Stonefeeder clicked");

                break;
            default:
                break;
        }
    }

    void buildHouse(GameObject parentHitObj)
    {

        //Haus bauen im Netzwerk
        GameObject house_go = (GameObject) PhotonNetwork.Instantiate(selectedHouse.name, parentHitObj.transform.position, Quaternion.identity, 0);

        //Erhöht Anzahl der bestimmen Hausart
        resources_counter[counter_position] += 1;

        //Setzt Namen des Hauses
        house_go.name = house_name;
        house_go.transform.GetChild(0).tag = house_name;

        //Fügt dem Ortsobjekt das Haus als Child hinzu
        house_go.transform.parent = parentHitObj.transform;
    }

    //Sucht den Tag der geklickten Objekte und zerstört diese
    void destroyBuilding(GameObject hitObject, GameObject parentObject)
    {
        m_building_menu.fakeAnimationDestroy();
        switch (hitObject.tag)
        {
            case ("Woodcutter"):
                resources_counter[0] -= 1;
                PhotonNetwork.Destroy(parentObject);

                break;
            case ("Ironfeeder"):
                resources_counter[1] -= 1;
                PhotonNetwork.Destroy(parentObject);

                break;
            case ("Stonefeeder"):
                resources_counter[2] -= 1;
                PhotonNetwork.Destroy(parentObject);

                break;
            default:
                break;
        }

	}
}
