using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class CustomRoomManager : MonoBehaviour
{

    #region INSTANTIATE
    public static CustomRoomManager CRM;

    void OnEnable()
    {
        CustomRoomManager.CRM = this;
    }
    #endregion

    [Header("Main Panel")]
    public GameObject customRoomPanel;
    public GameObject crossButton;

    [Header("Top Button Panel")]
    public Button createButton;
    public Button joinButton;

    [Header("Create Room Panel")]
    public Text shadowText;
    public Text roomText;
    public Button generate;
    public GameObject cancelButtonGO;
    public InputField createRoomIF;
    public GameObject createPanel;

    [Header("Join Room Panel")]
    public Text inputText;
    public InputField enterRoomIF;
    public Button enterButton;
    public GameObject joinPanel;

    [Header("Button Sprites")]
    public Sprite buttonOn;
    public Sprite buttonOff;

    void Start()
    {
        //createRoomClicked();
    }

    void Update()
    {
        if (createButton.interactable)
        {
            createButton.image.sprite = buttonOff;
        }
        else
        {
            createButton.image.sprite = buttonOn;

        }


        if (joinButton.interactable)
        {
            joinButton.image.sprite = buttonOff;
        }
        else
        {
            joinButton.image.sprite = buttonOn;
        }
    }

    #region TOP PANEL FUNCTIONS

    public void CreateTopPanelClicked()
    {
        createButton.interactable = false;
        joinButton.interactable = true;

        createPanel.SetActive(true);
        joinPanel.SetActive(false);
    }

    public void JoinTopPanelClicked()
    {
        createButton.interactable = true;
        joinButton.interactable = false;

        createPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void SetCRActive(bool isOn)
    {
        customRoomPanel.SetActive(isOn);
    }

    #endregion

    #region CREATE ROOM FUNCTIONS

    public void SetRoomText(string roomValue)
    {
        roomText.text = roomValue;
    }

    const string allLetters = "abcdefghijklmnopqrstuvwxyz";

    public static string roomName = "";

    public void createRoomClicked()
    {
        PhotonManager.PM.CreateCustomLobby(getRoomName(),false);
        generate.transform.gameObject.SetActive(false);
        shadowText.text = "generating..";
    }


    private string getRoomName()
    {
        roomName = "";

        for (int i = 0; i < 5; i++)
        {
            roomName += allLetters[Random.Range(0, allLetters.Length)];
        }

        return roomName;
    }

    public void RoomCreated()
    {
        PanelAnimation.PA.HostAnimation();
        generate.transform.gameObject.SetActive(false);
        cancelButtonGO.SetActive(true);
        createRoomIF.text = roomName;
        shadowText.text = "";
        crossButton.SetActive(false);
    }

    public void RoomDestroyed()
    {
        crossButton.SetActive(true);
        PhotonManager.PM.LeavePhotonRoom();
        PanelAnimation.PA.HostAnimationCancel();
        generate.transform.gameObject.SetActive(true);
        generate.interactable = false;
        cancelButtonGO.SetActive(false);
        createRoomIF.text = "";
        shadowText.text = "click generate";
    }

    #endregion

    #region JOIN ROOM FUNCTIONS

    public void OnValueChanged(string value)
    {
        roomName = value;
        if(value.Length == 5)
        {
            enterButton.transform.gameObject.SetActive(true);
        }
        else
        {
            enterButton.transform.gameObject.SetActive(false);
        }
    }

    public void OnEndEdit(string value)
    {
        if(value.Length == 5)
        {
            roomName = value;
            enterButton.transform.gameObject.SetActive(true);
        }
        else
        {
        }
    }

    public void EnterButtonClicked()
    {
        enterButton.transform.gameObject.SetActive(false);
        PhotonManager.PM.CreateCustomLobby(roomName,true);
    }

    public void EnterRoomFailed()
    {
        enterButton.transform.gameObject.SetActive(true);
    }

    #endregion

    public void ResetCustomRoom()
    {
        shadowText.text = "click generate";
        createRoomIF.text = "";
        cancelButtonGO.SetActive(false);
        generate.transform.gameObject.SetActive(true);
        enterButton.transform.gameObject.SetActive(false);
        enterRoomIF.text = "";
    }

}
