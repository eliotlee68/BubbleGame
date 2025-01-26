using UnityEngine;

public class BubbleSpawnerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Spawn Settings")]
    public GameObject bubblePrefab; // The platform prefab to spawn
    public float minSpawnRate = 2f;
    public float maxSpawnRate = 3.0f; 
    public float minSpeed = 4.2f; // Minimum platform speed
    public float maxSpeed = 7f; // Maximum platform speed
    public float xRange = 22.5f; // Range of X positions for spawning (left and right bounds)
    public float xOffset = -22.5f;

    [Header("Camera Bounds")]
    public float spawnHeight = -40f; // Height above the camera to spawn platforms

    private float timer = 0f;
    private float currSpawnRate;

    private void Start()
    {
        currSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
    }
    void Update()
    {
        // Timer to control spawning
        timer += Time.deltaTime;
        if (timer >= currSpawnRate)
        {
            SpawnPlatform();
            timer = 0f;
            currSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    void SpawnPlatform()
    {
        // Calculate a random position within the X range
        float randomX = Random.Range(-xRange + xOffset, xRange + xOffset);
        Vector3 spawnPosition = new Vector3(randomX, Camera.main.transform.position.y + spawnHeight, 0f);

        // Instantiate the platform
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Set random speed and length
        BubbleScript bubbleScript = bubble.GetComponent<BubbleScript>();
        if (bubbleScript != null)
        {
            bubbleScript.riseSpeed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
