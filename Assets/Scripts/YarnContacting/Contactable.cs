using UnityEngine;

/**
 * A GameObject with this class has the ability to become a Contact in a line of yarn.
 *
 * Disabling this component will prevent it from becoming a Contact.
 *
 * A Contactable is responsible for containing the sort of information that a Contact would
 * need if this GameObject became a Contact.
 */
public abstract class Contactable : MonoBehaviour {
    /**
     * Returns a new Contact object that represents this Contactable.
     */
    public Contact GetContact() {
        return new Contact();
    }
}

/**
 * If a Yarn object wrapping around this Contactable should send a Contact to that Yarn,
 * put this Behaviour on that GameObject.
 */
public class WrapContactable : Contactable {
    
}

/**
 * If the player interacting with this Contactable should send a Contact to the player,
 * put this Behaviour on that GameObject.
 */
public class InteractContactable : Contactable {
    
}