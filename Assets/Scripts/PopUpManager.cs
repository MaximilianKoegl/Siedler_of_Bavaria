﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {

    public GameObject popUpInfo;

    public Button closePopUp;
    public Button infoPopUp;
    public Button detailsPopUp;

    private Resources_Counter m_resources_counter;

    private bool yourCity = false;
    private bool doIt = false;

    public String actualHouseClicked;

	// Use this for initialization
	void Start () {
        m_resources_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();


        popUpInfo.SetActive(false);

    }

    //wird vom Baumenü aufgerufen
    // gibt aktuellen Wahrheitswert von doIt zurück

    public bool getDoIt()
    {
        return doIt;
    }

    public void setDoItFalse()
    {
        doIt = false;
    }


    //wird aufgerufen, wenn + Button gedrückt wird
    //wird nur ausgeführt, wenn Uni oder Kaserne gebaut und dass PopUp der Nachbarstadt geöffnet ist
    //wenn vorraussetzungen stimmen wird dorf eingenommen, gold zu den Rohstoffen hinzugefügt 
    public void onGetDorf()
    {
        if (doIt)
        {
            Debug.Log("true");
            if(m_resources_counter.getKaserneDorfEinnehmbar())
            {
                m_resources_counter.addGold(2000);
                onClosePopUp();
                yourCity = true;
                doIt = false;
            }else{
                if (m_resources_counter.getUniDorfEinnehmbar())
                {
                    m_resources_counter.addGold(2000);
                    m_resources_counter.removeResources();
                    onClosePopUp();
                    yourCity = true;
                    doIt = false;
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Pop Up schließen
    public void onClosePopUp()
    {
        popUpInfo.SetActive(false);
        closePopUp.gameObject.SetActive(false);
        infoPopUp.gameObject.SetActive(false);
        detailsPopUp.gameObject.SetActive(false);
    }

    //Öffnet Details Info
    public void onDetailsButtonClicked()
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = actualHouseClicked;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpDetails(actualHouseClicked);
    }

    //Öffnet Details Info
    public void onInfoButtonClicked()
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = actualHouseClicked;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(actualHouseClicked);
    }


    //öffnet dieses popUp wenn Uni gebaut wurde und Nachbarstadt angeklickt wird
    public void dorfInfoUni(String name)
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = name;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        if (yourCity)
        {
            infoText.text = "Sie sind mit Kempten verbündet.";
        }
        else
        {
            doIt = true;
            infoText.text = "Durch ihre Bildung konnten sie einen Bünisvertrag mit " + name + " aushandeln. Laut Vertrag erhalten sie beim Abschluss des Bündnisses 2000 Gold, müssen im Gegenzug jedoch 2000 Holz, 1000 Stein und 2000 Nahrung abgeben. Drücken sie den + Button, um das Bündnis " + name + " zu schließen! ";
        }
        popUpInfo.SetActive(true);
        closePopUp.gameObject.SetActive(true);
    }


    //öffnet dieses popUp wenn Kaserne gebaut wurde und Nachbarstadt angeklickt wird
    public void dorfInfoKaserne(String name)
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = name;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        if (yourCity)
        {
            
            infoText.text = "Sie haben Kempten eingenommen.";
        }
        else
        {
            doIt = true;
            infoText.text = "Sie können " + name + " durch ihr Militär einnehmen! Dazu brauchen sie mindestens 3 Soldaten(Kaserne bauen)! Drücken sie den + Button, um " + name + " einzunhemen!";
        }

        
        popUpInfo.SetActive(true);
        closePopUp.gameObject.SetActive(true);
    }


    //öffnet dieses Popup zu Spielbeginn
    public void OnGameStartInfo(String name)
    {

        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = name;
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(name);
        popUpInfo.SetActive(true);
        closePopUp.gameObject.SetActive(true);

        infoPopUp.gameObject.SetActive(false);
        detailsPopUp.gameObject.SetActive(false);

    }

    //Erzeugt ein Pop Up
    public void onFirstTimeBuild(string tag)
    {

        actualHouseClicked = tag;
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = getPopUpTitle(tag);
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(tag);
        closePopUp.gameObject.SetActive(true);
        infoPopUp.gameObject.SetActive(false);
        detailsPopUp.gameObject.SetActive(false);
        popUpInfo.SetActive(true);

        if (tag == "Dorfzentrum")
        {
            infoPopUp.gameObject.SetActive(true);
            detailsPopUp.gameObject.SetActive(true);
        }
        
    }


    //gibt den Titel eines popUps anhand des zugewiesen Tags zurück
    private string getPopUpTitle(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Holzfäller";
            case ("Ironfeeder"): return "Eisenmine";
            case ("Stonefeeder"): return "Steinmine";
            case ("Dorfzentrum"): return "Dorfzentrum";
            case ("LivingHouse"): return "Wohnhaus";
            case ("Church"): return "Kapelle";
            case ("Brauerei"): return "Brauerei";
            case ("Bäcker"): return "Bäcker";
            case ("Schmiede"): return "Schmiede";
            case ("Kaserne"): return "Kaserne";
            case ("Schule"): return "Schule";
            case ("Universität"): return "Universität";
            case ("Wahrzeichen"): return "Wahrzeichen";
            case ("Bamberg"): return "Bamberg.";
            case ("Nürnberg"): return "Nürnberg.";
            case ("Aschaffenburg"): return "Aschaffenburg.";
            case ("Weiden"): return "Weiden.";
            case ("Ingolstadt"): return "Ingolstadt.";
            case ("Kempten"): return "Kempten.";
            case ("Passau"): return "Passau.";
            default: return "";
        }
    }

    // Unterscheidung zwischen Städten! Auch bei Holzfäller und Co wsl!
    //gibt die Info eines popUps anhand des zugewiesen Tag zurück
    private string getPopUpInfo(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Produziert Holz.";
            case ("Ironfeeder"): return "Produziert Eisen.";
            case ("Stonefeeder"): return "Produziert Stein.";
            case ("Dorfzentrum"): return "Dies ist das Dorfzentrum.";
            case ("LivingHouse"): float satisfaction = m_resources_counter.getSatisfaction(); float satisfactionFood = m_resources_counter.getSatisfactionFood(); return "Zufriedenheit Essen: " + satisfactionFood + "%\nZufriedenheit: " + satisfaction +"%\nFuggerei in Augsburg als erste Sozialsiedlung Deutschlands";
            case ("Church"): return "16. Jahrhundert als Zeit der Reformation und Gegenreformation";
            case ("Brauerei"): return "Hofbräuhaus in München";
            case ("Bäcker"): return "Einwohner brauchen Essen";
            case ("Schmiede"): return "Dies ist eine Schmiede";
            case ("Kaserne"): return "Dies ist eine Kaserne";
            case ("Schule"): return "Dies ist eine Schule";
            case ("Universität"): return "Dies ist eine Universität";
            case ("Wahrzeichen"): return "Herzlichen Glückwunsch, du hast das Wahrzeichen gebaut und damit das Ende des Ziels erreicht!";
            case ("Augsburg"): return "Augsburg erlebte im  16. Jahrhundert eine wirtschaftliche Blütezeit. Auch die Glaubenspaltung ist mit Augsburg verbunden. So wurde auf dem Reichstag 1555 der Religionsfrieden beschlossen.";
            case ("Regensburg"): return "Regensburg wurde im 16. Jahrhundert von ... regiert.";
            case ("Landshut"): return "Landshut wurde im 16. Jahrhundert von ... regiert.";
            case ("Bayreuth"): return "Bayreuth wurde im 16. Jahrhundert von ... regiert.";
            case ("München"): return "München wurde im 16. Jahrhundert von ... regiert.";
            case ("Ansbach"): return "Ansbach wurde im 16. Jahrhundert von ... regiert.";
            case ("Würzburg"): return "Während des Bauernkrieges 1525 wurde die Würzburger Festung Mariental mehrfach berannt. Die Würzburger stellten sich dabei auf die Seite der Bauern.";
            case ("Bamberg"): return "Dies ist Bamberg.";
            case ("Nürnberg"): return "Dies ist Nürnberg.";
            case ("Aschaffenburg"): return "Dies ist Aschaffenburg.";
            case ("Weiden"): return "Dies ist Weiden.";
            case ("Ingolstadt"): return "Dies ist Ingolstadt.";
            case ("Kempten"): return "Dies ist Kempten.";
            case ("Passau"): return "Dies ist Passau.";
            default: return "";
        }
    }

    //Details bei Häusern mit mehreren Tabs
    private string getPopUpDetails(String tag)
    {
        switch (tag)
        {
            case ("Dorfzentrum"):
                int wood = m_resources_counter.woodCount;
                int stone = m_resources_counter.stoneCount;
                int iron = m_resources_counter.ironCount;
                int food = m_resources_counter.foodCount;
                int people = m_resources_counter.einwohnerCount;
                float satisfaction = m_resources_counter.getSatisfaction();
                float satisfactionFood = m_resources_counter.getSatisfactionFood();
                return "Holz: " + wood + "\nStein: " + stone + "\nEisen: " + iron + "\nNahrung: " + food + "\nEinwohner: " + people + "\nZufriedenheit: " + satisfaction + "\nZufriedenheit Essen: " + satisfactionFood;
            default:
                return "No Details found!";
        }
    }
}
