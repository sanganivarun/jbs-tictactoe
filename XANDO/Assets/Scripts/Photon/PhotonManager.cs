using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
//using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks, ILobbyCallbacks, IInRoomCallbacks
{
    #region INSTANTIATE
    public static PhotonManager PM = null;

    public override void OnEnable()
    {
        if(PhotonManager.PM == null)
        {
            PhotonManager.PM = this;
        }
        else
        {
            if(PhotonManager.PM != this)
            {
                Destroy(PhotonManager.PM.gameObject);
                PhotonManager.PM = this;
            }
        }

        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }


    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    #endregion


    private PhotonView PV;

    [Header("UI ELEMENTS")]
    public GameObject PlayButton;

    private Button playButton;

    private bool matchStarted = false;

    #region PHOTON VARIABLES

    public TypedLobby typedLobby;

    public Player[] photonPlayers;

    public int playersInRoom = 0;
    public int myNumberInRoom = 0;
    public string roomName = "";
    public int roomSize = 2;

    #endregion

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        playButton = PlayButton.GetComponent<Button>();
        PV = GetComponent<PhotonView>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        playButton.interactable = true;
    }

    public void OnPlayButtonClicked()
    {
        GameController.GameC.gameType = GAME_TYPE.RANDOM;
        //PhotonNetwork.JoinRandomRoom();
        typedLobby = new TypedLobby("tictactoerandomlobby", LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
    }

    #region JOIN LOBBY

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("ON JOINED LOBBY");
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region JOIN ROOM CALLBACKS

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("JOIN ROOM FAILED");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;

        Debug.Log("Photon players in room: " + playersInRoom + " myNumberInRoom: " + myNumberInRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        playersInRoom++;

        if (playersInRoom == roomSize && PhotonNetwork.IsMasterClient)
        {
            //CarromRPC.instance.CallGiveDetails(CarromGameManager.carromPlayerDetails[0].PlayerName, CarromGameManager.carromPlayerDetails[0].PlayerProfilePic, CarromGameManager.carromPlayerDetails[0].player_uid, CarromGameManager.carromPlayerDetails[0].playerStats.total, CarromGameManager.carromPlayerDetails[0].playerStats.wins, CarromGameManager.carromPlayerDetails[0].playerStats.loss, CarromGameManager.carromPlayerDetails[0].playerStats.ratio);
            Debug.Log("I AM MASTER AND WE ARE NOW CONNECTED");
            MenuController.MC.OnPlayButtonClicked();
            retryCounter = 0;
            matchStarted = true;
            HandshakeRequest();
        }

        Debug.Log("PLAYER IN ROOMS");
    }

    #endregion

    #region CREATE ROOM
    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsVisible = true;
        roomOps.IsOpen = true;
        roomOps.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        StartCoroutine(BotTimer());
        Debug.Log("ROOM IS SUCCESSFULLY CREATED");
    }

    #endregion

    #region RPC CALLS

    public void HandshakeRequest()
    {
        PV.RPC("RPC_Handshake", RpcTarget.Others);
    }

    public void SendMyButtonCall(int buttonNumber)
    {
        PV.RPC("RPC_Button", RpcTarget.Others, buttonNumber);
    }

    public void RematchRequest()
    {
        PV.RPC("RPC_Rematch", RpcTarget.Others);
    }

    public void BeginRematch()
    {
        PV.RPC("RPC_BeginRematch", RpcTarget.Others);
    }

    #endregion

    #region RPC HANDLERS

    [PunRPC]
    void RPC_Handshake()
    {
        Debug.Log("has master recvd message: " + PhotonNetwork.MasterClient);
        MenuController.MC.OnPlayButtonClicked();
        matchStarted = true;
        retryCounter = 0;
    }

    [PunRPC]
    void RPC_Button(int buttonNumber)
    {
        GameController.GameC.RPCValueRecvdButton(buttonNumber);
    }

    [PunRPC]
    void RPC_Rematch()
    {
        RetryCounter();
    }

    [PunRPC]
    void RPC_BeginRematch()
    {
        MenuController.MC.OnMultiplayerRematch(false);
    }

    #endregion

    #region BOT TIMER

    public IEnumerator BotTimer()
    {
        for(float i = 10; i > 0; i -= Time.deltaTime)
        {
            yield return null;
        }

        if (!matchStarted)
        {
            PhotonNetwork.LeaveRoom();
        }
        //start bot game here

        //Debug.Log("Time Up:  " + PhotonNetwork.InLobby);
    }

    #endregion

    #region CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Cause: " + cause);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            LoadMainMenu();
        }
        else
        {

        }
    }

    #endregion

    #region GAMEPLAY FUNCTIONS


    #region REMATCH
    public void RetryRequest()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            RetryCounter();
        }
        else
        {
            RematchRequest();
        }
    }

    private int retryCounter = 0;
    public void RetryCounter()
    {
        retryCounter++;

        if(retryCounter == 2)
        {
            retryCounter = 0;
            MenuController.MC.OnMultiplayerRematch(true);
            BeginRematch();
        }
    }


    public void LeaveRoomOnHomeClicked()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            LoadMainMenu();
        }
        else
        {

        }
    }

    public void LoadMainMenu()
    {
        MenuController.MC.GameOverPanel.SetActive(false);
        MenuController.MC.InGamePanel.SetActive(false);
        MenuController.MC.MainMenuPanel.SetActive(true);

        MenuController.MC.ResetGame();
    }

    #endregion

    #endregion
}
