using UnityEngine;

/**
 * If the player interacting with this Contactable should send a Contact to the player,
 * put this Behaviour on that GameObject.
 */
public class InteractContactable : Contactable {
    [Tooltip("This is the offset from the GameObject's position where yarn will be attached.")]
    public Vector3 pointOffset;
    
    public PointContact GetNewContact() {
        return new PointContact(this);
    }
}
