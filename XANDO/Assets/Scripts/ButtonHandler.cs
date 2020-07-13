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

    public void MultiplayerClicked()
    {
        PanelAnimation.PA.MultiplayerAnim();
    }

    public void SinglePlayerModeClicked(int option)
    {
        PanelAnimation.PA.SingleModeAnim();
        GameController.GameC.gameType = GAME_TYPE.AI;

        switch (option)
        {
            case 0:
                AIController.AIC.gameMode = GAME_MODE.EASY;
                break;
            case 1:
                AIController.AIC.gameMode = GAME_MODE.MEDIUM;
                break;
            case 2:
                AIController.AIC.gameMode = GAME_MODE.IMPOSSIBLE;
                break;
        }


    }

    public void MultiplayerModeClicked(int option)
    {
        switch (option)
        {
            case 0:

                break;
            case 1:
                GameController.GameC.gameType = GAME_TYPE.RANDOM;
                PanelAnimation.PA.MatchMakingAnim();
                break;
            case 2:

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

    }

    public void HomeClicked()
    {

    }

    public void MusicClicked(bool isOn)
    {

    }

    public void CancelClicked()
    {
        PanelAnimation.PA.CancelMatchmakingAnim();
    }


}
