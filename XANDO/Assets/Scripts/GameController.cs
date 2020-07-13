using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Photon.Pun;
using System.Xml.Linq;

public class GameController : MonoBehaviour
{
    public GAME_TYPE gameType = GAME_TYPE.AI;

    #region INSTANTIATE
    public static GameController GameC;


    private void OnEnable()
    {
        GameController.GameC = this;
    }
    #endregion

    // Start is called before the first frame update
    int turn = 0;

    public GameObject X;
    public GameObject O;

    public GameObject LR;

    public int[] GridValue = new int[9];

    public bool isLocalMultiplayer = false;

    public Texture2D X_tex = null;

    public Texture2D O_tex = null;

    public void OnButtonClicked(int buttonNumber)
    {
        switch (gameType)
        {
            case GAME_TYPE.AI:
                AIController.AIC.RemoveGridPoint(buttonNumber);
                SetValueOnGrid(buttonNumber);
                break;
            case GAME_TYPE.LOCAL:
                SetValueOnGrid(buttonNumber);
                break;
            case GAME_TYPE.CUSTOM:
                SetValueInRPC(buttonNumber);
                break;
            case GAME_TYPE.RANDOM:
                SetValueInRPC(buttonNumber);
                break;
        }
    }

    public void SetValueInRPC(int buttonNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetValueOnGrid(buttonNumber);
            PhotonManager.PM.SendMyButtonCall(buttonNumber);
            GridController.GC.SwitchOffGameButtons(false);
        }
        else
        {
            SetValueOnGrid(buttonNumber);
            PhotonManager.PM.SendMyButtonCall(buttonNumber);
            GridController.GC.SwitchOffGameButtons(false);
        }
    }

    public void RPCValueRecvdButton(int buttonNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetValueOnGrid(buttonNumber);
            SwitchOnButtonsForTurn();
        }
        else
        {
            SetValueOnGrid(buttonNumber);
            SwitchOnButtonsForTurn();
        }
    }

    public void SetValueOnGrid(int buttonNumber)
    {
        turn++;

        if (turn % 2 == 0)
        {
            GridController.GC.OnButtonClicked(buttonNumber, true);
            GridValue[buttonNumber] = 1;
        }
        else
        {
            GridController.GC.OnButtonClicked(buttonNumber, false);
            GridValue[buttonNumber] = 2;
        }

        Debug.Log("TURN VALUE: " + turn);
    }

    void SwitchOnButtonsForTurn()
    {
        for(int i = 0; i < GridValue.Length; i++)
        {
            if(GridValue[i] == 0)
            {
                GridController.GC.GridButtons[i].interactable = true;
            }
        }
    }

    public void CheckWinner()
    {
        if (GridValue[0] == GridValue[1] && GridValue[0] == GridValue[2] && GridValue[0] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[0].transform.position, GridController.GC.GridButtons[2].transform.position));
        }
        else if (GridValue[3] == GridValue[4] && GridValue[3] == GridValue[5] && GridValue[3] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[3].transform.position, GridController.GC.GridButtons[5].transform.position));
        }
        else if (GridValue[6] == GridValue[7] && GridValue[6] == GridValue[8] && GridValue[6] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[6].transform.position, GridController.GC.GridButtons[8].transform.position));
        }
        else if (GridValue[2] == GridValue[5] && GridValue[2] == GridValue[8] && GridValue[2] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[2].transform.position, GridController.GC.GridButtons[8].transform.position));
        }
        else if (GridValue[0] == GridValue[3] && GridValue[3] == GridValue[6] && GridValue[0] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[0].transform.position, GridController.GC.GridButtons[6].transform.position));
        }
        else if (GridValue[1] == GridValue[4] && GridValue[1] == GridValue[7] && GridValue[1] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[1].transform.position, GridController.GC.GridButtons[7].transform.position));
        }
        else if (GridValue[0] == GridValue[4] && GridValue[4] == GridValue[8] && GridValue[0] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[0].transform.position, GridController.GC.GridButtons[8].transform.position));
        }
        else if (GridValue[2] == GridValue[4] && GridValue[2] == GridValue[6] && GridValue[2] != 0)
        {
            StartCoroutine(LineDraw(GridController.GC.GridButtons[2].transform.position, GridController.GC.GridButtons[6].transform.position));
        }
        else
        {
            if (turn < 9)
            {
                if(gameType == GAME_TYPE.AI)
                {
                    if (turn % 2 != 0)
                    {
                        AIController.AIC.AITurn();
                    }
                }
                else if(gameType == GAME_TYPE.LOCAL)
                {
                    if (turn % 2 != 0)
                    {
                        SetCursorAsX();
                    }
                    else
                    {
                        SetCursorAsO();
                        //Cursor.SetCursor(X_Texture, Vector2.zero, CursorMode.Auto);
                    }
                }
                else if(gameType == GAME_TYPE.RANDOM)
                {
                    
                }
            }
            else
            {
                MenuController.MC.SetWinner(WINNER.DRAW);
            }
        }
    }

    public void SetCursorAsO()
    {
        Cursor.SetCursor(O_tex, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorAsX()
    {
        Cursor.SetCursor(X_tex, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorNull()
    {
        Texture2D nullTex = null;

        Cursor.SetCursor(nullTex, Vector2.zero, CursorMode.Auto);
    }

    public int GetPlayerCornerPoint()
    {
        if (GridValue[0].Equals(2))
            return 0;
        else if (GridValue[2].Equals(2))
            return 2;
        else if (GridValue[6].Equals(2))
            return 6;
        else if (GridValue[8].Equals(2))
            return 8;
        else
            return -1;
    }

    public int GetPlayerMiddleEdgePoint()
    {
        if (GridValue[1].Equals(2))
            return 1;
        else if (GridValue[3].Equals(2))
            return 3;
        else if (GridValue[5].Equals(2))
            return 5;
        else if (GridValue[7].Equals(2))
            return 7;
        else
            return -1;
    }

    public int CheckConsecutiveActive(int val)
    {
        //ROW CHECK
        for (int i = 0; i < 7; i += 3)
        {
            if (GridValue[i].Equals(GridValue[i + 1]) && GridValue[i].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i+2))
            {
                return i+2;
            }
            else if (GridValue[i].Equals(GridValue[i + 2]) && GridValue[i].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i + 1))
            {
                return i+1;
            }
            else if (GridValue[i + 1].Equals(GridValue[i + 2]) && GridValue[i+1].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i))
            {
                Debug.Log("ROW SUCCEFULL: " + i);
                return i;
            }
        }

        //COLUMNS CHECK
        for(int i = 0; i < 3; i++)
        {
            if (GridValue[i].Equals(GridValue[i + 3]) && GridValue[i].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i + 6))
            {
                return i+6;
            }
            else if (GridValue[i].Equals(GridValue[i + 6]) && GridValue[i].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i + 3))
            {
                return i+3;
            }
            else if (GridValue[i + 3].Equals(GridValue[i + 6]) && GridValue[i+3].Equals(val) && AIController.AIC.GridEmptyPoints.Contains(i))
            {
                Debug.Log("COLUMN SUCCEFULL: " + i);
                return i;
            }
        }

        //DIAGNOL CHECK
        if ((GridValue[0] == val && GridValue[4] == val && AIController.AIC.GridEmptyPoints.Contains(8)) ||
           (GridValue[0] == val && GridValue[8] == val && AIController.AIC.GridEmptyPoints.Contains(4)) ||
           (GridValue[4] == val && GridValue[8] == val && AIController.AIC.GridEmptyPoints.Contains(0)))
        {
            Debug.Log("DIAG 1 SUCCEFULL: ");


            if (!GridValue[0].Equals(val))
                return 0;
            else if (!GridValue[4].Equals(val))
                return 4;
            else
                return 8;
        }
        else if ((GridValue[2] == val && GridValue[4] == val && AIController.AIC.GridEmptyPoints.Contains(6)) ||
           (GridValue[2] == val && GridValue[6] == val && AIController.AIC.GridEmptyPoints.Contains(4)) ||
           (GridValue[4] == val && GridValue[6] == val && AIController.AIC.GridEmptyPoints.Contains(2)))
        {
            Debug.Log("DIAG 2 SUCCEFULL: ");

            if (!GridValue[2].Equals(val))
                return 2;
            else if (!GridValue[4].Equals(val))
                return 4;
            else
                return 6;
        }
        else
        {
            return -1;
        }
    }

    public void DeclareWinner()
    {

        if (turn % 2 == 0)
        {
            MenuController.MC.SetWinner(WINNER.XWINS);
        }
        else
        {
            MenuController.MC.SetWinner(WINNER.OWINS);
        }
    }

    IEnumerator LineDraw(Vector3 start, Vector3 end)
    {
        float t = 0;
        float time = 0.3f;

        start.z = 0f;
        end.z = 0f;

        GameObject myLine = new GameObject();

        myLine.transform.parent = LR.transform;

        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();

        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        lr.material.color = Color.black;

        lr.startColor = Color.black;
        lr.endColor = Color.black;

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.SetPosition(0, start);


        Vector3 newpos;
        for (; t < time; t += Time.deltaTime)
        {
            newpos = Vector3.Lerp(start, end, t / time);
            lr.SetPosition(1, newpos);
            yield return null;
        }
        lr.SetPosition(1, end);

        Destroy(myLine, 1);

        DeclareWinner();
    }

    public void ResetVariables()
    {
        //if (myLine.GetComponent<LineRenderer>())
        //{
        //    Destroy(myLine.GetComponent<LineRenderer>());
        //}
        GridValue = new int[9];
        Debug.Log("GridValue: " + GridValue);
        turn = 0;
    }


}

public enum GAME_TYPE
{
    AI = 0,
    LOCAL = 1,
    RANDOM = 2,
    CUSTOM = 3,
}
