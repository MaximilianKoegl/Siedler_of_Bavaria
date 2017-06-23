using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_Menu : MonoBehaviour {

    public GameObject buildingCanvas;
    public GameObject first_buildings_list;
    public GameObject second_buildings_list;
    public GameObject third_buildings_list;

    public Button first_building;
    public Button second_building;
    public Button third_building;
    public Button fourth_building;
    public Button fifth_building;

    public Texture2D cursorTexture;
    public Texture2D cursorMoveTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private bool isShowing;
    private bool destroyIsActivated;
    private int building_Number;
    

	// Use this for initialization
	void Start () {
        //Setzt Auswahlmenu und zweite und dritte Seite auf false
        buildingCanvas.SetActive(false);
        second_buildings_list.SetActive(false);
        third_buildings_list.SetActive(false);

        //Setzt Listener für die Gebäude der ersten Auswahlseite
        first_building.onClick.AddListener(() => { onBuildingSelected(1); });
        second_building.onClick.AddListener(() => { onBuildingSelected(2); });
        third_building.onClick.AddListener(() => { onBuildingSelected(3); });
        fourth_building.onClick.AddListener(() => { onBuildingSelected(4); });
        fifth_building.onClick.AddListener(() => { onBuildingSelected(5); });





    }

    //Aktion die ausgeführt wird wenn der Plusbutton für das Baumenu geklickt wird
    public void onClickedPlusButton()
    {
        isShowing = !isShowing;
        buildingCanvas.SetActive(isShowing);
        building_Number = 0;
        Debug.Log(transform);
       
    }

    public void deactivateBuildMode()
    {
        building_Number = 0;
    }

    //Aktion die ausgeführt wird wenn auf die Erste Epoche im Baumenu geklickt wird
    public void onFirstYearClicked()
    {
        second_buildings_list.SetActive(false);
        third_buildings_list.SetActive(false);

        first_buildings_list.SetActive(true);

    }

    public void onSecondYearClicked()
    {
        first_buildings_list.SetActive(false);
        third_buildings_list.SetActive(false);

        second_buildings_list.SetActive(true);
    }

    public void onThirdYearClicked()
    {
        first_buildings_list.SetActive(false);
        second_buildings_list.SetActive(false);

        third_buildings_list.SetActive(true);

    }

    //Listener für die geklickten Gebäude mit Index übergabe
    public void onBuildingSelected(int count)
    {
        building_Number = count;
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
