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

    private int gameDifficulty = 1;

    private int gameMode = 0; //single = 0; multi = 1

    void Start()
    {
        //GameDifficultySet();
        //SetMediumActive(true);
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void GameDifficultySet()
    {
        if (PlayerPrefs.HasKey("DIFFICULTY"))
        {
            gameDifficulty = PlayerPrefs.GetInt("DIFFICULTY");
            Debug.Log("DIFFICULTY: " + gameDifficulty);
        }
        else
        {
            Debug.Log("DIFFICULTY UNSET: " + gameDifficulty);
            PlayerPrefs.SetInt("DIFFICULTY", 1);
            PlayerPrefs.Save();
            gameDifficulty = 1;
        }

        if (PlayerPrefs.HasKey("GAME_MODE"))
        {
            gameMode = PlayerPrefs.GetInt("GAME_MODE");
            //Debug.Log("GAME_MODE SET: " + gameDifficulty);
        }
        else
        {
            //Debug.Log("GAME_MODE UNSET: " + gameMode);
            PlayerPrefs.SetInt("GAME_MODE", 0);
            PlayerPrefs.Save();
            gameMode = 1;
        }


        switch (gameDifficulty)
        {
            case 0:
                SetEasyActive(true);
                break;
            case 1:
                SetMediumActive(true);
                break;
            case 2:
                SetImpossibleActive(true);
                break;
        }



        switch(gameMode)
        {
            case 0:
                SetSinglePlayerActive(true);
                break;
            case 1:
                SetMultiPlayerActive(true);
                break;
        }

        difficultyGroup.allowSwitchOff = false;
    }


    public void SetSinglePlayerActive(bool isOn)
    {
        GameController.GameC.gameType = GAME_TYPE.AI;
        GameController.GameC.isLocalMultiplayer = false;
        DifficultyPanel.SetActive(true);
    }

    public void SetMultiPlayerActive(bool isOn)
    {
        GameController.GameC.gameType = GAME_TYPE.LOCAL;
        GameController.GameC.isLocalMultiplayer = true;
        DifficultyPanel.SetActive(false);
    }


    public void SetEasyActive(bool isOn)
    {
        difficultyToggles[0].isOn = isOn;
        AIController.AIC.gameMode = GAME_MODE.EASY;
        PlayerPrefs.SetInt("DIFFICULTY", 0);
        Debug.Log("gamemode:" + AIController.AIC.gameMode);
        //return true;
    }

    public void SetMediumActive(bool isOn)
    {
        difficultyToggles[1].isOn = isOn;
        AIController.AIC.gameMode = GAME_MODE.MEDIUM;
        PlayerPrefs.SetInt("DIFFICULTY", 1);
        Debug.Log("gamemode:" + AIController.AIC.gameMode);
        //return true;
    }

    public void SetImpossibleActive(bool isOn)
    {
        difficultyToggles[2].isOn = isOn;
        AIController.AIC.gameMode = GAME_MODE.IMPOSSIBLE;
        PlayerPrefs.SetInt("DIFFICULTY", 2);
        Debug.Log("gamemode:" + AIController.AIC.gameMode);
        //return true;
    }

    public void SetWinner(WINNER winner)
    {
        GridController.GC.SwitchOffGameButtons(false);

        switch(winner)
        {
            case WINNER.XWINS:
                GameOverImage.sprite = gameOverImages[0];
                GameOverImage.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case WINNER.OWINS:
                GameOverImage.sprite = gameOverImages[1];
                GameOverImage.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
            case WINNER.DRAW:
                GameOverImage.sprite = gameOverImages[2];
                GameOverImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
        }

        StartCoroutine(FillImage(GameOverImage));
    }

    IEnumerator FillImage(Image img)
    {
        for (float i = 0; i <= 1; i += (Time.deltaTime * 5))
        {
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        GameOverPanel.SetActive(true);
        OnGameOver();
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

    public void OnGameOver()
    {
        GameController.GameC.SetCursorNull();
    }

    public void OnPlayButtonClicked()
    {
        //PhotonNetwork.ConnectUsingSettings();
        MainMenuPanel.SetActive(false);
        ResetGame();
        InGamePanel.SetActive(true);
        GameController.GameC.SetCursorAsO();


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

    public void OnRetryClicked()
    {

        if (GameController.GameC.gameType == GAME_TYPE.AI || GameController.GameC.gameType == GAME_TYPE.LOCAL)
        {
            InGamePanel.SetActive(true);
            GameOverPanel.SetActive(false);
            GameController.GameC.SetCursorAsO();
            ResetGame();
        }
        else if (GameController.GameC.gameType == GAME_TYPE.RANDOM)
        {
            PhotonManager.PM.RetryRequest();
        }
    }

    public void OnHomeButtonClicked()
    {
        if (GameController.GameC.gameType == GAME_TYPE.AI || GameController.GameC.gameType == GAME_TYPE.LOCAL)
        {
            GameOverPanel.SetActive(false);
            InGamePanel.SetActive(false);
            MainMenuPanel.SetActive(true);

            ResetGame();
        }
        else if(GameController.GameC.gameType == GAME_TYPE.RANDOM)
        {
            PhotonManager.PM.LeaveRoomOnHomeClicked();
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
            GridController.GC.SwitchOffGameButtons(true);
        }
        else
        {
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
