using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    [SerializeField] private int player1Point;
    [SerializeField] private int player2Point;

    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;

    private void Awake()
    {
        player1Point = 1000;
        player2Point = 1000;
        player1Score.text = player1Point.ToString();
        player2Score.text = player2Point.ToString();
    }
    public int Player1Point
    {
        get { return player1Point; }
    }
    public int Player2Point
    {
        get { return player2Point; }
    }
    public void ReducePlayer1Point(int reduceValue)
    {
        player1Point -= reduceValue;
        player1Score.text = player1Point.ToString();
    }
    public void ReducePlayer2Point(int reduceValue)
    {
        player2Point -= reduceValue;
        player2Score.text = player2Point.ToString();
    }
}
