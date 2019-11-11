using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
    
    private PlayerInput _playerInput;
    private List<Interactable> _interactables;

    private void Awake() {
        _playerInput = new PlayerInput();
        _playerInput.Player.Fire.started += OnInteract;
        _playerInput.Player.Enable();
        
        _interactables = new List<Interactable>();
    }
    
    public Interactable TopInteractable() { return _interactables[0]; }
    public void AddInteractable(Interactable i) { _interactables.Add(i); }
    public void RemoveInteractable(Interactable i) { _interactables.Remove(i); }

    private void OnInteract(InputAction.CallbackContext ctx) { TopInteractable().Interact(); }

}


    