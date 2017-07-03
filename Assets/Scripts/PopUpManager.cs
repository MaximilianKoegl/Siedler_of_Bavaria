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

    public void OnGameStartInfo(String name)
    {

        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = name;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(name);
        popUpInfo.SetActive(true);
    }

    //Erzeugt ein Pop Up
    public void onFirstTimeBuild(string tag)
    {
        
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = getPopUpTitle(tag);
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(tag);
        popUpInfo.SetActive(true);
        //popUpClass.HouseName = name;
        
    }

    private string getPopUpTitle(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Holzfäller";
            case ("Ironfeeder"): return "Eisenmine";
            case ("Stonefeeder"): return "Steinmine";
            case ("Dorfzentrum"): return "Dorfzentrum";
            default: return "";
        }
    }

    private string getPopUpInfo(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Produziert Holz.";
            case ("Ironfeeder"): return "Produziert Eisen.";
            case ("Stonefeeder"): return "Produziert Stein.";
            case ("Dorfzentrum"): return "Dies ist das Dorfzentrum.";
            case ("Augsburg"): return "Augsburg erlebte im  16. Jahrhundert eine wirtschaftliche Blütezeit. Auch die Glaubenspaltung ist mit Augsburg verbunden. So wurde auf dem Reichstag 1555 der Religionsfrieden beschloßen.";
            case ("Regensburg"): return "Regensburg wurde im 16. Jahrhundert von ... regiert.";
            case ("Landshut"): return "Landshut wurde im 16. Jahrhundert von ... regiert.";
            case ("Bayreuth"): return "Bayreuth wurde im 16. Jahrhundert von ... regiert.";
            case ("München"): return "München wurde im 16. Jahrhundert von ... regiert.";
            case ("Ansbach"): return "Ansbach wurde im 16. Jahrhundert von ... regiert.";
            case ("Würzburg"): return "Würzburg wurde im 16. Jahrhundert von ... regiert.";
            default: return "";
        }
    }
}
