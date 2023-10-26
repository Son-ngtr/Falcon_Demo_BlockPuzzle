using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Lớp cập nhật thanh điểm tối đa
public class BestScorebar : MonoBehaviour
{
    public Image fillInImage;
    public Text bestScoreText;

    private void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;
    }

    private void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    // Hàm cập nhật thanh điểm
    private void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currentPercentage = (float)currentScore / (float)bestScore;
        fillInImage.fillAmount = currentPercentage;
        bestScoreText.text = bestScore.ToString();
    }
}
