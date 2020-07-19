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
    public Button FriendButton;

    private Button playButton;

    private bool matchStarted = false;

    private bool isCustomRoom = false;

    private bool isJoiningRoom = false;

    #region PHOTON VARIABLES

    public TypedLobby typedLobby;

    public Player[] photonPlayers;

    private int playersInRoom = 0;
    private int myNumberInRoom = 0;
    private int roomSize = 2;

    #endregion

    void Start()
    {
        playButton = PlayButton.GetComponent<Button>();
        PV = GetComponent<PhotonView>();
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        FriendButton.interactable = true;
        CustomRoomManager.CRM.generate.interactable = true;
        PanelAnimation.PA.PlayToast("Connection Successfull");
    }


    #region ON PLAY BUTTON CLICKED
    public void OnPlayButtonClicked()
    {
        isCustomRoom = false;
        isJoiningRoom = false;
        typedLobby = new TypedLobby("tictactoerandomlobby", LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
    }

    #endregion

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
        if (!isJoiningRoom)
        {
            CreateRoom();
        }
        else
        {
            isJoiningRoom = false;
            Debug.LogWarning(message);
            PanelAnimation.PA.PlayToast(message);
            CustomRoomManager.CRM.EnterRoomFailed();
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        playersInRoom++;

        if (playersInRoom == roomSize && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("I AM MASTER AND WE ARE NOW CONNECTED");
            //MenuController.MC.OnPlayButtonClicked();
            PanelAnimation.PA.OnOnlineGameStart();
            InGameUIManager.IGUI.turnText.text = "your turn";
            GameController.GameC.SetCursorAsO();
            retryCounter = 0;
            matchStarted = true;

            if(isCustomRoom)
            {
                CustomRoomManager.CRM.SetCRActive(false);
            }
            HandshakeRequest();
        }

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

        if (isCustomRoom)
        {
            Debug.Log("ROOM CREATED");
            CustomRoomManager.CRM.RoomCreated();
        }

        StartCoroutine(BotTimer());

        
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
        PanelAnimation.PA.OnOnlineGameStart();
        CustomRoomManager.CRM.SetCRActive(false);
        InGameUIManager.IGUI.turnText.text = "opponent's turn";
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
        yield return new WaitForSeconds(15);

        
        if (!matchStarted && !isCustomRoom)
        {
            LeavePhotonRoom();

            PanelAnimation.PA.CancelMatchmakingAnim();
            PanelAnimation.PA.PlayToast("Cannot find opponent");
        }

    }

    #endregion

    #region CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ButtonHandler.BH.isConnectedToMaster = false;
        PanelAnimation.PA.PlayToast("Disconnected");
        Debug.Log("Cause: " + cause);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        PanelAnimation.PA.PlayToast("Opponent Left");

        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }
        LoadMainMenu();
    }

    #endregion

    #region LEAVE ROOM

    public void LeavePhotonRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region CUSTOM ROOM 

    public void CreateCustomLobby(string roomName, bool joinRoom)
    {
        isCustomRoom = true;
        isJoiningRoom = joinRoom;
        typedLobby = new TypedLobby("tictactoerandomlobby" + roomName, LobbyType.Default);
        PhotonNetwork.JoinLobby(typedLobby);
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
        }

        LoadMainMenu();

    }

    public void LoadMainMenu()
    {
        MenuController.MC.GameOverPanel.SetActive(false);
        MenuController.MC.InGamePanel.SetActive(false);
        MenuController.MC.ButtonsPanel.SetActive(true);
        PanelAnimation.PA.BackToHomeScreen();
        MenuController.MC.ResetGame();
    }

    #endregion

    #endregion
}
