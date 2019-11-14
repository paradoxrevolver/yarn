using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
    
    private PlayerInput playerInput;
    private List<Interactable> interactables;

    private void Awake() {
        playerInput = new PlayerInput();
        playerInput.Player.Fire.started += OnInteract;
        playerInput.Player.Enable();
        
        interactables = new List<Interactable>();
    }
    
    public Interactable TopInteractable() { return interactables[0]; }
    public void AddInteractable(Interactable i) { interactables.Add(i); }
    public void RemoveInteractable(Interactable i) { interactables.Remove(i); }

    private void OnInteract(InputAction.CallbackContext ctx) {
        if(interactables.Count > 0)
            TopInteractable().Interact();
    }

}


    