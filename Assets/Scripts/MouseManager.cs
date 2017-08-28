using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject burg;
    public GameObject burg2;
    public GameObject burg3;
    public GameObject burg4;
    public GameObject dom;
    public GameObject dom2;
    public GameObject dom3;
    public GameObject dom4;
    private GameObject selectedHouse;

    //wird burg zugewiesen wenn zuvor Kaserne gebaut, wird dom zugewiesen wenn schule gebaut
    private GameObject wahrzeichen;
    private GameObject wahrzeichen2;
    private GameObject wahrzeichen3;
    private GameObject wahrzeichen4;

    // public GameObject aufgabenManager;

    private bool wahrzeichenBurg;

    public GameObject player;

    public Building_Menu m_building_menu;
    public Resources_Counter m_resources_counter;
    public PopUpManager m_popup_manager;

    private GameObject aufgabenManager;

    private string house_name;
    //holzfäller, wohnhaus, kapelle, bäcker,stonefeeeder, brauerei, eisenmine, schule, schmiede, uni, kaserne, wahrzeichen
    private int[] resources_counter = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int counter_position;
    private bool wahrzeichenBuildable = false;
    private float timer = 0;
    private GameObject wahrzeichenHitObject;




    //Empfängt und sendet die Daten für die gebauten Häuser

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Serializing");
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector3 pos = (Vector3)stream.ReceiveNext();
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

        m_building_menu = GameObject.FindGameObjectWithTag("BuildingMenuManager").GetComponent<Building_Menu>();
        
        m_resources_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();
        
        m_popup_manager = GameObject.FindGameObjectWithTag("PopUpManager").GetComponent<PopUpManager>();

        aufgabenManager = GameObject.Find("AufgabeManager");

        string playerName = PhotonNetwork.playerName;
        if (photonView.isMine)
        {
            m_popup_manager.OnGameStartInfo(getCityName(playerName));
            Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
            aufgabe.text = "Um dein Dorf ausbauen zu können, benötigst du Rohstoffe. Baue einen Holzfäller, um dein Dorf mit Holz zu versorgen.";

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
            updateWahrzeichenBuilding();
            
        }
    }

    //wird ausgeführt wenn das Wahrzeichen baubar ist
    //zählt die Uhr auf 100 sec hoch
    //baut alle 30-40 sekunden die neue ausbaustufe des Wahrzeichens
    //bei der letzten Stufe zeigt es glückwunschtext als aufgabe
    void updateWahrzeichenBuilding()
    {
        if (wahrzeichenBuildable)
        {
            timer += Time.deltaTime;
            int percentage = (int)timer;
            Text aufgabeTitel = aufgabenManager.transform.GetChild(0).GetComponent<Text>();
            aufgabeTitel.text = "";
            Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
            aufgabe.text = "Wahrzeichen wird gebaut: "+ percentage + " %";
            aufgabenManager.SetActive(true);
            if (Mathf.Round(timer) == 30)
            {
                buildWahrzeichen(1, wahrzeichen2);
            }
            if (Mathf.Round(timer) == 60)
            {
                buildWahrzeichen(2, wahrzeichen3);
            }
            if (Mathf.Round(timer) == 99)
            {
                buildWahrzeichen(3, wahrzeichen4);
                wahrzeichenBuildable = false;
            }
        }
    }

    //methode wird in Update aufgerufen
    //prüft ob irgendwas geklickt wurde
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
                    //m_popup_manager.setDoItFalse();
                    GameObject parentHitObject = ourHitObject.transform.parent.gameObject;

                    //Wird ausgeführt falls ein Haus ausgewählt wurde zum bauen
                    if (selectedHouse != null)
                        {
                        //Falls auf ein Hexagon geklickt, dort noch nichts gebaut ist und das Entferntool nicht geklickt wurde, kann ein Haus gebaut werden
                        if (ourHitObject.transform.tag == PhotonNetwork.playerName && parentHitObject.transform.childCount <= 1 && !m_building_menu.getDestroyBool() && m_resources_counter.checkBuildingCosts(house_name))
                        {
                            //Überprüft, ob auf dem angeklickten Feld gebaut werden darf
                            if (checkIfHouseBuiltIsPossible(hitInfo, ourHitObject))
                            {
                                buildHouse(parentHitObject);
                                m_resources_counter.reduceMaterials(house_name);
                                m_building_menu.deactivateBuildMode();
                                if (resources_counter[counter_position] == 1)
                                {
                                    m_popup_manager.onFirstTimeBuild(house_name);
                                }
                            }
                            else
                            {
                                //Möglichkeit weiteres Pop-up einzubauen mit Hinweis dass dort noch nicht gebaut werden kann
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
                    m_popup_manager.setDoItFalse();
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
                break;
            case ("Ironfeeder"):
                m_popup_manager.onFirstTimeBuild("Ironfeeder");
                break;
            case ("Stonefeeder"):
                m_popup_manager.onFirstTimeBuild("Stonefeeder");
                break;
            case ("LivingHouse"):
                m_popup_manager.onFirstTimeBuild("LivingHouse");
                break;
            case ("Church"):
                m_popup_manager.onFirstTimeBuild("Church");
                break;
            case ("Brauerei"):
                m_popup_manager.onFirstTimeBuild("Brauerei");
                break;
            case ("Bäcker"):
                m_popup_manager.onFirstTimeBuild("Bäcker");
                break;
            case ("Schmiede"):
                m_popup_manager.onFirstTimeBuild("Schmiede");
                break;
            case ("Kaserne"):
                m_popup_manager.onFirstTimeBuild("Kaserne");
                break;
            case ("Schule"):
                m_popup_manager.onFirstTimeBuild("Schule");
                break;
            case ("Universität"):
                m_popup_manager.onFirstTimeBuild("Universität");
                break;
            case ("Wahrzeichen"):
                m_popup_manager.onFirstTimeBuild("Wahrzeichen");
                break;
            case ("Dorfzentrum"):
                m_popup_manager.onFirstTimeBuild("Dorfzentrum");
                break;
            case ("Bamberg"):
                checkPopUp("Bamberg");
                break;
            case ("Nürnberg"):
                checkPopUp("Nürnberg");
                break;
            case ("Aschaffenburg"):
                checkPopUp("Aschaffenburg");
                break;
            case ("Weiden"):
                checkPopUp("Weiden");
                break;
            case ("Ingolstadt"):
                checkPopUp("Ingolstadt");
                break;
            case ("Kempten"):
                checkPopUp("Kempten");
                break;
            case ("Passau"):
                checkPopUp("Passau");
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


    //Wahrzeichen soll schrittweise gebaut werden
    //dazu soll zuerst die Baustufe abgerissen werden
    //anschließend wird die neue Baustufe gebaut
    //wird die letzte Stufe gebaut, wird ein gewinntext als aufgabe angezeigt
    void buildWahrzeichen(int step, GameObject wahrzeichen)
    {

        //zuerst zerstören
        PhotonNetwork.Destroy(wahrzeichenHitObject.transform.GetChild(1).gameObject);
        //Haus bauen im Netzwerk
        GameObject wahrzeichen_go = (GameObject)PhotonNetwork.Instantiate(wahrzeichen.name, wahrzeichenHitObject.transform.position, Quaternion.identity, 0);
        
        wahrzeichen_go.name = "Wahrzeichen";

        switch (step)
        {
            case (1): wahrzeichen_go.transform.GetChild(0).GetChild(0).tag = wahrzeichen_go.name;
                if (wahrzeichenBurg)
                {
                    wahrzeichen_go.transform.GetChild(0).GetChild(1).tag = wahrzeichen_go.name;
                }
                break;
            case (2): wahrzeichen_go.transform.GetChild(0).GetChild(0).tag = wahrzeichen_go.name;
                if (wahrzeichenBurg)
                {
                    wahrzeichen_go.transform.GetChild(0).GetChild(1).tag = wahrzeichen_go.name;
                    wahrzeichen_go.transform.GetChild(0).GetChild(2).tag = wahrzeichen_go.name;
                }
                break;
            case (3):
                wahrzeichen_go.transform.GetChild(0).GetChild(0).tag = wahrzeichen_go.name;
                if (wahrzeichenBurg)
                {
                    wahrzeichen_go.transform.GetChild(0).GetChild(1).tag = wahrzeichen_go.name;
                    wahrzeichen_go.transform.GetChild(0).GetChild(2).tag = wahrzeichen_go.name;
                    wahrzeichen_go.transform.GetChild(0).GetChild(3).tag = wahrzeichen_go.name;
                }
                resources_counter[11] += 1;
                Text aufgabeTitel = aufgabenManager.transform.GetChild(0).GetComponent<Text>();
                aufgabeTitel.text = "";
                Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                aufgabe.text = "Glückwunsch, du hast das Wahrzeichen deiner Stadt gebaut und das Spiel somit gewonnen!";
                Debug.Log(aufgabe.text);
                aufgabenManager.SetActive(true);
                break;
            default: break;
        }

        //Fügt dem Ortsobjekt das Haus als Child hinzu
        wahrzeichen_go.transform.parent = wahrzeichenHitObject.transform;

    }

    //bauen des Hauses
    void buildHouse(GameObject parentHitObj)
    {

        //Haus bauen im Netzwerk
        GameObject house_go = (GameObject) PhotonNetwork.Instantiate(selectedHouse.name, parentHitObj.transform.position, Quaternion.identity, 0);
        if (house_name.Equals("Kaserne"))
        {
            Vector3 position = new Vector3(parentHitObj.transform.position.x, parentHitObj.transform.position.y, parentHitObj.transform.position.z+3);
            GameObject soldat = Instantiate(player, position, Quaternion.identity);
            soldat.tag = "Soldat";

        }
        //Erhöht Anzahl der bestimmen Hausart
        if(resources_counter[4] == 1)
        {
            aufgabenManager.SetActive(false);
        }


        resources_counter[counter_position] += 1;

        //Setzt Namen des Hauses
        house_go.name = house_name;
                
        setTagsAndAufgaben(house_name,house_go,parentHitObj);
        
        
        //Fügt dem Ortsobjekt das Haus als Child hinzu
        house_go.transform.parent = parentHitObj.transform;

    }

    //unterscheidet welches ausgebaut wurde
    //setzt neue aufgabe, oder blendet sie aus
    //weißt den richtigen Unterobjekten mit mesh collider den Hausnamen als Tag zu
    //legt fest, ob burg oder dom als wahrzeichen gebaut wird
    private void setTagsAndAufgaben(string house_name,GameObject house_go, GameObject parentHitObj)
    {
        switch (house_name)
        {
            case ("LivingHouse"):
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                house_go.transform.GetChild(0).GetChild(1).tag = house_name;
                if (resources_counter[1] == 1)
                {
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Die Rohstoffproduktion ist abhängig von der Zufriedenheit der Einwohner. Die Zufriedenheit setzt sich aus verschiedenen Faktoren wie Nahrung oder Glauben zusammen. Sorge für eine ausreichende Nahrungs- und Glaubensversorgung!";
                }
                break;
            case ("Schule"):
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                house_go.transform.GetChild(0).GetChild(1).tag = house_name;
                house_go.transform.GetChild(0).GetChild(2).tag = house_name;
                house_go.transform.GetChild(0).GetChild(3).tag = house_name;
                if (resources_counter[7] == 1 || resources_counter[8] == 1)
                {
                    aufgabenManager.SetActive(false);
                }
                break;
            case ("Schmiede"):

                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                if (resources_counter[8] == 1 || resources_counter[7] == 1)
                {
                    aufgabenManager.SetActive(false);
                }
                break;
            case ("Stonefeeder"):
                if (resources_counter[4] == 1)
                {
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Baue dein Dorf nun weiter aus. Neue Gebäude werden stets freigeschaltet, wenn du alle vorherigen gebaut hast.";
                }
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                house_go.transform.GetChild(0).GetChild(1).tag = house_name;
                house_go.transform.GetChild(0).GetChild(2).tag = house_name;
                house_go.transform.GetChild(0).GetChild(3).tag = house_name;
                house_go.transform.GetChild(0).GetChild(4).tag = house_name;
                house_go.transform.GetChild(0).GetChild(5).tag = house_name;
                house_go.transform.GetChild(0).GetChild(6).tag = house_name;
                house_go.transform.GetChild(0).GetChild(7).tag = house_name;
                house_go.transform.GetChild(0).GetChild(8).tag = house_name;
                house_go.transform.GetChild(0).GetChild(9).tag = house_name;
                house_go.transform.GetChild(0).GetChild(10).tag = house_name;

                break;
            case ("Ironfeeder"):
                if (resources_counter[6] == 1)
                {
                    aufgabenManager.SetActive(true);
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Deine Bevölkerung sehnt sich nach Sicherheit(Schmiede) oder Wissen(Schule). Wähle einen der beiden Wege aus!";
                }
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                house_go.transform.GetChild(0).GetChild(1).tag = house_name;
                break;
            case ("Brauerei"):
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                house_go.transform.GetChild(0).GetChild(1).tag = house_name;
                house_go.transform.GetChild(0).GetChild(2).tag = house_name;
                aufgabenManager.SetActive(false);
                break;
            case ("Woodcutter"):
                if (resources_counter[0] == 1)
                {
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Durch den Bau des Holzfällers hast du neue Einwohner hinzubekommen. Um weitere Gebäude bauen zu können, musst du Platz für neue Einwohner schaffen.";
                }
                house_go.transform.GetChild(0).tag = house_name;

                break;
            case ("Bäcker"):
                if (resources_counter[3] == 1 && resources_counter[2] == 1)
                {
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Glückwunsch, du hast neue Gebäude freigeschaltet. Um dein Dorf weiter voranzubringen, solltest du Steine abbauen.";
                }
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                break;
            case ("Church"):
                if (resources_counter[2] == 1 && resources_counter[3] == 1)
                {
                    Text aufgabe = aufgabenManager.transform.GetChild(1).GetComponent<Text>();
                    aufgabe.text = "Glückwunsch, du hast neue Gebäude freigeschaltet. Um dein Dorf weiter voranzubringen, solltest du Steine abbauen.";
                }
                house_go.transform.GetChild(0).tag = house_name;
                break;
            case ("Kaserne"):
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                wahrzeichenBurg = true;
                wahrzeichen = burg;
                wahrzeichen2 = burg2;
                wahrzeichen3 = burg3;
                wahrzeichen4 = burg4;
                break;
            case ("Universität"):
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                wahrzeichenBurg = false;
                wahrzeichen = dom;
                wahrzeichen2 = dom2;
                wahrzeichen3 = dom3;
                wahrzeichen4 = dom4;
                break;
            case ("Wahrzeichen"):
                wahrzeichenHitObject = parentHitObj;
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                wahrzeichenBuildable = true;
                break;
            default:
                house_go.transform.GetChild(0).GetChild(0).tag = house_name;
                break;

        }
    }

    //Methode zum Überprüfen, ob das Feld bebaut werden darf/kann
    private Boolean checkIfHouseBuiltIsPossible(RaycastHit hitInfo, GameObject ourHitObject)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitInfo.point, 5.0f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform.parent != null)
            {
                if (hitColliders[i].transform.parent.childCount == 2)
                {
                    return true;
                }
            }
            else
            {
                switch (hitColliders[i].tag)
                {
                    /*case ("LivingHouse"):
                        return true;
                    case ("Schule"):
                        return true;
                    case ("Schmiede"):
                        return true;
                    case ("Stonefeeder"):
                        return true;
                    case ("Ironfeeder"):
                        return true;
                    case ("Brauerei"):
                        return true;
                    case ("Bäcker"):
                        return true;
                    case ("Woodcuter"):
                        return true;
                    case ("Church"):
                        return true;
                    case ("Wahrzeichen"):
                        return true;
                    case ("Dorfzentrum"):
                        return true;*/
                    case ("forrest"):
                        return true;
                    case ("hopfen"):
                        return true;

                }
            }

        }
        return false;
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
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Stonefeeder"):
                    m_resources_counter.destroyedBuilding("Stonefeeder");
                    resources_counter[4] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr2 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr2.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("LivingHouse"):
                    m_resources_counter.destroyedBuilding("LivingHouse");
                    resources_counter[1] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr3 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr3.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Church"):
                    m_resources_counter.destroyedBuilding("Church");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[2] -= 1;
                    MeshRenderer mr4 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr4.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Brauerei"):
                    m_resources_counter.destroyedBuilding("Brauerei");
                    resources_counter[5] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr5 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr5.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Bäcker"):
                    m_resources_counter.destroyedBuilding("Bäcker");
                    resources_counter[3] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr6 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr6.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;

                case ("Schmiede"):
                    m_resources_counter.destroyedBuilding("Schmiede");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[8] -= 1;
                    MeshRenderer mr7 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr7.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Kaserne"):
                    m_resources_counter.destroyedBuilding("Kaserne");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[10] -= 1;
                    MeshRenderer mr8 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr8.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Schule"):
                    m_resources_counter.destroyedBuilding("Schule");
                    resources_counter[7] -= 1;
                    m_building_menu.fakeAnimationDestroy();
                    MeshRenderer mr9 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr9.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                case ("Universität"):
                    m_resources_counter.destroyedBuilding("Universität");
                    m_building_menu.fakeAnimationDestroy();
                    resources_counter[9] -= 1;
                    MeshRenderer mr10 = parentObject.transform.parent.gameObject.GetComponentInChildren<MeshRenderer>();
                    mr10.material.color = getColorBezirk(PhotonNetwork.playerName);
                    PhotonNetwork.Destroy(parentObject.transform.parent.gameObject);
                    break;
                default:
                    m_building_menu.activateDestroy();
                    break;
            }
        }
        

	}

}
