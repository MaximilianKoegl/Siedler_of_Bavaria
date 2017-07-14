using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour {

    public GameObject popUpInfo;

    public Button closePopUp;

    private Resources_Counter m_resources_counter;

    private bool yourCity = false;
    private bool doIt = false;

	// Use this for initialization
	void Start () {
        m_resources_counter = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<Resources_Counter>();


        popUpInfo.SetActive(false);

    }

    public bool getDoIt()
    {
        return doIt;
    }

    public void onGetDorf()
    {
        if (doIt)
        {
            Debug.Log("true");
            m_resources_counter.addGold(2000);
            onClosePopUp();
            yourCity = true;
            doIt = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Pop Up schließen
    public void onClosePopUp()
    {
        popUpInfo.SetActive(false);
    }

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
            infoText.text = "Drücken sie den + Button, um ihr Wissen und ihre Bildung einzusetzen, um ein Bündnis mit " + name + " zu schließen? + Button drücken!";
        }
        popUpInfo.SetActive(true);
    }

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
            infoText.text = "Drücken sie den + Button, um " + name + " durch ihr Militär einzunehmen!";
        }

        
        popUpInfo.SetActive(true);
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
            case ("Wahrzeichen"): return "Wahrzeichen...";
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
}
