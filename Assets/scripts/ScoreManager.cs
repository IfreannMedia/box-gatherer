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

    public void EnlargeScoreText(float delay)
    {
        StartCoroutine(ElargeText(delay));
    }

    private IEnumerator ElargeText(float delay)
    {
        RectTransform score = scoreText.GetComponent<RectTransform>();
        yield return new WaitForSeconds(delay);
        float timeToAdjust = 1f;
        float timer = 0.0f;
        Vector3 targetScale = new Vector3(6.75f, 6.75f, 6.75f);
        while (timer <= timeToAdjust)
        {
            score.localScale = Vector3.Lerp(score.localScale, targetScale, timer);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
