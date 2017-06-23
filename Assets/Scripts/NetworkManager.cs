using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.MonoBehaviour {

    private const string roomName = "Bavarian Museum";
    private RoomInfo[] roomsList;
    private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
    public GameObject cameraPrefab;
    public GameObject mouseManagerPrefab;
    public GameObject interfacePrefab;
    public Texture2D backgroundImage;

    public Transform spawnPointOne;
    public Transform spawnPointTwo;
    public Transform spawnPointThree;
    public Transform spawnPointFour;
    public Transform spawnPointFive;
    public Transform spawnPointSix;
    public Transform spawnPointSeven;

    bool showGui = true;

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
            if (!PhotonNetwork.connected)
            {
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            }
            else if (PhotonNetwork.room == null)
            {
                // nur 1 raum kann geöffnet werden!(Museum nur ein Tisch --> nur ein Raum!


                if (GUI.Button(new Rect(100, 100, 250, 100), "Join " + roomName))
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
        roomsList = PhotonNetwork.GetRoomList();
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
        checkPlayers(PhotonNetwork.countOfPlayers);


    }


    //überprüft anzahl der spieler und weißt neuem Spieler entsprechende Stadt zu
    private void checkPlayers(int playerNumber)
    {
        switch (PhotonNetwork.countOfPlayers)
        {
            case (1):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    //wenn stadt schon vorhanden, dann nächste stadt
                    //wenn nicht, initialisierung
                    if (PhotonNetwork.playerList[i].NickName.Equals("Augsburg"))
                    {
                        playerNumber += 1;
                        if(playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        
                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointOne.position, spawnPointOne.rotation);
                        Instantiate(interfacePrefab, spawnPointOne.position, spawnPointOne.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointOne.position, spawnPointOne.rotation, 0);
                        PhotonNetwork.playerName = "Augsburg";
                        break;
                    }
                }
                
                break;
            case (2):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Würzburg"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }
                        
                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointTwo.position, spawnPointTwo.rotation);
                        Instantiate(interfacePrefab, spawnPointTwo.position, spawnPointTwo.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointTwo.position, spawnPointTwo.rotation, 0);
                        PhotonNetwork.playerName = "Würzburg";
                        break;
                    }
                }                
                break;
            case (3):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Landshut"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }

                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointThree.position, spawnPointThree.rotation);
                        Instantiate(interfacePrefab, spawnPointThree.position, spawnPointThree.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointThree.position, spawnPointThree.rotation, 0);
                        PhotonNetwork.playerName = "Landshut";
                        break;
                    }
                }
                break;
            case (4):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("München"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }

                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointFour.position, spawnPointFour.rotation);
                        Instantiate(interfacePrefab, spawnPointFour.position, spawnPointFour.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointFour.position, spawnPointFour.rotation, 0);
                        PhotonNetwork.playerName = "München";
                        break;
                    }
                }
                break;
            case (5):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Regensburg"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }

                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointFive.position, spawnPointFive.rotation);
                        Instantiate(interfacePrefab, spawnPointFive.position, spawnPointFive.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointFive.position, spawnPointFive.rotation, 0);
                        PhotonNetwork.playerName = "Regensburg";
                        break;
                    }
                }
                break;
            case (6):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Bayreuth"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }

                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointSix.position, spawnPointSix.rotation);
                        Instantiate(interfacePrefab, spawnPointSix.position, spawnPointSix.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointSix.position, spawnPointSix.rotation, 0);
                        PhotonNetwork.playerName = "Bayreuth";
                        break;
                    }
                }
                break;
            case (7):
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i].NickName.Equals("Ansbach"))
                    {
                        playerNumber += 1;
                        if (playerNumber == 8)
                        {
                            playerNumber = 1;
                        }

                        checkPlayers(playerNumber);
                        break;
                    }
                    else
                    {
                        Instantiate(cameraPrefab, spawnPointSeven.position, spawnPointSeven.rotation);
                        Instantiate(interfacePrefab, spawnPointSeven.position, spawnPointSeven.rotation);
                        PhotonNetwork.Instantiate(mouseManagerPrefab.name, spawnPointSeven.position, spawnPointSeven.rotation, 0);
                        PhotonNetwork.playerName = "Ansbach";
                        break;
                    }
                }
                break;
        }
    }
}
