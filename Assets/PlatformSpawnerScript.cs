using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Spawn Settings")]
    public GameObject platformPrefab; // The platform prefab to spawn
    public float maxSpawnRate = 3.5f; // How often platforms spawn (in seconds)
    public float minSpawnRate = 1.0f;
    public float minSpeed = 0.5f; // Minimum platform speed
    public float maxSpeed = 2f; // Maximum platform speed
    public float minLength = 1f; // Minimum platform length
    public float maxLength = 4f; // Maximum platform length
    public float xRange = 50f; // Range of X positions for spawning (left and right bounds)
    public float xOffset = 22.5f;

    [Header("Camera Bounds")]
    public float spawnHeight = 40f; // Height above the camera to spawn platforms

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
        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Set random speed and length
        PlatformScript platformScript = platform.GetComponent<PlatformScript>();
        if (platformScript != null)
        {
            platformScript.moveSpeed = Random.Range(minSpeed, maxSpeed);

            // Adjust platform length
            Vector3 scale = platform.transform.localScale;
            scale.x = Random.Range(minLength, maxLength);
            platform.transform.localScale = scale;
        }
    }
}
