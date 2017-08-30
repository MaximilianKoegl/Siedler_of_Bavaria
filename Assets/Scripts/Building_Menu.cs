using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_Menu : MonoBehaviour {

    public GameObject buildingCanvas;
    public GameObject first_buildings_list;

    public Button woodcutter;
    public Button stonefeeder;
    public Button ironfeeder;
    public Button livingHouse;
    public Button church;
    public Button brauerei;
    public Button bäcker;
    public Button wahrzeichen;
    public Button schmiede;
    public Button kaserne;
    public Button schule;
    public Button universität;
    public Button delete;
    public Button startPositionBuildList;

    
    public GameObject kostenstonefeeder;
    public GameObject kostenironfeeder;
    public GameObject kostenbrauerei;
    public GameObject kostenwahrzeichen;
    public GameObject kostenschmiede;
    public GameObject kostenkaserne;
    public GameObject kostenschule;
    public GameObject kostenuniversität;
    public Sprite imagestonefeeder;
    public Sprite imageironfeeder;
    public Sprite imagebrauerei;
    public Sprite imageBurg;
    public Sprite imageDom;
    public Sprite imageschmiede;
    public Sprite imagekaserne;
    public Sprite imageschule;
    public Sprite imageuniversität;
    public Sprite imageNotBuildable;


    public Texture2D cursorTexture;
    public Texture2D cursorMoveTexture;
    public Texture2D cursorTextureHolzfäller;
    public Texture2D cursorTextureSteinmine;
    public Texture2D cursorTextureEisenmine;
    public Texture2D cursorTextureWohnhaus;
    public Texture2D cursorTextureKapelle;
    public Texture2D cursorTextureBrauerei;
    public Texture2D cursorTextureSauerkrauterie;
    public Texture2D cursorTextureWahrzeichen;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public PopUpManager m_popup_manager;
    
    public Resources_Counter m_resource_counter;

    //stonefeeeder, brauerei, eisenmine, schule, schmiede, uni, kaserne, wahrzeichen 
    private bool[] buildable = new bool[] { false, false, false, false, false, false, false, false};


    private bool isShowing;
    private bool destroyIsActivated;
    private int building_Number;

    private Vector3 startPositionOfBuildingGrid;
    
	void Start () {
        //Setzt Auswahlmenu und zweite und dritte Seite auf false
        buildingCanvas.SetActive(false);
        kostenstonefeeder.SetActive(false);
        kostenironfeeder.SetActive(false);
        kostenbrauerei.SetActive(false);
        kostenkaserne.SetActive(false);
        kostenschule.SetActive(false);
        kostenschmiede.SetActive(false);
        kostenuniversität.SetActive(false);
        kostenwahrzeichen.SetActive(false);

        //Setzt den Button für die Startposition des Buildingsmenü auf false
        startPositionBuildList.gameObject.SetActive(false);

        //Setzt die StartPosition fest
        startPositionOfBuildingGrid = first_buildings_list.transform.Find("GridWithHouses").position;

        //Setzt Listener für die Gebäude der ersten Auswahlseite
        woodcutter.onClick.AddListener(() => { onBuildingSelected(1); });
        stonefeeder.onClick.AddListener(() => { if(buildable[0]) onBuildingSelected(2); });
        ironfeeder.onClick.AddListener(() => { if (buildable[2]) onBuildingSelected(3); });
        livingHouse.onClick.AddListener(() => { onBuildingSelected(4); });
        church.onClick.AddListener(() => { onBuildingSelected(5); });
        brauerei.onClick.AddListener(() => { if (buildable[1])  onBuildingSelected(6); });
        bäcker.onClick.AddListener(() => { onBuildingSelected(7); });


        schmiede.onClick.AddListener(() => { if (buildable[4]) onBuildingSelected(9); });
        kaserne.onClick.AddListener(() => { if (buildable[6]) onBuildingSelected(10); });
        schule.onClick.AddListener(() => { if (buildable[3]) onBuildingSelected(11); });
        universität.onClick.AddListener(() => { if (buildable[5]) onBuildingSelected(12); });
        wahrzeichen.onClick.AddListener(() => { if (buildable[7]) onBuildingSelected(8); });


        m_popup_manager = GameObject.FindGameObjectWithTag("PopUpManager").GetComponent<PopUpManager>();

        m_resource_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();

    }

    public void Update()
    {
        checkBuildable();
        if (buildingCanvas.active)
        {
            checkBuildListPosition();
        }
    }


    //überprüft ob ein Gebäude baubar ist
    private void checkBuildable()
    {
        int[] resourcesCounter = m_resource_counter.getResourcesCounter();
        checkSecondLevel(resourcesCounter);
        checkThirdLevel(resourcesCounter);
        checkFourthLevel(resourcesCounter);
        checkFifthLevel(resourcesCounter);
        checkSixthLevel(resourcesCounter);

    }

    //Überprüft die aktuelle Position der Liste des Baumenüs
    private void checkBuildListPosition()
    {
        if(first_buildings_list.transform.Find("GridWithHouses").position != startPositionOfBuildingGrid)
        {
            startPositionBuildList.gameObject.SetActive(true);
        }
    }

    //Setzt die Liste des Baumenüs auf den Startwert zurück
    public void onStartPositionBuildListButtonClicked()
    {
        first_buildings_list.transform.Find("GridWithHouses").position = startPositionOfBuildingGrid;
        startPositionBuildList.gameObject.SetActive(false);
    }


    //Überprüft ob Steinmine und Brauerei baubar sind
    //wenn ja --> Anzeige in Baumenü wird verändert
    //wenn nein --> Anzeige in Baumenü ist ?
    private void checkSecondLevel(int[] resourcesCounter)
    {
        if (resourcesCounter[0] > 0 && resourcesCounter[1] > 0 && resourcesCounter[2] > 0 && resourcesCounter[3] > 0)
        {
            kostenstonefeeder.SetActive(true);
            buildable[0] = true;
            stonefeeder.GetComponentsInChildren<Image>()[1].sprite = imagestonefeeder;
            stonefeeder.GetComponentInChildren<Text>().text = "Steinmine";
            kostenbrauerei.SetActive(true);
            buildable[1] = true;
            brauerei.GetComponentsInChildren<Image>()[1].sprite = imagebrauerei;
            brauerei.GetComponentInChildren<Text>().text = "Brauerei";

        }
        else
        {
            kostenstonefeeder.SetActive(false);
            buildable[0] = false;
            stonefeeder.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            stonefeeder.GetComponentInChildren<Text>().text = "";
            kostenbrauerei.SetActive(false);
            buildable[1] = false;
            brauerei.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            brauerei.GetComponentInChildren<Text>().text = "";
        }
    }


    //Überprüft ob Eisenmine baubar ist
    //wenn ja --> Anzeige in Baumenü wird verändert
    //wenn nein --> Anzeige in Baumenü ist ?
    private void checkThirdLevel(int[] resourcesCounter)
    {
        if (resourcesCounter[4] > 0 && resourcesCounter[5] > 0)
        {
            kostenironfeeder.SetActive(true);
            buildable[2] = true;
            ironfeeder.GetComponentsInChildren<Image>()[1].sprite = imageironfeeder;
            ironfeeder.GetComponentInChildren<Text>().text = "Eisenmine";
        }
        else
        {
            kostenironfeeder.SetActive(false);
            buildable[2] = false;
            ironfeeder.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            ironfeeder.GetComponentInChildren<Text>().text = "";
        }
    }


    //Überprüft ob Schule und Schmiede baubar sind
    //wenn ja --> Anzeige in Baumenü wird verändert
    //wenn nein --> Anzeige in Baumenü ist ?
    private void checkFourthLevel(int[] resourcesCounter)
    {
        if (resourcesCounter[6] > 0)
        {
            kostenschule.SetActive(true);
            buildable[3] = true;
            schule.GetComponentsInChildren<Image>()[1].sprite = imageschule;
            schule.GetComponentInChildren<Text>().text = "Schule";
            kostenschmiede.SetActive(true);
            buildable[4] = true;
            schmiede.GetComponentsInChildren<Image>()[1].sprite = imageschmiede;
            schmiede.GetComponentInChildren<Text>().text = "Schmiede";
        }
        else
        {
            kostenschule.SetActive(false);
            buildable[3] = false;
            schule.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            schule.GetComponentInChildren<Text>().text = "";
            kostenschmiede.SetActive(false);
            buildable[4] = false;
            schmiede.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            schmiede.GetComponentInChildren<Text>().text = "";
        }
    }


    //Überprüft ob Uni und Kaserne baubar sind
    //wenn ja --> Anzeige in Baumenü wird verändert
    //wenn nein --> Anzeige in Baumenü ist ?
    private void checkFifthLevel(int[] resourcesCounter)
    {
        if (resourcesCounter[7] > 0)
        {
            kostenuniversität.SetActive(true);
            buildable[5] = true;
            universität.GetComponentsInChildren<Image>()[1].sprite = imageuniversität;
            universität.GetComponentInChildren<Text>().text = "Universität";
            kaserne.gameObject.SetActive(false);
            schmiede.gameObject.SetActive(false);
        }
        else
        {
            kostenuniversität.SetActive(false);
            buildable[5] = false;
            universität.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            universität.GetComponentInChildren<Text>().text = "";
            kaserne.gameObject.SetActive(true);
            schmiede.gameObject.SetActive(true);
            if (resourcesCounter[8] > 0)
            {
                kostenkaserne.SetActive(true);
                buildable[6] = true;
                kaserne.GetComponentsInChildren<Image>()[1].sprite = imagekaserne;
                kaserne.GetComponentInChildren<Text>().text = "Kaserne";
                universität.gameObject.SetActive(false);
                schule.gameObject.SetActive(false);
            }
            else
            {
                kostenkaserne.SetActive(false);
                buildable[6] = false;
                kaserne.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
                kaserne.GetComponentInChildren<Text>().text = "";
                universität.gameObject.SetActive(true);
                schule.gameObject.SetActive(true);
            }
        }
    }


    //Überprüft ob Wahrzeichen baubar ist
    //wenn ja --> Anzeige in Baumenü wird verändert
    //wenn nein --> Anzeige in Baumenü ist ?
    private void checkSixthLevel(int[] resourcesCounter)
    {
        if (resourcesCounter[9] > 0 || resourcesCounter[10] > 0)
        {
            kostenwahrzeichen.SetActive(true);
            buildable[7] = true;
            if(resourcesCounter[9] > 0)
            {

                wahrzeichen.GetComponentsInChildren<Image>()[1].sprite = imageDom;
            }
            else
            {

                wahrzeichen.GetComponentsInChildren<Image>()[1].sprite = imageBurg;
            }
            wahrzeichen.GetComponentInChildren<Text>().text = "Wahrzeichen";
        }
        else
        {
            kostenwahrzeichen.SetActive(false);
            buildable[7] = false;
            wahrzeichen.GetComponentsInChildren<Image>()[1].sprite = imageNotBuildable;
            wahrzeichen.GetComponentInChildren<Text>().text = "";
        }
    }



    //Aktion die ausgeführt wird wenn der Plusbutton für das Baumenu geklickt wird
    public void onClickedPlusButton()
    {
        startPositionBuildList.gameObject.SetActive(false);
        
            deactivateBuildMode();
            isShowing = !isShowing;
            buildingCanvas.SetActive(isShowing);
            building_Number = 0;

            m_popup_manager.onClosePopUp();
        

    }

    // Löscht die Auswahl des gebauten Elements
    public void deactivateBuildMode()
    {
        switch (building_Number)
        {
            case (1):
                woodcutter.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (2):
                stonefeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (3):
                ironfeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (4):
                livingHouse.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (5):
                church.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (6):
                brauerei.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (7):
                bäcker.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            case (8):
                wahrzeichen.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (9):
                schmiede.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (10):
                kaserne.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (11):
                schule.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (12):
                universität.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            default: changeCursor(null); break;
        }
        building_Number = 0;
        changeCursor(null);
    }


    //Ändert Cursor und Auswahlfarbe im Baumenü auf null
    public void deselectAll()
    {
        changeCursor(null);
        woodcutter.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        stonefeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        ironfeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        bäcker.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        brauerei.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        kaserne.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        schmiede.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        schule.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        universität.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        livingHouse.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        church.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        wahrzeichen.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
    }

    //Listener für die geklickten Gebäude mit Index Übergabe
    public void onBuildingSelected(int count)
    {
        
            building_Number = count;
            m_popup_manager.onClosePopUp();
            destroyIsActivated = false;
            delete.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            switch (building_Number)
            {
                case (1):
                    if(woodcutter.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        woodcutter.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Woodcutter"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureHolzfäller);
                            woodcutter.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                
                    break;
                case (2):
                    if (stonefeeder.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        stonefeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Stonefeeder"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureSteinmine);
                            stonefeeder.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (3):
                    if (ironfeeder.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        ironfeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Ironfeeder"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureEisenmine);
                            ironfeeder.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (4):
                    if (livingHouse.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        livingHouse.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("LivingHouse"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWohnhaus);
                            livingHouse.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (5):
                    if (church.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        church.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Church"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureKapelle);
                            church.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (6):
                    if (brauerei.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        brauerei.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Brauerei"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureBrauerei);
                            brauerei.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (7):
                    if (bäcker.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        bäcker.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Bäcker"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureSauerkrauterie);
                            bäcker.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (8):
                    if (wahrzeichen.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        wahrzeichen.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Wahrzeichen"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWahrzeichen);
                            wahrzeichen.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                        else
                        {
                            wahrzeichen.GetComponent<Image>().color = new Color32(80, 80, 0, 186);
                        }
                    }
                    break;
                case (9):
                    if (schmiede.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        schmiede.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Schmiede"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWahrzeichen);
                            schmiede.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (10):
                    if (kaserne.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        kaserne.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Kaserne"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWahrzeichen);

                            kaserne.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (11):
                    if (schule.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        schule.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Schule"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWahrzeichen);
                            schule.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }
                    break;
                case (12):
                    if (universität.GetComponent<Image>().color == new Color32(80, 0, 0, 186))
                    {
                        changeCursor(null);
                        universität.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                        building_Number = 0;
                    }
                    else
                    {
                        if (m_resource_counter.checkBuildingCosts("Universität"))
                        {
                            deselectAll();
                            changeCursor(cursorTextureWahrzeichen);
                            universität.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
                        }
                    }

                    break;
                default: changeCursor(null);break;
            }

        
    }

    //Gibt Gebäude Index weiter 
    public int getBuildingNumber()
    {
        int number = building_Number;
        return number;
    }

    //Aktion die ausgeführt wird wenn auf Destroy-Button geklickt wird
    public void activateDestroy()
    {
        
        destroyIsActivated = !destroyIsActivated;
        delete.GetComponent<Image>().color = new Color32(80, 0, 0, 186);
        switch (building_Number)
        {
            case (1):
                woodcutter.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (2):
                stonefeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (3):
                ironfeeder.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (4):
                livingHouse.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (5):
                church.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (6):
                brauerei.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (7):
                bäcker.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (8):
                wahrzeichen.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (9):
                schmiede.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (10):
                kaserne.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (11):
                schule.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
            case (12):
                universität.GetComponent<Image>().color = new Color32(0, 0, 0, 0);

                break;
            default: changeCursor(null); break;
        }
        changeCursorToHammer();
        if (!destroyIsActivated)
        {
            delete.GetComponent<Image>().color = new Color32(255,255,255,255);
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    //Nur bei der Windows Version nützlich / anwendbar
    public void fakeAnimationDestroy()
    {
        Cursor.SetCursor(cursorMoveTexture, hotSpot, cursorMode);
        Invoke("changeCursorToHammer", 0.22f);


    }

    void changeCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    }

    void changeCursorToHammer()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    }

    //Gibt den Zustand des Destroy-Button zurück
    public bool getDestroyBool()
    {
        return destroyIsActivated;
    }
}
