using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/**
 * A GameObject with this class has the ability to become a Contact in a line of yarn.
 *
 * Disabling this component will prevent it from becoming a Contact.
 *
 * A Contactable is responsible for containing the sort of information that a Contact would
 * need if this GameObject became a Contact.
 */
public abstract class Contactable : MonoBehaviour {
    [HideInInspector] public Vector3 pastPosition;
    [HideInInspector] public Vector3 currentPosition;

    [HideInInspector] public List<WrapContactable> wrapContactables;
    
    protected virtual void Awake() {
        var position = transform.position;
        pastPosition = position;
        currentPosition = position;

        wrapContactables = FindObjectsOfType<WrapContactable>().ToList();
    }

    public void UpdatePosition() {
        // update past and current positions
        pastPosition = currentPosition;
        currentPosition = transform.position;
    }

    /**
     * Return new Contacts for every wrap around a Contactable that is being made from this Contact to the other Contact.
     */
    public List<Contact> CheckOverlaps(Contact other) {
        // create an edited watch list
        var watchedContactablesEdit = new List<WrapContactable>(wrapContactables);
        // if the other Contactable is a WrapContactable, remove it
        if(other.host.GetType() == typeof(WrapContactable))
            watchedContactablesEdit.Remove((WrapContactable)other.host);
        
        List<Contact> newContacts = new List<Contact>();
        
        // loop through each WrapContactable and consider whether not a line has crossed it
        foreach (var watchedContactable in wrapContactables) {
            /*if (WrapContactable.CheckWrapAround(, watchedContactable, other)) {
                
            }*/
        }

        return newContacts;
    }
}