using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    private void Awake()
    {
        // Ensures that the GameObject with this script is a Singleton:
        // a duplicate of this script will always be deleted
        // and this GameObject will not be destroyed on scene change.
        instance = this;
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    /**
     * Restarts the currently active scene.
     */
    private void OnRestart() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}
