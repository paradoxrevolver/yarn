using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Interactable TopInteractable() { return interactables[0]; }
    private List<Interactable> interactables;


    void Update()
    {
        
        
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        interactable.Interact();
    }

    public void AddInteractable(Interactable i)
    {
        interactables.Add(i);
    }

    public void RemoveInteractable(Interactable i)
    {
        interactables.Remove(i);
    }

}


    