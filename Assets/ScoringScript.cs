using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ScoringScript : MonoBehaviour
{
    public static ScoringScript Instance; // Singleton pattern for easy access

    public TMP_Text scoreText; // Reference to the TextMeshPro UI element
    private int score = 0; // Current score

    private float timer = 0f; // Timer to track seconds

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Increase the score every second
        if (timer >= 1f)
        {
            AddScore(1);
            timer = 0f; // Reset the timer
        }
    }

    public void AddScore(int amount)
    {
        score += amount; // Increase the score
        UpdateScoreText(); // Update the UI
    }

    public float GetScore()
    {
        return score;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
