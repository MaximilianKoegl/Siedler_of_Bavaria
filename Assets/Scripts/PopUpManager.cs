using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {

    public GameObject popUpInfo;

    public Button closePopUp;
    public Button infoPopUp;
    public Button detailsPopUp;

    public Button jaButton;
    public Button neinButton;

    private Resources_Counter m_resources_counter;

    private bool yourCity = false;
    private bool doIt = false;
    private bool startet = false;

    public String actualHouseClicked;

	void Start () {
        m_resources_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();

        popUpInfo.SetActive(false);
        jaButton.gameObject.SetActive(false);
        neinButton.gameObject.SetActive(false);

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


    //
    //wird nur ausgeführt, wenn Uni oder Kaserne gebaut und dass PopUp der Nachbarstadt geöffnet ist
    //wenn vorraussetzungen stimmen wird dorf eingenommen, gold zu den Rohstoffen hinzugefügt 
    public void onGetDorf()
    {
        if (doIt)
        {
            if(m_resources_counter.getKaserneDorfEinnehmbar())
            {

                //losgehen
                for(int i = 0; i< GameObject.FindGameObjectsWithTag("Soldat").Length; i++)
                {
                    AgentWalkment soldat = GameObject.FindGameObjectsWithTag("Soldat")[i].GetComponent<AgentWalkment>();
                    soldat.startWalk();
                }
                startet = true;
                //erst wenn erreicht
                onClosePopUp();
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

        jaButton.gameObject.SetActive(false);
        neinButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (startet)
        {
            //erst wenn erreicht
            bool allOnDestination = true;
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Soldat").Length; i++)
            {
                AgentWalkment soldat = GameObject.FindGameObjectsWithTag("Soldat")[i].GetComponent<AgentWalkment>();
                if (!soldat.getOnDestination())
                {
                    allOnDestination = false;
                    break;
                }
            }
            if (allOnDestination)
            {
                m_resources_counter.addGold(2000);
                yourCity = true;
                startet = false;
            }
            
        }
        
    }

    //Pop Up schließen und setzt alle Button auf inaktiv
    public void onClosePopUp()
    {
        popUpInfo.SetActive(false);
        closePopUp.gameObject.SetActive(false);
        infoPopUp.gameObject.SetActive(false);
        detailsPopUp.gameObject.SetActive(false);

        jaButton.gameObject.SetActive(false);
        neinButton.gameObject.SetActive(false);

    }

    //Öffnet Details des PopUps
    public void onDetailsButtonClicked()
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = getPopUpTitle(actualHouseClicked);
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpDetails(actualHouseClicked);
    }

    //Öffnet Info des PopUps
    public void onInfoButtonClicked()
    {
        Text title = popUpInfo.GetComponentInChildren<Text>();
        title.text = getPopUpTitle(actualHouseClicked);
        Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
        infoText.text = getPopUpInfo(actualHouseClicked);
    }


    //Öffnet dieses PopUp wenn Uni gebaut wurde und Nachbarstadt angeklickt wird
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
            infoText.text = "Durch ihre Bildung konnten sie einen Bünsisvertrag\n mit " + name + " aushandeln. Laut Vertrag erhalten sie beim Abschluss\n des Bündnisses 2000 Gold, müssen im Gegenzug jedoch 2000 Holz,\n 1000 Stein und 2000 Nahrung abgeben.";
            jaButton.gameObject.SetActive(true);
            neinButton.gameObject.SetActive(true);
        }
        
        popUpInfo.SetActive(true);
        closePopUp.gameObject.SetActive(true);
    }


    //Öffnet dieses PopUp wenn Kaserne gebaut wurde und Nachbarstadt angeklickt wird
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
            infoText.text = "Sie können " + name + " durch ihr Militär\n einnehmen! Dazu brauchen sie mindestens\n 3 Soldaten(1 Soldat pro Kaserne)!";
            jaButton.gameObject.SetActive(true);
            neinButton.gameObject.SetActive(true);
        }

        
        popUpInfo.SetActive(true);
        closePopUp.gameObject.SetActive(true);
    }


    //Öffnet dieses Popup zu Spielbeginn
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

    //Erzeugt das Pop Up - kann nur geöffnet werden wenn kein PopUp zurzeit offen ist
    public void onFirstTimeBuild(string tag)
    {
        if (!popUpInfo.active)
        {
            actualHouseClicked = tag;
            Text title = popUpInfo.GetComponentInChildren<Text>();
            title.text = getPopUpTitle(tag);
            Text infoText = popUpInfo.transform.Find("InfoText").GetComponentInChildren<Text>();
            infoText.text = getPopUpInfo(tag);
            closePopUp.gameObject.SetActive(true);
            infoPopUp.gameObject.SetActive(true);
            detailsPopUp.gameObject.SetActive(true);
            popUpInfo.SetActive(true);
        }
    }


    //Gibt den Titel eines PopUps anhand des zugewiesen Tags zurück
    private string getPopUpTitle(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Holzfäller";
            case ("Ironfeeder"): return "Eisenmine";
            case ("Stonefeeder"): return "Steinmine";
            case ("LivingHouse"): return "Wohnhaus";
            case ("Church"): return "Kapelle";
            default: return tag;
        }
    }

    
    //gibt die Info eines popUps anhand des zugewiesen Tag zurück
    private string getPopUpInfo(String tag)
    {
        switch (tag)
        {
            case ("Woodcutter"): return "Produziert Holz.";
            case ("Ironfeeder"): return "Produziert Eisen.";
            case ("Stonefeeder"): return "Produziert Stein.";
            case ("Dorfzentrum"): return "Dies ist das Dorfzentrum.";
            case ("LivingHouse"): return "Fuggerei in Augsburg als erste Sozialsiedlung Deutschlands";
            case ("Church"): return "16. Jahrhundert als Zeit der Reformation und Gegenreformation";
            case ("Brauerei"): return "Hofbräuhaus in München";
            case ("Bäcker"): return "Einwohner brauchen Essen";
            case ("Schmiede"): return "Dies ist eine Schmiede";
            case ("Kaserne"): return "Dies ist eine Kaserne";
            case ("Schule"): return "Dies ist eine Schule";
            case ("Universität"): return "Dies ist eine Universität";
            case ("Wahrzeichen"): return "Dies ist ein Wahrzeichen";
            case ("Augsburg"): return "Augsburg erlebte im  16. Jahrhundert eine wirtschaftliche Blütezeit. Auch die Glaubenspaltung ist mit Augsburg verbunden. So wurde auf dem Reichstag 1555 der Religionsfrieden beschlossen.";
            case ("Regensburg"): return "Zu Beginn des 16. Jahrhunderts lieferte sich Regensburg eine Auseinanersetzung mit Kaiser Maximilian den I. und widersetzte sich der Berufung des Kaiserlichen Reichshauptmanns für Regensburg.";
            case ("Landshut"): return "Der Beginn des 16. Jahrhunderts stellte für Landshut die Ende seiner glanzvollen Zeit dar. Mit dem Tod von Georg dem Reichen wurde Landshut mit München vereinigt, da dieser keine nachfahren hatte.";
            case ("Bayreuth"): return "Bayreuth stellte sich bereits 1528 - also bereits 11 Jahre nach Beginn der Reformation - den lutheranischen Glaubensbekenntnis an.";
            case ("München"): return "München stellte mit des 16. Jahrhundert das Zentrum der Gegenreformation dar. Der Protstantismus wurde hier  1555 komplett verboten.";
            case ("Ansbach"): return "In Ansbach wurden im 16. Jahrhundert mehrere Frauen der Hexerei bezichtigt in hingerichtet.";
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
        int wood = m_resources_counter.woodCount;
        int stone = m_resources_counter.stoneCount;
        int iron = m_resources_counter.ironCount;
        int food = m_resources_counter.foodCount;
        int gold = m_resources_counter.goldCount;
        int people = m_resources_counter.einwohnerCount;
        float satisfaction = m_resources_counter.getSatisfaction();
        float satisfactionFood = m_resources_counter.getSatisfactionFood();

        switch (tag)
        {
            case ("Dorfzentrum"):
                return "Holz: " + wood + "\nStein: " + stone + "\nEisen: " + iron + "\nNahrung: " + food + "\nGold: " + gold + "\nEinwohner: " + people + "\nZufriedenheit: " + satisfaction + "\nZufriedenheit Essen: " + satisfactionFood;
            case ("Woodcutter"): return "Aktuell " + wood + " Holz zur Verfügung!" ;
            case ("Ironfeeder"): return "Aktuell " + iron + " Eisen zur Verfügung!";
            case ("Stonefeeder"): return "Aktuell  " + stone + " Stein zur Verfügung!";
            case ("LivingHouse"): return "Aktuell " + people + " Leute in ihrem Regierungsbezirk!" + "\nZufriedenheit Essen: " + satisfactionFood + "%\nZufriedenheit: " + satisfaction + "%";
            case ("Universität"): return "Die Universität ermöglicht den Bewohnern die Bildung zu verbessern und hilft so den Lebensstandart und die Zufriedenheit zu erhöhen.";
            case ("Kaserne"): return "Die Kaserne hilft ihren Einwohnern, sich sicherer zu fühlen und trägt dadurch zur Zufriedenheit ihrer Einwohner bei! Jede Kaserne beherbergt einen Soldaten!";
            case ("Schmiede"): return "Die Schmiede ist  die Grundvoraussetzung, um eine Kaserne bauen zu können. Hier werden die Waffen geschmiedet, welche die Soldaten spöter benutzen.";
            case ("Schule"): return "Die Schule ist die Grundvoraussetzung, um eine Universität bauen zu können.";
            case ("Church"): return "Die Kapelle hilft ihren Bewohnern dabei, ihren Glauben auszuleben. Dadurch sind sie zufriedener un fühlen sich von Gott beschützt.";
            case ("Brauerei"): return "Die Brauerei stellt die Getränke für ihre Einwohner her.";
            case ("Bäcker"): return "Die Bäckerei nutzt das Getreide dazu, um die Stadt mit Lebensmitteln zu versorgen, und so das Leben zu ermöglichen. Ist zu wenig Nahrung vorhanden, sinkt die Zufriedenheit der Bürger, und dadurch die Rohstoffproduktion";
            case ("Wahrzeichen"): return "Das Wahrzeichen ist das Aushängeschild der Stadt!";
            case ("Bamberg"): return "Bamberg ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Nürnberg"): return "Nürnberg ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Aschaffenburg"): return "Aschaffenburg ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Weiden"): return "Weiden ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Ingolstadt"): return "Ingolstadt ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Kempten"): return "Kempten ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            case ("Passau"): return "Passau ist die zweitgrößte Stadt in ihren Regierungsbezirk. In ihr wird Gold abgebaut.";
            default:
                return "No Details found!";
        }
    }
}
