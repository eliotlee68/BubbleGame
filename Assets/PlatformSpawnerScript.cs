using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Spawn Settings")]
    public GameObject platformPrefab; // The platform prefab to spawn
    public float spawnRate = 2f; // How often platforms spawn (in seconds)
    public float minSpeed = 0.5f; // Minimum platform speed
    public float maxSpeed = 2f; // Maximum platform speed
    public float minLength = 1f; // Minimum platform length
    public float maxLength = 4f; // Maximum platform length
    public float xRange = 50f; // Range of X positions for spawning (left and right bounds)

    [Header("Camera Bounds")]
    public float spawnHeight = 40f; // Height above the camera to spawn platforms

    private float timer = 0f;

    void Update()
    {
        // Timer to control spawning
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnPlatform();
            timer = 0f;
        }
    }

    void SpawnPlatform()
    {
        // Calculate a random position within the X range
        float randomX = Random.Range(-xRange, xRange);
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
