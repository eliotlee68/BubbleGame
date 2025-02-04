using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public float moveSpeed = 0.9f;
    public float deadZone = -30;

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.down * moveSpeed * Time.deltaTime);

        if (transform.position.y < deadZone)
        {
            //Debug.Log("Platform Deleted");
            Destroy(gameObject);
        }
    }

}
