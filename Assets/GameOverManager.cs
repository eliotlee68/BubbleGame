using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text finalScoreText; // Reference to the final score text

    void Start()
    {
        // Display the final score
        if (finalScoreText != null && ScoringScript.Instance != null)
        {
            finalScoreText.text = "Final Score: " + ScoringScript.Instance.GetScore().ToString();
        }
    }

    // Called when the "Restart" button is clicked
    public void RestartGame()
    {
        ScoringScript.Instance.ResetScore();
        SceneManager.LoadScene(1);
    }

    // Called when the "Main Menu" button is clicked
    public void GoToMainMenu()
    {
        // Destroy the ScoreManager to avoid duplicates
        if (ScoringScript.Instance != null)
        {
            Destroy(ScoringScript.Instance.gameObject);
        }
        SceneManager.LoadScene(0);
    }
}
