using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour, IPunObservable
{
    public PhotonView photonView;
    public GameManager gameManager;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }


    //called when the player clicks on a board piece
    public void NotifySelectBoardPiece(GameObject gameObject)
    {

        if ((int)gameManager.currentActivePlayer.id == PhotonNetwork.LocalPlayer.ActorNumber)  //allow only current player to call the RPC method
        {
           
            photonView.RPC("RPC_NotifySelectBoardPiece", RpcTarget.All, gameObject.name);
        }
     
    }

    //going to be called by Photon
    [PunRPC]
    public void RPC_NotifySelectBoardPiece(string gameObjectName)
    {
        GetComponent<GameManager>().SelectBoardPiece(GameObject.Find(gameObjectName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
