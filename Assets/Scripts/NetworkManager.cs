using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform spawnPointOne;
    public Transform spawnPointTwo;
    public Transform spawnPointThree;
    public Transform spawnPointFour;
    public Transform spawnPointFive;
    public Transform spawnPointSix;
    public Transform spawnPointSeven;

    public GameObject haupthaus;

    public Font captureIt2;

    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    private bool exists = false;
    private bool showGui = true;

    private bool schwaben = true;
    private bool niederbayern = true;
    private bool oberbayern = true;
    private bool oberpfalz = true;
    private bool oberfranken = true;
    private bool mittelfranken = true;
    private bool unterfranken = true;

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("v4.2");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (showGui)
        {
            GUI.Box(new Rect(-5, -5, Screen.width + 10, Screen.height + 10), backgroundImage);
            guiStyle.fontSize = 40;
            guiStyle.font = captureIt2;
            GUI.Label(new Rect(Screen.width/2 - 200, 75, 200, 50), "Siedler of Bavaria", guiStyle);
            if (!PhotonNetwork.connected)
            {
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            }
            else if (PhotonNetwork.room == null)
            {
                // nur 1 raum kann geöffnet werden!(Museum nur ein Tisch --> nur ein Raum!


                if (GUI.Button(new Rect(Screen.width/2 - 125, Screen.height/2 - 50, 250, 100), "Hier klicken um am Spiel teilzunehmen!"))
                {

                    PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 7, IsOpen = true, IsVisible = true }, lobbyName);
                    showGui = false;
                }
            }
        }
    }

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(lobbyName);
    }



    void OnReceivedRoomListUpdate()
    {
        Debug.Log("Room was created");
        //roomsList = PhotonNetwork.GetRoomList();
    }


    void OnJoinedLobby()
    {

        Debug.Log("Joined Lobby");
        //PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 7, IsOpen = true, IsVisible = true }, lobbyName);
            
    }
    //Log if connected
    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        // Spawn player
        checkPlayer(PhotonNetwork.countOfPlayers);


    }

    private void checkPlayer(int playersCount)
    {
        if(playersCount == 1)
        {
            instantiatePlayer(spawnPointOne.position, spawnPointOne.rotation, "Schwaben", spawnPointOneHex);
        }
        else
        {
            //mehr spieler als einer -->prüfen, welche Spieler vorhanden
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if (PhotonNetwork.playerList[i].NickName.Equals("Schwaben"))
                {
                    schwaben = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Oberpfalz"))
                {
                    oberpfalz = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Oberbayern"))
                {
                    oberbayern = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Niederbayern"))
                {
                    niederbayern = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Unterfranken"))
                {
                    unterfranken = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Mittelfranken"))
                {
                    mittelfranken = false;
                }
                if (PhotonNetwork.playerList[i].NickName.Equals("Oberfranken"))
                {
                    oberfranken = false;
                }
            }
            //erstbeste noch nicht belegte Stadt wird zugewissen
            if (schwaben)
            {
               instantiatePlayer(spawnPointOne.position, spawnPointOne.rotation, "Schwaben", spawnPointOneHex);
            }
            else
            {
                if(unterfranken)
                {
                    instantiatePlayer(spawnPointTwo.position, spawnPointTwo.rotation, "Unterfranken", spawnPointTwoHex);
                }
                else
                {
                    if (niederbayern)
                    {
                        instantiatePlayer(spawnPointThree.position, spawnPointThree.rotation, "Niederbayern", spawnPointThreeHex);
                    }
                    else
                    {
                        if (oberbayern)
                        {
                            instantiatePlayer(spawnPointFour.position, spawnPointFour.rotation, "Oberbayern", spawnPointFourHex);
                        }
                        else
                        {
                            if (schwaben)
                            {
                                instantiatePlayer(spawnPointFive.position, spawnPointFive.rotation, "Oberpfalz", spawnPointFiveHex);
                            }
                            else
                            {
                                if (oberfranken)
                                {
                                    instantiatePlayer(spawnPointSix.position, spawnPointSix.rotation, "Oberfranken", spawnPointSixHex);
                                }
                                else
                                {
                                    if (mittelfranken)
                                    {
                                        instantiatePlayer(spawnPointSeven.position, spawnPointSeven.rotation, "Mittelfranken", spawnPointSevenHex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    //überprüft anzahl der spieler und weißt neuem Spieler entsprechende Stadt zu
    /*private void checkPlayers(int playerNumber)
    {
        switch (playerNumber)
        {
            case (1):
                exists = true;
                //checkt alle vorhandenen Spieler, ob Schwaben bereits existiert
                //- nein --> Spieler wird Schwaben
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    exists = false;
                    if (PhotonNetwork.playerList[i].NickName.Equals("Schwaben"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointOne.position, spawnPointOne.rotation, "Schwaben", spawnPointOneHex);
                    break;
                }
                break;
            case (2):
                exists = true;
                //checkt alle vorhandenen Spieler, ob Unterfranken bereits existiert
                //- nein --> Spieler wird Unterfranken
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    exists = false;
                    if (PhotonNetwork.playerList[i].NickName.Equals("Unterfranken"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointTwo.position, spawnPointTwo.rotation, "Unterfranken", spawnPointTwoHex);
                    break;
                }           
                break;
            case (3):
                exists = true;
                //checkt alle vorhandenen Spieler, ob Niederbayern bereits existiert
                //- nein --> Spieler wird Niederbayern
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    exists = false;
                    if (PhotonNetwork.playerList[i].NickName.Equals("Niederbayern"))
                    {

                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointThree.position, spawnPointThree.rotation, "Niederbayern", spawnPointThreeHex);
                    break;
                }
                break;
            case (4):
                exists = false;
                //checkt alle vorhandenen Spieler, ob Oberbayern bereits existiert
                //- nein --> Spieler wird Oberbayern
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Oberbayern"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointFour.position, spawnPointFour.rotation, "Oberbayern", spawnPointFourHex);
                    break;
                }
                break;
            case (5):
                exists = false;
                //checkt alle vorhandenen Spieler, ob Oberpfalz bereits existiert
                //- nein --> Spieler wird Oberpfalz
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Oberpfalz"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointFive.position, spawnPointFive.rotation, "Oberpfalz", spawnPointFiveHex);
                    break;
                }
                break;
            case (6):
                exists = false;
                //checkt alle vorhandenen Spieler, ob Oberfranken bereits existiert
                //- nein --> Spieler wird Oberfranken
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Oberfranken"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointSix.position, spawnPointSix.rotation, "Oberfranken", spawnPointSixHex);
                    break;
                }
                break;
            case (7):
                exists = false;
                //checkt alle vorhandenen Spieler, ob Mittelfranken bereits existiert
                //- nein --> Spieler wird Mittelfranken
                //- ja --> nächste Stadt
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Mittelfranken"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        exists = true;
                        checkPlayers(playerNumber);
                        break;
                    }
                }
                if (!exists)
                {
                    instantiatePlayer(spawnPointSeven.position, spawnPointSeven.rotation, "Mittelfranken", spawnPointSevenHex);
                    break;
                }
                break;
        }

        
    }*/

    private void instantiatePlayer(Vector3 spawnPosition, Quaternion spawnRotation, string playerName, GameObject spawnHex)
    {
        Instantiate(cameraPrefab, spawnPosition, spawnRotation);
        Instantiate(interfacePrefab, spawnPosition, spawnRotation);
        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPosition, spawnRotation, 0);
        PhotonNetwork.playerName = playerName;
        GameObject house_go = (GameObject)PhotonNetwork.Instantiate(haupthaus.name, spawnHex.transform.position, Quaternion.identity, 0);
        house_go.name = "Dorfzentrum";
        house_go.transform.GetChild(0).tag = "Dorfzentrum";
        house_go.transform.parent = spawnHex.transform;
        MeshRenderer mr = spawnHex.GetComponentInChildren<MeshRenderer>();
        mr.material.color = Color.red;
    }
}
