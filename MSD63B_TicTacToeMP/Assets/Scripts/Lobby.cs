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

    List<RoomInfo> availableRooms = new List<RoomInfo>();



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


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("Number of rooms:"+roomList.Count);
        availableRooms = roomList;
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        foreach(RoomInfo roomInfo in availableRooms)
        {
            //instantiate row prefab
            GameObject rowRoom = Instantiate(RowRoom);
            rowRoom.transform.parent = ScrollViewContent.transform;
            rowRoom.transform.localScale = Vector3.one;

            //update content inside prefab
            rowRoom.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomInfo.Name;
            rowRoom.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = roomInfo.PlayerCount.ToString();

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
