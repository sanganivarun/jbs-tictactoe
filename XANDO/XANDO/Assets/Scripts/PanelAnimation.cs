using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{

    #region INSTANTIATE
    public static PanelAnimation PA = null;

    void OnEnable()
    {
        PanelAnimation.PA = this;
    }

    #endregion

    void Start()
    {
        singleButtonImage = singlePlayerButton.image;
        multiButtonImage = multiPlayerButton.image;
    }


    #region SINGLEPLAYER CLICK ANIM

    [Header("SINGLEPLAYER PANEL")]

    public Button singlePlayerButton;
    public Sprite singleButtonActive;
    public Sprite singleButtonInActive;
    public GameObject easyGO;
    public GameObject mediumGO;
    public GameObject impossibleGO;
    public GameObject singleGO;

    private Image singleButtonImage;

    public void SingleplayerAnim()
    {
        multiButtonImage.sprite = multiButtonInActive;
        multiPlayerButton.interactable = true;
        multiGO.SetActive(false);

        singleButtonImage.sprite = singleButtonActive;
        singlePlayerButton.interactable = false;
        easyGO.transform.localPosition = new Vector3(0, 120, 0);
        mediumGO.transform.localPosition = new Vector3(0, 120, 0);
        impossibleGO.transform.localPosition = new Vector3(0, 120, 0);

        singleGO.SetActive(true);

        StartCoroutine(LerpObjectY(120f, 0f, mediumGO, .3f));
        StartCoroutine(LerpObjectY(120f, -120f, impossibleGO, .3f));
    }

    #endregion

    #region MULTIPLAYER CLICK ANIM
    
    [Header("MULTIPLAYER PANEL")]
    public Button multiPlayerButton;
    public Sprite multiButtonActive;
    public Sprite multiButtonInActive;
    public GameObject localGO;
    public GameObject onlineGO;
    public GameObject friendGO;
    public GameObject multiGO;

    private Image multiButtonImage;

    public void MultiplayerAnim()
    {
        singlePlayerButton.interactable = true;
        singleButtonImage.sprite = singleButtonInActive;
        singleGO.SetActive(false);

        multiButtonImage.sprite = multiButtonActive;
        multiPlayerButton.interactable = false;

        localGO.transform.localPosition = new Vector3(0, 120, 0);
        onlineGO.transform.localPosition = new Vector3(0, 120, 0);
        friendGO.transform.localPosition = new Vector3(0, 120, 0);

        multiGO.SetActive(true);

        StartCoroutine(LerpObjectY(120f, 0f, onlineGO, .3f));
        StartCoroutine(LerpObjectY(120f, -120f, friendGO, .3f));
    }

    #endregion

    #region ON SINGLE MODE CLICK ANIM

    [Header("SINGLE MODE")]
    
    public GameObject titleAndLogo;
    public GameObject singleButtonGO;
    public GameObject multiButtonGO;


    public void StartGameAnim(bool isSingleplayer)
    {
        //title and logo
        StartCoroutine(LerpObjectY(0f, 1100f, titleAndLogo, 0.4f));
        
        //buttons
        StartCoroutine(LerpObjectX(-200f, -800f, singleButtonGO, 0.4f));
        StartCoroutine(LerpObjectX(200f, 800f, multiButtonGO, 0.4f));

        //modes
        if (isSingleplayer)
        {
            singleGO.SetActive(false);
        }
        else
        {
            multiGO.SetActive(false);
        }
    }

    #endregion

    #region MATCHMAKING ANIMATION

    private string matchMakingString = "Matchmaking..";

    [Header("MATCHMAKING ANIM")]
    public Text matchmakingLabel;
    public GameObject topPanelGO;
    public GameObject cancelGO;

    public void MatchMakingAnim()
    {
        multiPlayerButton.interactable = false;
        singlePlayerButton.interactable = false;

        matchmakingGO.SetActive(true);

        multiGO.SetActive(false);
        topPanelGO.SetActive(false);

        matchmakingLabel.text = "";
        StartCoroutine(TextAnim());

        //buttons
        StartCoroutine(LerpObjectX(-200f, -800f, singleButtonGO, 0.4f));
        StartCoroutine(LerpObjectX(200f, 800f, multiButtonGO, 0.4f));
    }

    IEnumerator TextAnim()
    {
        foreach( char c in matchMakingString)
        {
            matchmakingLabel.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        cancelGO.SetActive(true);
        PhotonManager.PM.OnPlayButtonClicked();
    }

    public void CancelMatchmakingAnim()
    {
        cancelGO.SetActive(false);
        topPanelGO.SetActive(true);

        matchmakingLabel.text = "";
        multiButtonImage.sprite = multiButtonInActive;
        multiPlayerButton.interactable = true;
        singlePlayerButton.interactable = true;

        StartCoroutine(LerpObjectX(-800f, -200f, singleButtonGO, 0.4f));
        StartCoroutine(LerpObjectX(800f, 200f, multiButtonGO, 0.4f));
    }

    #endregion

    #region ONLINE MULTI GAME START

    [Header("Online Multi")]
    public GameObject matchmakingGO;

    public void OnOnlineGameStart()
    {
        //title and logo
        StartCoroutine(LerpObjectY(0f, 1100f, titleAndLogo, 0.4f));

        InGameUIManager.IGUI.UpdateInGameTexts(4, 0, 0);
        InGameUIManager.IGUI.SetInGamePanel(true);

        matchmakingGO.SetActive(false);
        cancelGO.SetActive(false);

        MenuController.MC.ResetGame();
    }

    #endregion

    #region BACK TO HOME SCREEN ANIM

    public void BackToHomeScreen()
    {
        singlePlayerButton.interactable = true;
        multiPlayerButton.interactable = true;

        multiButtonImage.sprite = multiButtonInActive;
        singleButtonImage.sprite = singleButtonInActive;

        singleGO.SetActive(false);
        multiGO.SetActive(false);

        //title and logo
        StartCoroutine(LerpObjectY(1100f, 0f, titleAndLogo, 0.4f));

        //buttons
        StartCoroutine(LerpObjectX(-800f, -200f, singleButtonGO, 0.4f));
        StartCoroutine(LerpObjectX(800f, 200f, multiButtonGO, 0.4f));
    }

    #endregion

    #region TOAST ANIMATION

    [Header("TOAST")]
    public Image toast;
    public Text toastText;

    public void PlayToast(string textValue)
    {
        toastText.text = textValue;
        StartCoroutine(LerpToast(toast, toastText));
    }

    #endregion

    #region HOST ROOM ANIMATION

    [Header("CUSTOM ROOM")]
    public Button createButton;
    public Text createButtonText;
    public Button joinButton;


    public void HostAnimation()
    {
        StartCoroutine(LerpObjectX(-180, 0, createButton.transform.gameObject, 0.2f));
        createButtonText.text = "Host";
        joinButton.transform.gameObject.SetActive(false);
    }

    public void HostAnimationCancel()
    {
        StartCoroutine(LerpObjectX(0, -180, createButton.transform.gameObject, 0.2f));
        createButtonText.text = "Create";
        joinButton.transform.gameObject.SetActive(true);
    }


    #endregion

    #region OPEN CUSTOM ROOM 

    #endregion

    #region LERP OBJECT
    IEnumerator LerpObjectY(float startPos, float endPos, GameObject GO, float time)
    {
        float i = 0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;

            GO.transform.localPosition = new Vector3(0, Mathf.Lerp(startPos, endPos, i), 0);
            yield return null;
        }
    }

    IEnumerator LerpObjectX(float startPos, float endPos, GameObject GO, float time)
    {
        float i = 0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;

            GO.transform.localPosition = new Vector3(Mathf.Lerp(startPos, endPos, i), 0,0);
            yield return null;
        }
    }


    IEnumerator LerpToast(Image image, Text text)
    {
        for (float i = 0; i <= 1; i += (Time.deltaTime * 2))
        {
            // set color with i as alpha
            image.color = new Color(1, 1, 1, i);
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        for (float i = 1; i >= 0; i -= (Time.deltaTime * 2))
        {
            // set color with i as alpha
            image.color = new Color(1, 1, 1, i);
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }



    #endregion

}
