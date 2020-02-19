﻿using UnityEngine;

/**
 * An Interactable GameObject can perform an action when the Player interacts with it.
 *
 * An Interactable will create its own BoxCollider according to dimensions and offset
 */
public abstract class Interactable : MonoBehaviour {
    protected GameObject player;
    protected PlayerManager playerManager;
    protected PlayerInteract playerInteract;
    
    // Sets the dimensions of the collider and offset from center of this GameObject
    private Vector3 colliderDim = new Vector3(2, 2, 2);
    private Vector3 colliderOffset = new Vector3(0, 1, 0);
    private BoxCollider boxCollider;

    protected virtual void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        playerInteract = player.GetComponent<PlayerInteract>();
        
        // place a trigger region on this Interactable
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = colliderDim;
        boxCollider.center = colliderOffset;
    }

    /*
     * Performs an interaction with this Interactable.
     * Should be customized per type of Interactable.
     */
    public abstract void Interact();

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerInteract>().AddInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerInteract>().RemoveInteractable(this);
        }
    }
}
