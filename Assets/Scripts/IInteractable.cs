using System;
using UnityEngine;

/**
 * An Interactable GameObject can perform an action when the Player interacts with it.
 * A BoxCollider acts as the trigger for interacting with an Interactable.
 */
[RequireComponent(typeof(BoxCollider))]
public abstract class IInteractable : MonoBehaviour {
    /* Performs an interaction with this Interactable. */
    public abstract void Interact();
}
