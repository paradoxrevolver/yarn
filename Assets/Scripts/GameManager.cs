﻿using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The GameManager holds any data or functions that need to persist across scene loads.
 */
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    private void Awake()
    {
        // Ensures that the GameObject with this script is a Singleton:
        // a duplicate of this script will always be deleted
        // and this GameObject will not be destroyed on scene change.
        _instance = this;
        if (_instance != null && _instance != this)
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
