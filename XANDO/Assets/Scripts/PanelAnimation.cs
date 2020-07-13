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
    //public GameObject friendGO;
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
        //friendGO.transform.localPosition = new Vector3(0, 120, 0);

        multiGO.SetActive(true);

        StartCoroutine(LerpObjectY(120f, 0f, onlineGO, .3f));
        //StartCoroutine(LerpObjectY(120f, 0f, friendGO, .3f));
    }

    #endregion

    #region ON SINGLE MODE CLICK ANIM

    [Header("SINGLE MODE")]
    
    public GameObject titleAndLogo;
    public GameObject singleButtonGO;
    public GameObject multiButtonGO;


    public void SingleModeAnim()
    {
        //title and logo
        StartCoroutine(LerpObjectY(0f, 1100f, titleAndLogo, 0.4f));
        
        //buttons
        StartCoroutine(LerpObjectX(-200f, -800f, singleButtonGO, 0.4f));
        StartCoroutine(LerpObjectX(200f, 800f, multiButtonGO, 0.4f));

        //modes
        singleGO.SetActive(false);
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

        multiGO.SetActive(false);
        topPanelGO.SetActive(false);

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

    #endregion

}
