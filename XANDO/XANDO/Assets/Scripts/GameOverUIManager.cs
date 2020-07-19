using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager GOUI;

    void Start()
    {
        GameOverUIManager.GOUI = this;
    }



    [Header("Sprites")]
    public Sprite XWins;
    public Sprite Owins;
    public Sprite XODraw;
    public Image GameOverImage;


    [Header("Panels")]
    public GameObject GameOverPanel;

    [Header("Buttons")]
    public Button retryButton;



    public void OnGameOverUI()
    {
        GameController.GameC.SetCursorNull();
        GameOverPanel.SetActive(true);
        retryButton.interactable = true;
    }

    public void SetWinner(WINNER winner)
    {
        GridController.GC.SwitchOffGameButtons(false);

        switch (winner)
        {
            case WINNER.XWINS:
                GameOverImage.sprite = XWins;
                GameOverImage.transform.localScale = new Vector3(1f, 1f, 1f);
                InGameUIManager.IGUI.UpdateLabelOnGameOver(false);
                break;
            case WINNER.OWINS:
                GameOverImage.sprite = Owins;
                GameOverImage.transform.localScale = new Vector3(1f, 1f, 1f);
                InGameUIManager.IGUI.UpdateLabelOnGameOver(true);
                break;
            case WINNER.DRAW:
                GameOverImage.sprite = XODraw;
                GameOverImage.transform.localScale = new Vector3(1f, 1f, 1f);
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

        OnGameOverUI();
    }




}
