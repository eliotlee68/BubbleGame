using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManagerScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
