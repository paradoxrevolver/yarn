using System.Collections.Generic;
using UnityEngine;

/**
 * A YarnLine is a list that represents all the connections of a line of yarn.
 */
public class YarnLine {
    // the head and tail contacts are always PointContacts
    private PointContact head, tail;
    // every other contact in the list is some sort of Contact
    private List<Contact> wraps;

    private Yarn host;
    
    public List<Contact> contacts { get; private set; }
    public List<Vector3> renderPoints { get; private set; }

    /**
     * A YarnLine can only be created by a Yarn that has been started between the Player and a Pushpin
     */
    public YarnLine(Yarn host, PointContact player, PointContact pushpin) {
        this.host = host;
        head = player;
        tail = pushpin;
        wraps = new List<Contact>();
    }

    public void Update() {
        
    }
    
    /* Fires whenever the structure of the line changes. */
    public void OnLineChanged() {
        // update the contacts list
        List<Contact> newContacts = new List<Contact>(wraps.Count + 2){tail};
        newContacts.AddRange(wraps);
        newContacts.Add(head);
        contacts = newContacts;
        
        // update the renderpoints based on changes contacts
        
    }

    public void Add(Contact contact) {
        wraps.Add(contact);
        OnLineChanged();
    }

    public int RemoveAll(Contact contact) {
        int result = wraps.RemoveAll(wrap => wrap.host.gameObject.Equals(contact.host.gameObject));
        OnLineChanged();
        return result;
    }
    
    /**
     * If you try to give this function a Player's Contact, it will flip the entire list
     * to ensure the Player is at the tail.
     */
    public void SetHead(PointContact contact) {
        head = contact;
        if (contact.host.CompareTag("Player")) ReverseList();
        OnLineChanged();
    }
    
    public void SetTail(PointContact contact) {
        tail = contact;
        OnLineChanged();
    }

    /* Reverses the entire list, including the head and tail contacts. */
    public void ReverseList() {
        // swap head and tail
        var temp = tail;
        tail = head;
        head = temp;
        // reverse all wrapping points
        wraps.Reverse();
        // reverse all prev and next Contact references
        
    }
    
    private void OnDrawGizmos() {
        // render gizmos showing the render point connections
        if (contacts != null)
            for (var i = 0; i < contacts.Count; i++) {
                // draw lines between all of the render points for this contact
                var thisContact = contacts[i];
                if (thisContact.renderPoints.Count < 1) continue;
                
                for (var j = 0; j < thisContact.renderPoints.Count - 1; j++) {
                    var thisPoint = thisContact.renderPoints[j];
                    var nextPoint = thisContact.renderPoints[j+1];
                    Gizmos.DrawLine(thisPoint, nextPoint);
                }
                // if we know there's another contact after this one, draw a line
                // from the last renderPoint on this contact to the first renderPoint
                // on the next contact.
                if (i >= contacts.Count - 1) continue;

                var nextContact = contacts[i + 1];
                if (nextContact.renderPoints.Count < 1) continue;
                var thisContactLastPointIndex = thisContact.renderPoints.Count - 1;
                    
                var thisContactLastPoint = thisContact.renderPoints[thisContactLastPointIndex];
                var nextContactFirstPoint = nextContact.renderPoints[0];
                Gizmos.DrawLine(thisContactLastPoint, nextContactFirstPoint);
            }
    }
}