using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : Photon.MonoBehaviour {

    public GameObject forrest;
    public GameObject haupthaus;
    public GameObject holzfäller;
    public GameObject steinmine;
    public GameObject eisenmine;
    public GameObject wohnhaus;
    public GameObject kapelle;
    public GameObject brauerei;
    public GameObject bäcker;
    public GameObject schmiede;
    public GameObject kaserne;
    public GameObject schule;
    public GameObject universität;
    public GameObject wahrzeichen;
    private GameObject selectedHouse;

    public GameObject Augsburg;

    public Building_Menu m_building_menu;
    public Resources_Counter m_resources_counter;
    public PopUpManager m_popup_manager;

    private string house_name;
    //holzfäller, wohnhaus, kapelle, bäcker,stonefeeeder, brauerei, eisenmine, schule, scmiede, uni, kaserne, wahrzeichen
    private int[] resources_counter = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
            //addObjects(pos);
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
        //holzfäller, wohnhaus, kapelle, bäcker,stonefeeeder, brauerei, eisenmine, schule, schmiede, uni, kaserne, wahrzeichen

        switch (m)                                                      
        {
            case (1): selectedHouse = holzfäller;
                house_name = "Woodcutter";
                counter_position = 0;
                // add needed people 
                break;
            case (2):
                selectedHouse = steinmine;
                house_name = "Stonefeeder";
                counter_position = 4;
                // add needed people 
                break;
            case (3): 
                selectedHouse = eisenmine;
                house_name = "Ironfeeder";
                counter_position = 6;
                // add needed people 
                break;
            case (4):
                selectedHouse = wohnhaus;
                house_name = "LivingHouse";
                counter_position = 1;
                // add people space
                break;
            case (5):
                selectedHouse = kapelle;
                house_name = "Church";
                counter_position = 2;
                // add needed people 
                break;
            case (6):
                selectedHouse = brauerei;
                house_name = "Brauerei";
                counter_position = 5;
                // add needed people 
                break;
            case (7):
                selectedHouse = bäcker;
                house_name = "Bäcker";
                counter_position = 3;
                // add needed people 
                break;
            case (8):
                selectedHouse = wahrzeichen;
                house_name = "Wahrzeichen";
                counter_position = 11;
                // add needed people 
                break;
            case (9):
                selectedHouse = schmiede;
                house_name = "Schmiede";
                counter_position = 8;
                // add needed people 
                break;
            case (10):
                selectedHouse = kaserne;
                house_name = "Kaserne";
                counter_position = 10;
                // add needed people 
                break;
            case (11):
                selectedHouse = schule;
                house_name = "Schule";
                counter_position = 7;
                // add needed people 
                break;
            case (12):
                selectedHouse = universität;
                house_name = "Universität";
                counter_position = 9;
                // add needed people 
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


        string playerName = PhotonNetwork.playerName;
        if (photonView.isMine)
        {
            m_popup_manager.OnGameStartInfo(getCityName(playerName));
        }
        
    }


    // gives back the Name of the City by the playerName
    private string getCityName(string playerName)
    {
        switch (playerName)
        {
            case ("Schwaben"): return "Augsburg";
            case ("Unterfranken"): return "Würzburg";
            case ("Oberpfalz"): return "Regensburg";
            case ("Oberbayern"): return "München";
            case ("Niederbayern"): return "Landshut";
            case ("Oberfranken"): return "Bayreuth";
            case ("Mittelfranken"): return "Ansbach";
            default: return "";

        }
    }
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            addObjects(Input.mousePosition);
        }
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
                    m_popup_manager.onClosePopUp();
                    GameObject parentHitObject = ourHitObject.transform.parent.gameObject;

                        //Wird ausgeführt falls ein Haus ausgewählt wurde zum bauen
                        if (selectedHouse != null)
                        {
                            //Falls auf ein Hexagon geklickt, dort noch nichts gebaut ist und das Entferntool nicht geklickt wurde, kann ein Haus gebaut werden
                            if (ourHitObject.transform.tag == PhotonNetwork.playerName && parentHitObject.transform.childCount <= 1 && !m_building_menu.getDestroyBool() && m_resources_counter.checkBuildingCosts(house_name))
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
                    else
                    {
                        //Überprüft ob Entferntool aktiviert wurde
                        if (m_building_menu.getDestroyBool())
                        {

                            destroyBuilding(ourHitObject, parentHitObject);

                        }

                        if (!m_building_menu.getDestroyBool())
                        {
                            presentBuildingInfo(ourHitObject);
                            m_building_menu.deactivateBuildMode();
                        }
                    }

                        
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_building_menu.deactivateBuildMode();
                    m_popup_manager.onClosePopUp();
                }
            }
        }
    }


    //gives a pop up wenn clicked on a building
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
                m_popup_manager.onFirstTimeBuild("Stonefeeder");
                Debug.Log("Stonefeeder clicked");
                break;
            case ("LivingHouse"):
                m_popup_manager.onFirstTimeBuild("LivingHouse");
                Debug.Log("LivingHouse clicked");
                break;
            case ("Church"):
                m_popup_manager.onFirstTimeBuild("Church");
                Debug.Log("Church clicked");
                break;
            case ("Brauerei"):
                m_popup_manager.onFirstTimeBuild("Brauerei");
                Debug.Log("Brauerei clicked");
                break;
            case ("Bäcker"):
                m_popup_manager.onFirstTimeBuild("Bäcker");
                Debug.Log("Bäcker clicked");
                break;
            case ("Schmiede"):
                m_popup_manager.onFirstTimeBuild("Schmiede");
                Debug.Log("Schmiede clicked");
                break;
            case ("Kaserne"):
                m_popup_manager.onFirstTimeBuild("Kaserne");
                Debug.Log("Kaserne clicked");
                break;
            case ("Schule"):
                m_popup_manager.onFirstTimeBuild("Schule");
                Debug.Log("Schule clicked");
                break;
            case ("Universität"):
                m_popup_manager.onFirstTimeBuild("Universität");
                Debug.Log("Universität clicked");
                break;
            case ("Wahrzeichen"):
                m_popup_manager.onFirstTimeBuild("Wahrzeichen");
                Debug.Log("Wahrzeichen clicked");
                break;
            case ("Dorfzentrum"):
                m_popup_manager.onFirstTimeBuild("Dorfzentrum");
                Debug.Log("Dorfzentrum clicked");
                break;
            case ("Bamberg"):
                checkPopUp("Bamberg");
                Debug.Log("Bamberg clicked");
                break;
            case ("Nürnberg"):
                checkPopUp("Nürnberg");
                Debug.Log("Nürnberg clicked");
                break;
            case ("Aschaffenburg"):
                checkPopUp("Aschaffenburg");
                Debug.Log("Aschaffenburg clicked");
                break;
            case ("Weiden"):
                checkPopUp("Weiden");
                Debug.Log("Weiden clicked");
                break;
            case ("Ingolstadt"):
                checkPopUp("Ingolstadt");
                Debug.Log("Ingolstadt clicked");
                break;
            case ("Kempten"):
                checkPopUp("Kempten");
                Debug.Log("Kempten clicked");
                break;
            case ("Passau"):
                checkPopUp("Passau");
                Debug.Log("Passau clicked");
                break;
            default:
           
                break;
        }
    }

    private void checkPopUp(string tag)
    {
        resources_counter = m_resources_counter.getResourcesCounter();
        //wenn Uni gebaut
        if (resources_counter[9] > 0)
        {
            m_popup_manager.dorfInfoUni(tag);
        }
        else
        {
            //wenn Kaserne gebaut
            if (resources_counter[10] > 0)
            {
                m_popup_manager.dorfInfoKaserne(tag);
            }
            else
            {
                m_popup_manager.onFirstTimeBuild(tag);
            }
        }
    }

    //bauen des Hauses
    void buildHouse(GameObject parentHitObj)
    {

        //Haus bauen im Netzwerk
        GameObject house_go = (GameObject) PhotonNetwork.Instantiate(selectedHouse.name, parentHitObj.transform.position, Quaternion.identity, 0);

        //Erhöht Anzahl der bestimmen Hausart
        
        resources_counter[counter_position] += 1;

        //Setzt Namen des Hauses
        house_go.name = house_name;
        if (house_name.Equals("LivingHouse"))
        {

            house_go.transform.GetChild(3).tag = house_name;
            house_go.transform.GetChild(4).tag = house_name;
        }
        else
        {

            house_go.transform.GetChild(0).tag = house_name;
        }
        

        //Fügt dem Ortsobjekt das Haus als Child hinzu
        house_go.transform.parent = parentHitObj.transform;

        MeshRenderer mr = parentHitObj.GetComponentInChildren<MeshRenderer>();
        mr.material.color = Color.red;

    }


   

    private Color getColorBezirk(string tag)
    {
        switch (tag)
        {
            case ("Unterfranken"): return Color.white;
            case ("Oberfranken"): return Color.magenta;
            case ("Mittelfranken"): return Color.black;
            case ("Oberpfalz"): return Color.blue;
            case ("Oberbayern"): return Color.green;
            case ("Niederbayern"): return Color.cyan;
            case ("Schwaben"): return Color.yellow;
            default: return Color.white;
        }
    }

    //Sucht den Tag der geklickten Objekte und zerstört diese
    void destroyBuilding(GameObject hitObject, GameObject parentObject)
    {

        if (photonView.isMine)
        {
            switch (hitObject.tag)
            {
                //holzfäller, wohnhaus, kapelle, bäcker,stonefeeeder, brauerei, eisenmine, schule, scmiede, uni, kaserne, wahrzeichen

                case ("Woodcutter"):
                    m_resources_counter.destroyedBuilding("Woodcutter");
                    resources_counter[0] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    Debug.Log(parentObject.transform.GetChild(0).tag);
                    MeshRenderer mr = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Ironfeeder"):
                    m_resources_counter.destroyedBuilding("Ironfeeder");
                    resources_counter[6] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr1 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr1.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Stonefeeder"):
                    m_resources_counter.destroyedBuilding("Stonefeeder");
                    resources_counter[4] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr2 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr2.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("LivingHouse"):
                    m_resources_counter.destroyedBuilding("LivingHouse");
                    resources_counter[1] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr3 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr3.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Church"):
                    m_resources_counter.destroyedBuilding("Church");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[2] -= 1;
                    MeshRenderer mr4 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr4.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Brauerei"):
                    m_resources_counter.destroyedBuilding("Brauerei");
                    resources_counter[5] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr5 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr5.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Bäcker"):
                    m_resources_counter.destroyedBuilding("Bäcker");
                    resources_counter[3] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr6 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr6.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;

                case ("Schmiede"):
                    m_resources_counter.destroyedBuilding("Schmiede");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[8] -= 1;
                    MeshRenderer mr7 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr7.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Kaserne"):
                    m_resources_counter.destroyedBuilding("Kaserne");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[10] -= 1;
                    MeshRenderer mr8 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr8.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Schule"):
                    m_resources_counter.destroyedBuilding("Schule");
                    resources_counter[7] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr9 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr9.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                case ("Universität"):
                    m_resources_counter.destroyedBuilding("Universität");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[9] -= 1;
                    MeshRenderer mr10 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr10.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject);
                    break;
                default:
                    m_building_menu.activateDestroy();
                    break;
            }
        }
        

	}
}
