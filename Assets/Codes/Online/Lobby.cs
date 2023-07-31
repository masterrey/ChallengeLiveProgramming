using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;

public class Lobby : MonoBehaviourPunCallbacks
{
    private bool isConnecting;

    void Awake()
    {

      PhotonNetwork.AutomaticallySyncScene = true;

    }
    void Start()
    {
      
    }


    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
        // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joining Room...");
        }
      else
        {
        // #Critical, we must first and foremost connect to Photon Online Server.
        PhotonNetwork.GameVersion = "2";
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting...");
        }
    }

    public override void OnConnectedToMaster()
    {
     
        if (isConnecting)
        {
            Debug.Log("OnConnectedToMaster: Next -> try to Join Random Room");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed: Next -> Create a new Room");
       
        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnDisconnected(DisconnectCause cause)
    { 
        Debug.LogError("Disconnected");

        isConnecting = false;
       

    }
    public override void OnJoinedRoom()
    {
      
        Debug.Log("Now this client is in a room.");

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");

            // #Critical
            // Load the Room Level. 
            PhotonNetwork.LoadLevel("GameOlineVS");

        }
    }
}
