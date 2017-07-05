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
    public Button sauerkrauterie;
    public Button wahrzeichen;

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


    private bool isShowing;
    private bool destroyIsActivated;
    private int building_Number;
    

	// Use this for initialization
	void Start () {
        //Setzt Auswahlmenu und zweite und dritte Seite auf false
        buildingCanvas.SetActive(false);

        //Setzt Listener für die Gebäude der ersten Auswahlseite
        woodcutter.onClick.AddListener(() => { onBuildingSelected(1); });
        stonefeeder.onClick.AddListener(() => { onBuildingSelected(2); });
        ironfeeder.onClick.AddListener(() => { onBuildingSelected(3); });
        livingHouse.onClick.AddListener(() => { onBuildingSelected(4); });
        church.onClick.AddListener(() => { onBuildingSelected(5); });
        brauerei.onClick.AddListener(() => { onBuildingSelected(6); });
        sauerkrauterie.onClick.AddListener(() => { onBuildingSelected(7); });
        wahrzeichen.onClick.AddListener(() => { onBuildingSelected(8); });


        m_popup_manager = GameObject.FindGameObjectWithTag("PopUpManager").GetComponent<PopUpManager>();

        m_resource_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();

    }

    //Aktion die ausgeführt wird wenn der Plusbutton für das Baumenu geklickt wird
    public void onClickedPlusButton()
    {
        deactivateBuildMode();
        isShowing = !isShowing;
        buildingCanvas.SetActive(isShowing);
        building_Number = 0;
        Debug.Log(transform);

        m_popup_manager.onClosePopUp();

    }

    public void deactivateBuildMode()
    {
        building_Number = 0;
        changeCursor(null);
    }

    //Listener für die geklickten Gebäude mit Index übergabe
    public void onBuildingSelected(int count)
    {
        building_Number = count;
        m_popup_manager.onClosePopUp();
        destroyIsActivated = false;
        switch (building_Number)
        {
            case (1):
                if(m_resource_counter.checkBuildingCosts("Woodcutter")){
                    changeCursor(cursorTextureHolzfäller);
                }
                break;
            case (2):
                if(m_resource_counter.checkBuildingCosts("Stonefeeder")){
                    changeCursor(cursorTextureSteinmine);
                }
                break;
            case (3):
                if(m_resource_counter.checkBuildingCosts("Ironfeeder")){
                    changeCursor(cursorTextureEisenmine);
                }
                break;
            case (4):
                if (m_resource_counter.checkBuildingCosts("LivingHouse"))
                {
                    changeCursor(cursorTextureWohnhaus);
                }
                break;
            case (5):
                if (m_resource_counter.checkBuildingCosts("Church"))
                {
                    changeCursor(cursorTextureKapelle);
                }
                break;
            case (6):
                if (m_resource_counter.checkBuildingCosts("Brauerei"))
                {
                    changeCursor(cursorTextureBrauerei);
                }
                break;
            case (7):
                if (m_resource_counter.checkBuildingCosts("Sauerkrauterie"))
                {
                    changeCursor(cursorTextureSauerkrauterie);
                }
                break;
            case (8):
                if (m_resource_counter.checkBuildingCosts("Wahrzeichen"))
                {
                    changeCursor(cursorTextureWahrzeichen);
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
        changeCursorToHammer();
        if (!destroyIsActivated)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

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

    //Gibt den zustand des Destroy-Button weiter
    public bool getDestroyBool()
    {
        return destroyIsActivated;
    }
}
