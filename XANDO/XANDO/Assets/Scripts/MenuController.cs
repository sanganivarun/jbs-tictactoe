using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public static MenuController MC;



    #region INSTANTIATE
    private void OnEnable()
    {
        MenuController.MC = this;
    }
    #endregion

    [Header("Toggles")]
    public ToggleGroup difficultyGroup = null;

    public ToggleGroup gameModeGroup = null;

    public Toggle[] difficultyToggles = null;


    [Header("Images")]
    public Sprite[] gameOverImages = null;

    public Image GameOverImage = null;


    [Header("Panels")]
    public GameObject ExitGamePanel = null;

    public GameObject DifficultyPanel = null;

    public GameObject MainMenuPanel = null;

    public GameObject InGamePanel = null;

    public GameObject GameOverPanel = null;

    public GameObject ButtonsPanel = null;

    public GameObject TopPanel = null;

    void Start()
    {
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }


    public void ExitButtonClickedUI()
    {
        ExitGamePanel.SetActive(true);
    }

    public void ExitYesClickedUI()
    {
        Application.Quit();
    }

    public void ExitNoClickedUI()
    {
        ExitGamePanel.SetActive(false);
    }

    public void OnMusicToggleClicked(bool isOn)
    {
        
    }

    public void OnPlayButtonClicked()
    {
        if (GameController.GameC.gameType == GAME_TYPE.RANDOM)
        {
            GameController.GameC.SetCursorNull();

            if (PhotonNetwork.IsMasterClient)
            {
                GridController.GC.SwitchOffGameButtons(true);
            }
            else
            {
                GridController.GC.SwitchOffGameButtons(false);
            }
        }
    }


    public void ResetGame()
    {
        GameController.GameC.ResetVariables();
        GridController.GC.ResetVariables();
        AIController.AIC.ResetVariables();
        GameOverImage.sprite = null;
        GameOverImage.color = new Color(255, 255, 255, 0);
        
    }

    public void OnMultiplayerRematch(bool isMasterClient)
    {
        InGamePanel.SetActive(true);
        GameOverPanel.SetActive(false);
        ResetGame();

        if (isMasterClient)
        {
            InGameUIManager.IGUI.turnText.text = "your turn";
            GridController.GC.SwitchOffGameButtons(true);
        }
        else
        {
            InGameUIManager.IGUI.turnText.text = "opponent's turn";
            GridController.GC.SwitchOffGameButtons(false);
        }
    }

}

public enum WINNER
{
    XWINS = 0,
    OWINS = 1,
    DRAW = 2,
}