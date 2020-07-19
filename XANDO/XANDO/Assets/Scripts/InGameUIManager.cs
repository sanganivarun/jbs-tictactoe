using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{

    public static InGameUIManager IGUI;

    // Start is called before the first frame update
    void Start()
    {
        InGameUIManager.IGUI = this;
    }

    public List<string> opponentNames = new List<string> { };
    //import from a json file

    #region LABELS UPDATE


    [Header("SCORE TEXTS")]
    public Text PlayerScoreText;
    public Text OpponentNameText;
    public Text OpponentScoreText;
    public Text PlayerNameText;
    public Text turnText;


    public void UpdateInGameTexts(int OpponentName, int OpponentScore, int MyScore)
    {

        PlayerScoreText.text = MyScore.ToString();
        OpponentScoreText.text = OpponentScore.ToString();
        turnText.text = "";

        switch (OpponentName)
        {
            case 0:
                this.OpponentNameText.text = "Easy";
                break;
            case 1:
                this.OpponentNameText.text = "Medium";
                break;
            case 2:
                this.OpponentNameText.text = "Impossible";
                break;
            case 3:
                this.OpponentNameText.text = "X";
                break;
            default:
                this.OpponentNameText.text = "Opponent";
                break;
        }

        switch (GameController.GameC.gameType)
        {
            case GAME_TYPE.AI:
                PlayerNameText.text = "You";
                break;
            case GAME_TYPE.LOCAL:
                PlayerNameText.text = "O";
                break;
            case GAME_TYPE.RANDOM:
                PlayerNameText.text = "You";
                break;
            case GAME_TYPE.CUSTOM:
                PlayerNameText.text = "You";
                break;
        }

        SetTextPanel(true);
    }
    #endregion


    [Header("PANEL")]
    public GameObject InGamePanel;
    public GameObject TextPanel;


    public void SetInGamePanel(bool isOn)
    {
        InGamePanel.SetActive(isOn);
    }

    public void SetTextPanel(bool isOn)
    {
        TextPanel.SetActive(isOn);
    }

    public void UpdateLabelOnGameOver(bool isPlayerWon)
    {
        if(isPlayerWon)
        {
            PlayerScoreText.text = (int.Parse(PlayerScoreText.text) + 1).ToString();
        }
        else
        {
            OpponentScoreText.text = (int.Parse(OpponentScoreText.text) + 1).ToString();
        }
    }


}
