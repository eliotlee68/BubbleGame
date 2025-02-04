using TMPro;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("STARTED");
        // Update the ScoreManager's scoreText reference
        if (ScoringScript.Instance != null && scoreText != null)
        {
            ScoringScript.Instance.SetScoreText(scoreText);
            Debug.Log("Score text reference updated in GameScene.");
        }
        else
        {
            Debug.LogError("ScoreManager instance or score text is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
