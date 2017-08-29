using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System;

public class NetworkManager : Photon.MonoBehaviour {

    private const string roomName = "Bavarian Museum";
    private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
    public GameObject cameraPrefab;
    public GameObject mouseManagerPrefab;
    public GameObject interfacePrefab;
    public Texture2D backgroundImage;

    public GameObject spawnPointOneHex;
    public GameObject spawnPointTwoHex;
    public GameObject spawnPointThreeHex;
    public GameObject spawnPointFourHex;
    public GameObject spawnPointFiveHex;
    public GameObject spawnPointSixHex;
    public GameObject spawnPointSevenHex;
    public GameObject spawnPointDorfOneHex;
    public GameObject spawnPointDorfTwoHex;
    public GameObject spawnPointDorfThreeHex;
    public GameObject spawnPointDorfFourHex;
    public GameObject spawnPointDorfFiveHex;
    public GameObject spawnPointDorfSixHex;
    public GameObject spawnPointDorfSevenHex;
    public Transform spawnPointOne;
    public Transform spawnPointTwo;
    public Transform spawnPointThree;
    public Transform spawnPointFour;
    public Transform spawnPointFive;
    public Transform spawnPointSix;
    public Transform spawnPointSeven;

    public GameObject haupthaus;
    public GameObject dorf;
    public Button exitButton;
    public Text leaveGame;


    public Font captureIt2;

    private GUIStyle guiStyle = new GUIStyle(); 

    private bool showGui = true;

    private bool schwabenFree = true;
    private bool niederbayernFree = true;
    private bool oberbayernFree = true;
    private bool oberpfalzFree = true;
    private bool oberfrankenFree = true;
    private bool mittelfrankenFree = true;
    private bool unterfrankenFree = true;

    private bool lastConnection = false;
    private bool exitPressed = false;
    

    
    void Start () {
        PhotonNetwork.ConnectUsingSettings("v4.2");
    }
	
	
	void Update () {

        checkConnectionDuringGame();
    }


    //überprüft, ob eine Änderung der Verbindung zwischen dem letzten und dem aktuellen Frame von verbunden zu nicht verbunden besteht
    //--> Änderung verbunden zu nicht verbunden --> Wenn der Exit Button nicht gedrückt wurde, wird das Spiel verlassen( Aufruf leaveRoom).
    private void checkConnectionDuringGame()
    {
        if (lastConnection && !PhotonNetwork.connected)
        {
            if (!exitPressed)
            {
                leaveRoom(1);
            }
            else
            {
                exitPressed = false;
            }
        }
        lastConnection = PhotonNetwork.connected;
    }


    //verlassen des Raumes = beenden des laufenden Spieles
    //Anzeige abhängig ob Verbindung verloren oder Exit gedrückt
    public void leaveRoom(int i)
    {
        Debug.Log("LeaveRoom");
        if(i == 1)
        {
            leaveGame.text = "Verbindung verloren, verlasse Spiel";
        }
        else
        {
            leaveGame.text = "Verlasse Spiel";
            exitPressed = true;
        }
        leaveGame.gameObject.SetActive(true);
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        hardRestartGame();
    }

    // spiel wird neu gestartet indem die Szene neu geladen wird
    void hardRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    //wird aufgerufen, wenn der Raum verlassen wurde
    void OnLeftRoom()
    {
        OnGUI();
    }


    //zeigt gui elemente
    void OnGUI()
    {
        if (showGui)
        {
            drawBackground();
            drawExitGameButton();
            if (!PhotonNetwork.connected)
            {
                drawReloadGameButton();
            }
            else if (PhotonNetwork.room == null)
            {
                drawStartGameButton();
            }
        }
    }

    //zeichne Spiel verlassen Button
    //wenn der Button geklickt wird, wird das SPiel verlassen
    private void drawExitGameButton()
    {
        if (GUI.Button(new Rect(Screen.width - 250, 50, 200, 60), "Spiel verlassen"))
        {
            Application.Quit();
        }
    }

    // zeichnen des Start Spiel Buttons
    //wenn der Button gedrückt wird der Raum erstellt oder betreten
    private void drawStartGameButton()
    {
        if (GUI.Button(new Rect(Screen.width / 8, Screen.height / 8, Screen.width / 2 + Screen.width / 4, Screen.height / 2 + Screen.height / 4), "Hier klicken um am Spiel teilzunehmen!"))
        {
            //nur ein Raum aufgrund Museum
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 7, IsOpen = true, IsVisible = true }, lobbyName);
            showGui = false;
        }
    }

    // zeichnen des Reload Spiel Buttons
    //bei cklick wird das Spiel gestartet
    private void drawReloadGameButton()
    {
        //GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        if (GUI.Button(new Rect(Screen.width / 8, Screen.height / 8, Screen.width / 2 + Screen.width / 4, Screen.height / 2 + Screen.height / 4), "Keine Verbindung! Neu laden!"))
        {
            Start();
        }
    }



    //Hintergrund wird gezeichnet
    //Label = Spielname wird gesetzt
    private void drawBackground()
    {
        GUI.Box(new Rect(-5, -5, Screen.width + 10, Screen.height + 10), backgroundImage);
        guiStyle.fontSize = 40;
        guiStyle.font = captureIt2;
        GUI.Label(new Rect(Screen.width / 2 - 200, 75, 200, 50), "Settlers of Bavaria", guiStyle);
    }


    //wenn eine Verbindung mit dem Masterserver aufgebaut wurde, wird die Lobby automatisch betreten
    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(lobbyName);
    }


    

    
    void OnReceivedRoomListUpdate()
    {
    }


    void OnJoinedLobby()
    {
        
            
    }


    //Wenn der Spieler einen Raum beigetreten ist, wird checkPlayers aufgerufen
    //--> Anzahl der vorhnanden Spieler wird übergeben
    void OnJoinedRoom()
    {
        // Spawn player
        checkPlayer(PhotonNetwork.countOfPlayers);


    }

    //überprüft, welcher Spieler hinzukommt und welche Stadt er bekommt
    //Überprüfung anhand Spieleranzahl
    private void checkPlayer(int playersCount)
    {
        Debug.Log(playersCount);
        if (playersCount == 1)
        {
            instantiatePlayer(spawnPointOne.position, spawnPointOne.rotation, "Schwaben", spawnPointOneHex);
            instantiateDorfPlayer(spawnPointDorfOneHex, "Kempten");
        }
        else
        {
            checkBelegteStädte();
            //erstbeste noch nicht belegte Stadt wird zugewissen
            assignCity();
            
        }
    }

    //durchläuft die Liste der Spieler und überprüft, welche Stadt belegt ist.
    //weißt dem Spieler die erste frei Stadt zu
    //ruft instantiateCity auf
    private void assignCity()
    {
        if (schwabenFree)
        {
            instantiateCity(spawnPointOne, spawnPointOneHex, spawnPointDorfOneHex, "Schwaben", "Kempten");
        }
        else
        {
            if (unterfrankenFree)
            {
                instantiateCity(spawnPointTwo, spawnPointTwoHex, spawnPointDorfTwoHex, "Unterfranken", "Aschaffenburg");
            }
            else
            {
                if (niederbayernFree)
                {
                    instantiateCity(spawnPointThree, spawnPointThreeHex, spawnPointDorfThreeHex, "Niederbayern", "Passau");
                }
                else
                {
                    if (oberbayernFree)
                    {
                        instantiateCity(spawnPointFour, spawnPointFourHex, spawnPointDorfFourHex, "Oberbayern", "Ingolstadt");
                    }
                    else
                    {
                        if (oberpfalzFree)
                        {
                            instantiateCity(spawnPointFive, spawnPointFiveHex, spawnPointDorfFiveHex, "Oberpfalz", "Weiden");
                        }
                        else
                        {
                            if (oberfrankenFree)
                            {
                                instantiateCity(spawnPointSix, spawnPointSixHex, spawnPointDorfSixHex, "Oberfranken", "Bamberg");
                            }
                            else
                            {
                                if (mittelfrankenFree)
                                {
                                    instantiateCity(spawnPointSeven, spawnPointSevenHex, spawnPointDorfSevenHex, "Mittelfranken", "Nürnberg");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //ruft instantiatePlayer und instantiateDorfPlayer auf
    //Übergibt die Spawn für Camera, die Hexagons für Dorfzentrum und Nachbardorf, Bezirknamen und Name des Nachbarsdorfes
    private void instantiateCity(Transform spawnPoint, GameObject spawnPointHex, GameObject spawnPointDorfHex, string bezirk, string nachbarsdorf)
    {
        instantiatePlayer(spawnPoint.position, spawnPoint.rotation, bezirk, spawnPointHex);
        instantiateDorfPlayer(spawnPointDorfHex, nachbarsdorf);
    }

    //läuft alle Spielernamen (= Stadt) ab und weißt einen bool wert zu, falls eine Stadt schon vorhanden ist
    private void checkBelegteStädte()
    {
        
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            switch (PhotonNetwork.playerList[i].NickName)
            {
                case ("Schwaben"): schwabenFree = false; break;
                case ("Oberpfalz"): oberpfalzFree = false; break;
                case ("Oberbayern"): oberbayernFree = false; break;
                case ("Niederbayern"): niederbayernFree = false; break;
                case ("Unterfranken"): unterfrankenFree = false; break;
                case ("Mittelfranken"): mittelfrankenFree = false; break;
                case ("Oberfranken"): oberfrankenFree = false; break;
                default: break;
            }
            /*
            if (PhotonNetwork.playerList[i].NickName.Equals("Schwaben"))
            {
                schwabenFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Oberpfalz"))
            {
                oberpfalzFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Oberbayern"))
            {
                oberbayernFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Niederbayern"))
            {
                niederbayernFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Unterfranken"))
            {
                unterfrankenFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Mittelfranken"))
            {
                mittelfrankenFree = false;
            }
            if (PhotonNetwork.playerList[i].NickName.Equals("Oberfranken"))
            {
                oberfrankenFree = false;
            }*/
        }
    }


    //camera, Interface, Button, MouseManager und Dorfzentrum instanziert
    //Spielname wird zugewiesen
    private void instantiatePlayer(Vector3 spawnPosition, Quaternion spawnRotation, string playerName, GameObject spawnHex)
    {
        Instantiate(cameraPrefab, spawnPosition, spawnRotation);
        Instantiate(interfacePrefab, spawnPosition, spawnRotation);

        instantiateButtons();
        
        //als Netzwerkobject
        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPosition, spawnRotation, 0);
        PhotonNetwork.playerName = playerName;

        spawnDorfzentrum(spawnHex);
        
    }
    
    
    //exit ButtonListener wird gesetzt
    //Spiel verlassen Button des Startmenüs wird ausgeblendet und inactive
    private void instantiateButtons()
    {
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(() => { leaveRoom(0); });
        leaveGame = GameObject.Find("LeaveGameInfo").GetComponent<Text>();
        leaveGame.gameObject.SetActive(false);
    }

    //Dorfzentrum wird instanziert und gesetzt
    //Kindelemente bekommen Tags zugewiesen, um später auf click zu reagieren
    private void spawnDorfzentrum(GameObject spawnHex)
    {
        GameObject house_go = (GameObject)PhotonNetwork.Instantiate(haupthaus.name, spawnHex.transform.position, Quaternion.identity, 0);
        house_go.name = "Dorfzentrum";
        house_go.transform.GetChild(0).GetChild(0).tag = "Dorfzentrum";
        house_go.transform.GetChild(0).GetChild(1).tag = "Dorfzentrum";
        house_go.transform.GetChild(0).GetChild(2).tag = "Dorfzentrum";
        house_go.transform.parent = spawnHex.transform;
    }


    //Nachbarsdorf wird gesetzt
    //Tags werden zugewiesen, um später auf click zu reagieren 
    private void instantiateDorfPlayer(GameObject spawnHex, string dorfName)
    {
        GameObject house_go = (GameObject)PhotonNetwork.Instantiate(dorf.name, spawnHex.transform.position, Quaternion.identity, 0);
        house_go.name = dorfName;
        house_go.transform.GetChild(0).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(0).GetChild(0).GetChild(1).tag = dorfName;
        house_go.transform.GetChild(0).GetChild(0).GetChild(2).tag = dorfName;
        house_go.transform.GetChild(1).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(1).GetChild(0).GetChild(1).tag = dorfName;
        house_go.transform.GetChild(1).GetChild(0).GetChild(2).tag = dorfName;
        house_go.transform.GetChild(2).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(3).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(3).GetChild(0).GetChild(1).tag = dorfName;
        house_go.transform.GetChild(4).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(5).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(5).GetChild(0).GetChild(1).tag = dorfName;
        house_go.transform.GetChild(6).GetChild(0).GetChild(0).tag = dorfName;
        house_go.transform.GetChild(6).GetChild(0).GetChild(1).tag = dorfName;
        house_go.transform.GetChild(6).GetChild(0).GetChild(2).tag = dorfName;
        house_go.transform.GetChild(6).GetChild(0).GetChild(3).tag = dorfName;
        house_go.transform.parent = spawnHex.transform;
    }
}
