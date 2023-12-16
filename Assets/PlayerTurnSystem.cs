using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurnSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTurnText;
    public enum PlayerTurn
    {
        Player1Turn, Player2Turn, None
    }
    //Player 1 Turn, Player 2 Turn: Strategy Time
    //None: Common Game Time
    public PlayerTurn playerTurn;

    private void Start()
    {
        playerTurn = PlayerTurn.Player1Turn;
        EvaluateDisplayText();
    }
    public void SetTurnToPlayer1()
    {
        playerTurn = PlayerTurn.Player1Turn;
        EvaluateDisplayText();
    }
    public void SetTurnToPlayer2()
    {
        playerTurn = PlayerTurn.Player2Turn;
        EvaluateDisplayText();
    }
    public void SwitchPlayer()
    {
        if (playerTurn == PlayerTurn.Player1Turn)
        {
            playerTurn = PlayerTurn.Player2Turn;
        }
        else if (playerTurn == PlayerTurn.Player2Turn)
        {
            playerTurn = PlayerTurn.Player1Turn;
        }
        EvaluateDisplayText();
    }
    public void SetPlayerTurnToNone()
    {
        playerTurn = PlayerTurn.None;
        EvaluateDisplayText();
    }

    private void EvaluateDisplayText()
    {
        string displayText;
        if (playerTurn == PlayerTurn.Player1Turn)
        {
            displayText = "Player 1's Turn";
        }
        else if (playerTurn == PlayerTurn.Player2Turn)
        {
            displayText = "Player 2's Turn";
        }
        else//None
        {
            displayText = "Game Time";
        }
        playerTurnText.text = displayText.ToString();
    }
}
