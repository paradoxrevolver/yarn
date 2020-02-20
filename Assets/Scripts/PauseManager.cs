using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //PlayerInput - should be moved to PlayerInteract if possible at a later time
    private PlayerInput playerInput;
    private GameObject PauseMenuPointer;
    private void Awake()
    {
        playerInput = new PlayerInput();
        PauseMenuPointer = gameObject.transform.Find("PauseMenu").gameObject;
        playerInput.Player.Pause.started += OnPause;
        playerInput.Player.Pause.performed += OnPause;
        playerInput.Player.Pause.canceled += OnPause;
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
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void resumeButton()
    {
        //Debug.Log("Resume Attempted");
        TogglePause();
    }


}

