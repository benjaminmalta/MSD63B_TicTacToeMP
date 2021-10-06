using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{

    [Tooltip("Scoll View Content Game Object")]
    public GameObject ScrollViewContent;

    [Tooltip("UI Row Prefab")]
    public GameObject RowRoom;

    [Tooltip("Input Player Name")]
    public GameObject InputPlayerName;

    [Tooltip("Input Room Name")]
    public GameObject InputRoomName;

    [Tooltip("Status Message")]
    public GameObject LblStatusMessage;

    [Tooltip("Button Create Room")]
    public GameObject BtnCreateRoom;



    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.0";
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnGUI()
    {
        LblStatusMessage.GetComponent<TextMeshProUGUI>().text =
            "Status:" + PhotonNetwork.NetworkClientState.ToString();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)2;

        PhotonNetwork.JoinOrCreateRoom(InputRoomName.GetComponent<TMP_InputField>().text,
            roomOptions, TypedLobby.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
