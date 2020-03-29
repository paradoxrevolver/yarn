using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenuPointer;
    //PlayerInput - should be moved to PlayerInteract if possible at a later time
    private PlayerInput playerInput;
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Pause.performed += OnPause;
        //Debug.Log("Debug Awake");
    }
    private void OnPause(InputAction.CallbackContext ctx)
    {
        TogglePause();
        //Debug.Log("Pause Attempted");
    }

    private void OnEnable() { playerInput.Player.Pause.Enable(); }
    private void OnDisable() { playerInput.Player.Pause.Disable(); }



    // Pause toggling, depending on if component itself is active
    public void TogglePause()
    {
        if(PauseMenuPointer != null) PauseMenuPointer.SetActive(!PauseMenuPointer.activeSelf);
        else {
            Debug.LogWarning("PauseManager tried to toggle PauseMenu but didn't have reference to a PauseMenu.");
        }
    }

    public void ResumeButton()
    {
        //Debug.Log("Resume Attempted");
        TogglePause();
    }

    public void MenuButton() { SceneManager.LoadScene("Main Menu"); }

    public void QuitButton() { Application.Quit(); }
}

