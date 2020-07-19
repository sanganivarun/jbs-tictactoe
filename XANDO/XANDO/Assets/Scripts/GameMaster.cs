using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region INSTANTIATE
    public static GameMaster GM;
    void Start()
    {
        GameMaster.GM = this;
    }
    #endregion


    #region PLAYERPREFS DATA
    public List<int> myScoreVsAI = new List<int> { 0, 0, 0 };
    public List<int> aiScoreVsME = new List<int> { 0, 0, 0 };

    void GetScoreVsAI()
    {

        if(PlayerPrefs.HasKey("EASY_MYSCORE"))
        {
            myScoreVsAI[0] = PlayerPrefs.GetInt("EASY_MYSCORE");
        }
        else
        {
            PlayerPrefs.SetInt("EASY_MYSCORE", 0);
        }

        if (PlayerPrefs.HasKey("EASY_AISCORE"))
        {
            aiScoreVsME[0] = PlayerPrefs.GetInt("EASY_AISCORE");
        }
        else
        {
            PlayerPrefs.SetInt("EASY_AISCORE", 0);
        }



        if (PlayerPrefs.HasKey("MEDIUM_MYSCORE"))
        {
            myScoreVsAI[1] = PlayerPrefs.GetInt("MEDIUM_MYSCORE");
        }
        else
        {
            PlayerPrefs.SetInt("MEDIUM_MYSCORE", 0);
        }

        if (PlayerPrefs.HasKey("MEDIUM_AISCORE"))
        {
            aiScoreVsME[1] = PlayerPrefs.GetInt("MEDIUM_AISCORE");
        }
        else
        {
            PlayerPrefs.SetInt("MEDIUM_AISCORE", 0);
        }



        if (PlayerPrefs.HasKey("IMPOSSIBLE_MYSCORE"))
        {
            myScoreVsAI[2] = PlayerPrefs.GetInt("IMPOSSIBLE_MYSCORE");
        }
        else
        {
            PlayerPrefs.SetInt("IMPOSSIBLE_MYSCORE", 0);
        }

        if (PlayerPrefs.HasKey("IMPOSSIBLE_AISCORE"))
        {
            aiScoreVsME[2] = PlayerPrefs.GetInt("IMPOSSIBLE_AISCORE");
        }
        else
        {
            PlayerPrefs.SetInt("IMPOSSIBLE_AISCORE", 0);
        }

    }


    #endregion
    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void IncrementScore(int winner, GAME_MODE gameMode)
    {
        Debug.Log("winner: " + winner + " Game Mode: " + gameMode);

        switch(gameMode)
        {
            case GAME_MODE.EASY:
                if(winner == 0)
                {
                    myScoreVsAI[0]++;
                }
                else
                {
                    aiScoreVsME[0]++;
                }
                break;
            case GAME_MODE.MEDIUM:
                if (winner == 0)
                {
                    myScoreVsAI[1]++;
                }
                else
                {
                    aiScoreVsME[1]++;
                }
                break;
            case GAME_MODE.IMPOSSIBLE:
                if (winner == 0)
                {
                    myScoreVsAI[2]++;
                }
                else
                {
                    aiScoreVsME[2]++;
                }
                break;
        }

        PlayerPrefs.Save();
    }


}
