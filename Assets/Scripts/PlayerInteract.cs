using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Interactable interactable;

    void Update()
    {
        if (InputManager.Interact)
        {
            interactable.activate();
        }

    }

}


    