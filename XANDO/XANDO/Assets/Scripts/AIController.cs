using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{

    #region INSTANTIATE
    public static AIController AIC;

    private void OnEnable()
    {
        if (AIController.AIC == null)
        {
            AIController.AIC = this;
        }
        else
        {
            if (AIController.AIC != this)
            {
                Destroy(AIController.AIC.gameObject);
                AIController.AIC = this;
            }
        }
    }
    #endregion

    //public int[] GridEmptyPoints = new int[9];

    public List<int> GridEmptyPoints = new List<int>();

    public GAME_MODE gameMode = GAME_MODE.EASY;

    private List<int> cornerPoints = new List<int>() { 0, 2, 6, 8 };

    private List<int> middleEdgePoints = new List<int> { 1, 3, 5, 7 };

    private int turn = 0;

    //corner points 0,2,6,8
    //center point 4
    //middle edges 1,3,5,7


    void Start()
    {
        InsertValuesInGrid();
    }

    void InsertValuesInGrid()
    {
        Debug.Log("START");


        for (var i = 0; i < GridController.GC.GridButtons.Length; i++)
        {
            GridEmptyPoints.Add(i);
        }
    }

    public void RemoveGridPoint(int buttonNumber)
    {
        GridEmptyPoints.Remove(buttonNumber);
        if (cornerPoints.Contains(buttonNumber))
        {
            //Debug.Log("remopved");
            cornerPoints.Remove(buttonNumber);
        }

        if(middleEdgePoints.Contains(buttonNumber))
        {
            middleEdgePoints.Remove(buttonNumber);
        }
    }

    public void AITurn()
    {

        Debug.Log("AI TURN");
        turn++;

        if (gameMode == GAME_MODE.EASY)
        {
            SetOnRandomPoint();
        }
        else if (gameMode == GAME_MODE.MEDIUM)
        {
            if (turn == 1)
            {
                SetOnCornerPoint();
            }
            else if (turn > 1)
            {
                int val = GameController.GameC.CheckConsecutiveActive(2);

                if (val != -1)
                {
                    Debug.Log("CONTAINS: " + val);

                    if (GridEmptyPoints.Contains(val))
                    {
                        GameController.GameC.OnButtonClicked(val);
                    }
                    else
                    {
                        if (GridEmptyPoints.Contains(4))
                        {
                            GameController.GameC.OnButtonClicked(4);
                        }
                        else
                        {
                            if (turn == 2)
                            {
                                SetOnCornerPoint();
                            }
                            else
                            {
                                int newValue = GameController.GameC.CheckConsecutiveActive(1);

                                if (newValue != -1)
                                {
                                    if (GridEmptyPoints.Contains(newValue))
                                    {
                                        GameController.GameC.OnButtonClicked(newValue);
                                    }
                                    else
                                    {
                                        if (GridEmptyPoints.Contains(4))
                                        {
                                            GameController.GameC.OnButtonClicked(4);
                                        }
                                        else
                                        {
                                            SetOnMiddleEdgePoint();
                                        }
                                    }
                                }
                                else
                                {
                                    if (GridEmptyPoints.Contains(4))
                                    {
                                        GameController.GameC.OnButtonClicked(4);
                                    }
                                    else
                                    {
                                        SetOnMiddleEdgePoint();
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    SetOnMiddleEdgePoint();
                }
            }
        }
        else if (gameMode == GAME_MODE.IMPOSSIBLE)
        {
            if (turn == 1)
            {
                if(cornerPoints.Count < 4 || middleEdgePoints.Count < 4)
                {
                    Debug.Log("Player put it on corner point");
                    GameController.GameC.OnButtonClicked(4);
                }
                else
                {
                    SetOnCornerPoint();
                }
            }
            else if (turn == 2)
            {
                int val = GameController.GameC.CheckConsecutiveActive(2);

                if (val != -1)
                {
                    if (GridEmptyPoints.Contains(val))
                    {
                        GameController.GameC.OnButtonClicked(val);
                    }
                    else
                    {
                        if (GridEmptyPoints.Contains(4))
                        {
                            GameController.GameC.OnButtonClicked(4);
                        }
                        else
                        {
                            SetOnCornerPoint();
                        }
                    }
                }
                else
                {
                    int cornerPoint = GameController.GameC.GetPlayerCornerPoint();
                    if(cornerPoint != -1)
                    {
                        int middleEdgePoint = GameController.GameC.GetPlayerMiddleEdgePoint();
                        if(middleEdgePoint != -1)
                        {
                            if(cornerPoint == 0 && middleEdgePoint == 7)
                            {
                                GameController.GameC.OnButtonClicked(6);
                            }
                            else if (cornerPoint == 2 && middleEdgePoint == 7)
                            {
                                GameController.GameC.OnButtonClicked(8);
                            }

                            else if (cornerPoint == 0 && middleEdgePoint == 5)
                            {
                                GameController.GameC.OnButtonClicked(2);
                            }
                            else if (cornerPoint == 6 && middleEdgePoint == 5)
                            {
                                GameController.GameC.OnButtonClicked(8);
                            }

                            else if (cornerPoint == 6 && middleEdgePoint == 1)
                            {
                                GameController.GameC.OnButtonClicked(0);
                            }
                            else if (cornerPoint == 8 && middleEdgePoint == 1)
                            {
                                GameController.GameC.OnButtonClicked(2);
                            }

                            else if (cornerPoint == 2 && middleEdgePoint == 3)
                            {
                                GameController.GameC.OnButtonClicked(0);
                            }
                            else if (cornerPoint == 8 && middleEdgePoint == 3)
                            {
                                GameController.GameC.OnButtonClicked(6);
                            }
                            else
                            {
                                if (GridEmptyPoints.Contains(4))
                                {
                                    GameController.GameC.OnButtonClicked(4);
                                }
                                else
                                {
                                    Debug.Log("Set on corner point 1: " + val);
                                    SetOnCornerPoint();
                                }
                            }
                        }
                        else
                        {
                            if(cornerPoints.Count < 3)
                            {
                                if((!GridEmptyPoints.Contains(0) && !GridEmptyPoints.Contains(8)) || (!GridEmptyPoints.Contains(2) && !GridEmptyPoints.Contains(6)) )
                                {
                                    if(GridEmptyPoints.Contains(1))
                                    {
                                        GameController.GameC.OnButtonClicked(1);
                                    }
                                    else if(GridEmptyPoints.Contains(7))
                                    {
                                        GameController.GameC.OnButtonClicked(7);
                                    }
                                }
                                else
                                {
                                    SetOnMiddleEdgePoint();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (GridEmptyPoints.Contains(4))
                        {
                            GameController.GameC.OnButtonClicked(4);
                        }
                        else
                        {
                            Debug.Log("Set on corner point 3: " + val);
                            SetOnCornerPoint();
                        }
                    }
                }
            }
            else if (turn > 2)
            {
                int val = GameController.GameC.CheckConsecutiveActive(1);

                if (val != -1)
                {
                    Debug.Log("CONTAINS: " + val);

                    if (GridEmptyPoints.Contains(val))
                    {
                        GameController.GameC.OnButtonClicked(val);
                    }
                    else
                    {
                        Debug.Log("does not contain: " + val);

                        int newValue = GameController.GameC.CheckConsecutiveActive(2);

                        Debug.Log("NewValue: " + newValue);

                        if (newValue != -1)
                        {
                            Debug.Log("New Value Succeded: " + newValue);
                            if (GridEmptyPoints.Contains(newValue))
                            {
                                Debug.Log("Contains: " + newValue);
                                GameController.GameC.OnButtonClicked(newValue);
                            }
                            else
                            {
                                Debug.Log("does not contain: " + newValue);

                                if (GridEmptyPoints.Contains(4))
                                {
                                    GameController.GameC.OnButtonClicked(4);
                                }
                                else
                                {
                                    Debug.Log("Set on corner point: " + val);
                                    SetOnCornerPoint();
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    Debug.Log("DOES NOT CONTAIN: " + val);

                    int newValue = GameController.GameC.CheckConsecutiveActive(2);

                    if (newValue != -1)
                    {
                        Debug.Log("New Value Succeded: " + newValue);
                        if (GridEmptyPoints.Contains(newValue))
                        {
                            Debug.Log("Contains: " + newValue);
                            GameController.GameC.OnButtonClicked(newValue);
                        }
                        else
                        {
                            Debug.Log("does not contain: " + newValue);

                            if (GridEmptyPoints.Contains(4))
                            {
                                GameController.GameC.OnButtonClicked(4);
                            }
                            else
                            {
                                SetOnMiddleEdgePoint();
                            }
                        }
                    }
                    else
                    {
                        if (GridEmptyPoints.Contains(4))
                        {
                            GameController.GameC.OnButtonClicked(4);
                        }
                        else
                        {
                            SetOnMiddleEdgePoint();
                        }
                    }
                }
            }
        }
    }

    private void SetOnMiddleEdgePoint()
    {
        if(middleEdgePoints.Count != 0)
        {
            int rand = Random.Range(0, middleEdgePoints.Count - 1);

            int buttonNumber = middleEdgePoints[rand];

            if (GridEmptyPoints.Contains(buttonNumber))
            {
                GameController.GameC.OnButtonClicked(buttonNumber);
            }
            else
            {
                SetOnCornerPoint();
            }
        }
    }

    private void SetOnCornerPoint()
    {
        if (cornerPoints.Count != 0)
        {
            int rand = Random.Range(0, cornerPoints.Count - 1);

            int buttonNumber = cornerPoints[rand];

            if (GridEmptyPoints.Contains(buttonNumber))
            {
                GameController.GameC.OnButtonClicked(buttonNumber);
            }
            else
            {
                SetOnRandomPoint();
            }
        }
        else
        {
            SetOnRandomPoint();
        }
    }

    private void SetOnRandomPoint()
    {
        int rand = Random.Range(0, GridEmptyPoints.Count - 1);

        Debug.Log("rand: " + rand);
        int buttonNumber = GridEmptyPoints[rand];

        GameController.GameC.OnButtonClicked(buttonNumber);
    }

    public void ResetVariables()
    {

        GridEmptyPoints.Clear();
        for (var i = 0; i < GridController.GC.GridButtons.Length; i++)
        {
            GridEmptyPoints.Add(i);
        }

        cornerPoints.Clear();
        middleEdgePoints.Clear();

        cornerPoints = new List<int> { 0, 2, 6, 8 };
        middleEdgePoints = new List<int> { 1, 3, 5, 7 };

        turn = 0;
    }
}

public enum GAME_MODE
{
    EASY = 1,
    MEDIUM = 2,
    IMPOSSIBLE = 3
}
