using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Tooltip("Panel Waiting For Player")]
    public GameObject PanelWaitingForPlayer;

    [Tooltip("Panel Lobby")]
    public GameObject PanelLobby;

    List<RoomInfo> availableRooms = new List<RoomInfo>();

    UnityEngine.Events.UnityAction buttonCallback;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.5";
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

            //make button work
            buttonCallback = () => this.OnClickJoinRoom(roomInfo.Name);
            rowRoom.transform.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(buttonCallback);

        }
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
        //load the main game level
        //PhotonNetwork.LoadLevel("MainGame");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
        //check if there are two players in this room
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{
            //load main game
			PhotonNetwork.LoadLevel("MainGame");
		}
	}

    public void OnClickJoinRoom(string roomName)
    {
        //set our player's nickname
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;

        //join the room
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PanelLobby.SetActive(false);
        PanelWaitingForPlayer.SetActive(true);
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
