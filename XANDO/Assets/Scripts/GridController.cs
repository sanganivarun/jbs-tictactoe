using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public static GridController GC;

    public Button[] GridButtons = null;

    public Sprite X = null;
    public Sprite O = null;
    private void OnEnable()
    {
        if(GridController.GC == null)
        {
            GridController.GC = this;
        }
        else
        {
            if(GridController.GC != this)
            {
                Destroy(GridController.GC.gameObject);
                GridController.GC = this;
            }
        }
    }
    public void OnButtonClicked(int buttonNumber, bool obj)
    {
        //GridButtons[buttonNumber].image.color = new Color(255, 255, 255, 255);

        GridButtons[buttonNumber].interactable = false;
        if (obj)
            GridButtons[buttonNumber].image.sprite = X;
        else
            GridButtons[buttonNumber].image.sprite = O;

        GridButtons[buttonNumber].transform.localScale = new Vector3(0.6f,0.6f,0.6f);

        StartCoroutine(FillImage(GridButtons[buttonNumber].image));
    }


    IEnumerator FillImage(Image img)
    {
        for (float i = 0; i <= 1; i += (Time.deltaTime*5))
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        GameController.GameC.CheckWinner();

    }


    public void ResetVariables()
    {
        for(var i = 0; i < GridButtons.Length; i++)
        {
            GridButtons[i].image.color = new Color(255, 255, 255, 0);
            GridButtons[i].transform.localScale = new Vector3(1f, 1f, 1f);
            GridButtons[i].image.sprite = null;
            GridButtons[i].interactable = true;
        }
    }

    public void SwitchOffGameButtons(bool isOn)
    {
        for(int i = 0; i <GridButtons.Length; i++)
        {
            GridButtons[i].interactable = isOn;
        }
    }

}
