using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void add(int points)
    {
        score += points;
    }

    public void RenderScoreText()
    {
        scoreText.SetText("score: " + score);
    }
}
