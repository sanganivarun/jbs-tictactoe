using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{

    #region INSTANTIATE
    public static ButtonHandler BH = null;

    void OnEnable()
    {
        ButtonHandler.BH = this;
    }

    #endregion

    public void SingleplayerClicked()
    {
        PanelAnimation.PA.SingleplayerAnim();
    }

    public bool isConnectedToMaster = false;
    public void MultiplayerClicked()
    {
        PanelAnimation.PA.MultiplayerAnim();
        CustomRoomManager.CRM.ResetCustomRoom();
        if(!isConnectedToMaster)
        {
            isConnectedToMaster = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void SinglePlayerModeClicked(int option)
    {
        PanelAnimation.PA.StartGameAnim(true);

        GameController.GameC.gameType = GAME_TYPE.AI;

        switch (option)
        {
            case 0:
                AIController.AIC.gameMode = GAME_MODE.EASY;
                InGameUIManager.IGUI.UpdateInGameTexts(0, GameMaster.GM.aiScoreVsME[0], GameMaster.GM.myScoreVsAI[0]);
                break;
            case 1:
                AIController.AIC.gameMode = GAME_MODE.MEDIUM;
                InGameUIManager.IGUI.UpdateInGameTexts(1, GameMaster.GM.aiScoreVsME[1], GameMaster.GM.myScoreVsAI[1]);
                break;
            case 2:
                AIController.AIC.gameMode = GAME_MODE.IMPOSSIBLE;
                InGameUIManager.IGUI.UpdateInGameTexts(2, GameMaster.GM.aiScoreVsME[2], GameMaster.GM.myScoreVsAI[2]);
                break;
        }

        InGameUIManager.IGUI.SetInGamePanel(true);
    }

    public void MultiplayerModeClicked(int option)
    {

        switch (option)
        {
            case 0:
                PanelAnimation.PA.StartGameAnim(false);
                GameController.GameC.gameType = GAME_TYPE.LOCAL;
                InGameUIManager.IGUI.UpdateInGameTexts(3, 0, 0);
                InGameUIManager.IGUI.SetInGamePanel(true);
                break;
            case 1:
                GameController.GameC.gameType = GAME_TYPE.RANDOM;
                PanelAnimation.PA.MatchMakingAnim();
                break;
            case 2:
                GameController.GameC.gameType = GAME_TYPE.RANDOM;
                CustomRoomManager.CRM.SetCRActive(true);
                MenuController.MC.ButtonsPanel.SetActive(false);
                MenuController.MC.TopPanel.SetActive(false);
                PanelAnimation.PA.multiGO.SetActive(false);
                break;
        }

        
    }

    public void ExitButtonClicked()
    {
        MenuController.MC.ExitButtonClickedUI();
    }

    public void ExitYesClicked()
    {
        MenuController.MC.ExitYesClickedUI();
    }

    public void ExitNoClicked()
    {
        MenuController.MC.ExitNoClickedUI();
    }

    public void ExitInGameClicked()
    {

    }

    public void ExitInGameYesClicked()
    {

    }

    public void ExitInGameNoClicked()
    {

    }

    public void RetryClicked()
    {
        if (GameController.GameC.gameType == GAME_TYPE.AI)
        {
            InGameUIManager.IGUI.SetInGamePanel(true);
            GameOverUIManager.GOUI.GameOverPanel.SetActive(false);
            GameController.GameC.SetCursorNull();
            MenuController.MC.ResetGame();
        }
        else if(GameController.GameC.gameType == GAME_TYPE.LOCAL)
        {
            InGameUIManager.IGUI.SetInGamePanel(true);
            GameOverUIManager.GOUI.GameOverPanel.SetActive(false);
            GameController.GameC.SetCursorAsO();
            MenuController.MC.ResetGame();
        }
        else if (GameController.GameC.gameType == GAME_TYPE.RANDOM)
        {
            PhotonManager.PM.RetryRequest();
            PanelAnimation.PA.PlayToast("Request Sent !");
            GameOverUIManager.GOUI.retryButton.interactable = false;
        }
    }

    public void HomeClicked()
    {
        if (GameController.GameC.gameType == GAME_TYPE.AI || GameController.GameC.gameType == GAME_TYPE.LOCAL)
        {
            GameOverUIManager.GOUI.GameOverPanel.SetActive(false);
            InGameUIManager.IGUI.SetInGamePanel(false);
            MenuController.MC.MainMenuPanel.SetActive(true);
            MenuController.MC.ResetGame();
            PanelAnimation.PA.BackToHomeScreen();
        }
        else if (GameController.GameC.gameType == GAME_TYPE.RANDOM)
        {
            PhotonManager.PM.LeaveRoomOnHomeClicked();
        }
    }

    public void MusicClicked(bool isOn)
    {

    }

    public void CancelClicked()
    {
        PhotonManager.PM.LeavePhotonRoom();
        PanelAnimation.PA.CancelMatchmakingAnim();
    }

    public void CustomRoomExit()
    {
        CustomRoomManager.CRM.SetCRActive(false);
        MenuController.MC.ButtonsPanel.SetActive(true);
        //MenuController.MC.TopPanel.SetActive(true);
        PanelAnimation.PA.CancelMatchmakingAnim();
    }


}
