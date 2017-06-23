using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {

    public GameObject popUpInfo;

    public Button closePopUp;

	// Use this for initialization
	void Start () {
        popUpInfo.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Pop Up schließen
    public void onClosePopUp()
    {
        popUpInfo.SetActive(false);
    }

    //Erzeugt ein Pop Up
    public void onFirstTimeBuild(string name)
    {
        popUpInfo.SetActive(true);
        //popUpClass.HouseName = name;
        switch (name)
        {
            case ("Woodcutter"):
                Debug.Log("Woodcutter");
                //Wood Pop up;
                break;
            case ("Ironfeeder"):
                Debug.Log("Ironfeeder");
                //Iron Pop up;
                break;
            case ("Stonefeeder"):
                Debug.Log("Stonefeeder");
                //Stone Pop up;
                break;
            default:
                break;
        }
    }
}
