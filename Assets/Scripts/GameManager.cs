using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/**
 * The GameManager holds any data or functions that need to persist across scene loads.
 */
public class GameManager : MonoBehaviour {
    private PlayerInput playerInput;
    
    private static GameManager instance;
    
    private void Awake()
    {
        // Ensures that the GameObject with this script is a Singleton:
        // a duplicate of this script will always be deleted
        // and this GameObject will not be destroyed on scene change.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        playerInput = new PlayerInput();
        playerInput.Player.Restart.started += OnRestart;
        playerInput.Player.Enable();
    }
    
    private void OnDestroy() {
        if (playerInput != null) {
            playerInput.Player.Restart.started -= OnRestart;
            playerInput.Player.Disable();
        }
    }

    /**
     * Restarts the currently active scene.
     */
    private void OnRestart(InputAction.CallbackContext ctx) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
