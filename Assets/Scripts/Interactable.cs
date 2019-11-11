using UnityEngine;

/**
 * An Interactable GameObject can perform an action when the Player interacts with it.
 *
 * An Interactable will create its own BoxCollider according to dimensions and offset
 */
public abstract class Interactable : MonoBehaviour {
    // Sets the dimensions of the collider and offset from center of this GameObject
    private Vector3 colliderDim = new Vector3(2, 2, 2);
    private Vector3 colliderOffset = new Vector3(0, 1, 0);
    private BoxCollider _collider;

    protected virtual void Awake() {
        // place a trigger region on this Interactable
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _collider.size = colliderDim;
        _collider.center = colliderOffset;
    }

    /*
     * Performs an interaction with this Interactable.
     * Should be customized per type of Interactable.
     */
    public abstract void Interact();

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerInteract>().AddInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerInteract>().RemoveInteractable(this);
        }
    }
}
