using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;

    [SerializeField] private TextMeshProUGUI player1ScoreTMP;
    [SerializeField] private TextMeshProUGUI player2ScoreTMP;

    private void Awake()
    {
        player1Score = 0;
        player2Score = 0;
        player1ScoreTMP.text = player1Score.ToString();
        player2ScoreTMP.text = player2Score.ToString();
    }
    public int Player1Score
    {
        get { return player1Score; }
    }
    public int Player2Score
    {
        get { return player2Score; }
    }
    public void IncreasePlayer1Score(int increaseValue)
    {
        player1Score += increaseValue;
        player1ScoreTMP.text = player1Score.ToString();
    }
    public void IncreasePlayer2Score(int increaseValue)
    {
        player2Score += increaseValue;
        player2ScoreTMP.text = player2Score.ToString();
    }
}
