using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTurnText;
    public enum PlayerTurn
    {
        Player1Turn, Player2Turn,GameTime, None
    }
    //Player 1 Turn, Player 2 Turn: Strategy Time
    //None: Common Game Time
    public PlayerTurn playerTurn;

    [SerializeField] private CameraControlRuntime cameraControlRuntime;
    private Image backgroundImage;

    private string p1BackgroundColor;
    private string p1TextColor;
    private string p2BackgroundColor;
    private string p2TextColor;
    private string gameBackgroundColor;
    private string gameTextColor;

    private float backgroundOpacity;
    private float textOpacity;

    private void Awake()
    {
        p1BackgroundColor = SetParameters.Player1TurnBackgroundColor;
        p2BackgroundColor = SetParameters.Player2TurnBackgroundColor;
        gameBackgroundColor = SetParameters.GameTimeBackgroundColor;

        p1TextColor = SetParameters.Player1TurnTextColor;
        p2TextColor = SetParameters.Player2TurnTextColor;
        gameTextColor = SetParameters.GameTimeTextColor;

        backgroundOpacity = SetParameters.BackgroundColorAlphaValue;
        textOpacity = SetParameters.TextColorAlphaValue;
    }

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        gameObject.SetActive(false);
        playerTurn = PlayerTurn.None;
    }
    public void SetTurnToPlayer1()
    {
        playerTurn = PlayerTurn.Player1Turn;
    }
    public void SetTurnToPlayer2()
    {
        playerTurn = PlayerTurn.Player2Turn;
    }
    public void SetTurnToGameTime()
    {
        playerTurn = PlayerTurn.GameTime;
    }
    public void SetPlayerTurnToNone()
    {
        playerTurn = PlayerTurn.None;
        EvaluateDisplayText();
    }

    private void EvaluateDisplayText()
    {
        string displayText;
        if (cameraControlRuntime.firstCameraSwitch)
        {
            displayText = "Player 1's Turn";
            gameObject.SetActive(true);
            StartCoroutine(TimeForTextDisplayOnScreen());
        }
        else if (cameraControlRuntime.secondCameraSwitch)
        {
            displayText = "Player 2's Turn";
            gameObject.SetActive(true);
            StartCoroutine(TimeForTextDisplayOnScreen());
        }
        else if (cameraControlRuntime.thirdCameraSwitch)
        {
            displayText = "GameTime";
            gameObject.SetActive(true);
            StartCoroutine(TimeForTextDisplayOnScreen());
        }
        else
        {
            displayText = "None";
        }
        EvaluateDisplayTextColor(displayText);
        playerTurnText.text = displayText.ToString();             
    }
    private IEnumerator TimeForTextDisplayOnScreen()
    {
        yield return new WaitForSeconds(SetParameters.CameraSwitchDuration);
        gameObject.SetActive(false);
    }
    private void EvaluateDisplayTextColor(string displayText)
    {
        if (displayText == "Player 1's Turn")
        {
            SetColor(p1BackgroundColor, p1TextColor);
        }
        else if (displayText == "Player 2's Turn")
        {
            SetColor(p2BackgroundColor, p2TextColor);
        }
        else
        {
            SetColor(gameBackgroundColor, gameTextColor);
        }
    }
    private Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
    private void SetColor(string backgColor, string textColor)
    {
        Color backgroundColor = HexToColor(backgColor);
        Color displayTextColor = HexToColor(textColor);

        backgroundColor.a = backgroundOpacity;
        displayTextColor.a = textOpacity;

        backgroundImage.color = backgroundColor;
        playerTurnText.color = displayTextColor;
    }
}
