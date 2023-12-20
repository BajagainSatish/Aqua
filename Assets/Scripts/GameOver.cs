using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject finalScoreSystem;
    [SerializeField] private GameObject p1FinalScoreObject;
    [SerializeField] private GameObject p2FinalScoreObject;

    private TextMeshProUGUI p1FinalScoreText;
    private TextMeshProUGUI p2FinalScoreText;
    private TextMeshProUGUI conclusionText;

    [SerializeField] private TextMeshProUGUI p1OriginalScoreText;//the one that was on the top right side
    [SerializeField] private TextMeshProUGUI p2OriginalScoreText;//the one that was on the top right side

    private enum GameConclusion
    {
        Player1Won, Player2Won, Draw
    }
    private GameConclusion gameConclusion;

    private void Start()
    {
        for (int i = 0; i < finalScoreSystem.transform.childCount; i++)
        {
            GameObject conclusionDisplayObject = finalScoreSystem.transform.GetChild(i).gameObject;
            if (conclusionDisplayObject.name == "ConclusionDisplay")
            {
                conclusionText = conclusionDisplayObject.GetComponent<TextMeshProUGUI>();
            }
        }
        p1FinalScoreText = p1FinalScoreObject.GetComponent<TextMeshProUGUI>();
        p2FinalScoreText = p2FinalScoreObject.GetComponent<TextMeshProUGUI>();

        finalScoreSystem.SetActive(false);
    }
    public void GameOverDueToMainBuildingDestroy(bool buildingOfP1Destroyed)
    {
        EvaluateWhichPlayerWon_MainBuildingDestroyBased(buildingOfP1Destroyed);
        PauseTheGame();
        DisplayFinalScorePage();
    }
    public void GameOverDueToTimeOver()
    {
        EvaluateWhichPlayerWonOrDraw_TimeAndScoreBased();
        PauseTheGame();
        DisplayFinalScorePage();
    } 
    private void PauseTheGame()
    {
        Time.timeScale = 0;
    }
    private void DisplayFinalScorePage()
    {
        DisplayFinalScoreCount();
        DisplayWhichPlayerWonOrDraw();
        finalScoreSystem.SetActive(true);
    }
    private void DisplayFinalScoreCount()
    {
        p1FinalScoreText.text = p1OriginalScoreText.text;
        p2FinalScoreText.text = p2OriginalScoreText.text;
    }
    private void DisplayWhichPlayerWonOrDraw()
    {
        if (gameConclusion == GameConclusion.Player1Won)
        {
            conclusionText.text = "Player 1 Wins!";
        }
        else if (gameConclusion == GameConclusion.Player2Won)
        {
            conclusionText.text = "Player 2 Wins!";
        }
        else if (gameConclusion == GameConclusion.Draw)
        {
            conclusionText.text = "Draw";
        }
        else
        {
            Debug.LogWarning("Not set the value of game conclusion!");
        }
    }
    private void EvaluateWhichPlayerWonOrDraw_TimeAndScoreBased()
    {
        int p1OriginalScore = int.Parse(p1OriginalScoreText.text);
        int p2OriginalScore = int.Parse(p2OriginalScoreText.text);

        if (p1OriginalScore > p2OriginalScore)
        {
            gameConclusion = GameConclusion.Player1Won;
        }
        else if (p1OriginalScore < p2OriginalScore)
        {
            gameConclusion = GameConclusion.Player2Won;
        }
        else
        {
            gameConclusion = GameConclusion.Draw;
        }
    }
    private void EvaluateWhichPlayerWon_MainBuildingDestroyBased(bool buildingOfP1Destroyed)
    {
        if (buildingOfP1Destroyed)
        {
            gameConclusion = GameConclusion.Player2Won;
        }
        else
        {
            gameConclusion = GameConclusion.Player1Won;
        }
    }
}
