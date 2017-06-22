using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.MonoBehaviour {

    private const string roomName = "Bavarian Museeum";
    private RoomInfo[] roomsList;
    private TypedLobby lobbyName = new TypedLobby("New_Lobby", LobbyType.Default);
    public GameObject cameraPrefab;
    public GameObject mouseManagerPrefab;
    public GameObject interfacePrefab;

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("v4.2");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            // nur 1 raum kann geöffnet werden!(Museum nur ein Tisch --> nur ein Raum!)
            //PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 7, IsOpen = true, IsVisible = true }, lobbyName);

            if (GUI.Button(new Rect(100, 250, 250, 100), "Join " + roomName))
            {
                PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 7, IsOpen = true, IsVisible = true }, lobbyName);
            }

            // Join Room
            /*if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].Name))
                    {
                        PhotonNetwork.JoinRoom(roomsList[i].Name);
                    }
                }
            }*/
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
        Instantiate(cameraPrefab, Vector3.up * 5, Quaternion.identity);
        Instantiate(interfacePrefab, Vector3.up * 5, Quaternion.identity);
        PhotonNetwork.Instantiate(mouseManagerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);


    }
}
