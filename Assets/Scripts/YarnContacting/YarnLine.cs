using System;
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

    private Yarn yarn;

    public List<Contact> contacts { get; private set; }
    public List<Vector3> renderPoints { get; private set; }

    /**
     * A YarnLine can only be created by a Yarn that has been started between the Player and a Pushpin
     */
    public YarnLine(Yarn yarn, PointContact player, PointContact pushpin) {
        this.yarn = yarn;
        head = pushpin;
        tail = player;
        wraps = new List<Contact>();
        OnLineChanged();
    }

    /**
     * Updates the line of yarn physically, considering new contact points and removing invalid ones
     */
    public void PhysicsUpdate() {
        // run through all Contacts:
        // if a Contact's Contactable sees an overlap with other Contactables, add them
        // if a Contact has become invalid, remove it
        for (var i = 0; i < contacts.Count; i++) {
            var contact = contacts[i];
            // todo: check whatever needs to be checked to add or remove contacts from this YarnLine
        }
    }

    /**
     * Updates the line of yarn visually, after physics calculation have been performed
     */
    public void Draw() {
        
    }

    /* Fires whenever the structure of the line changes. */
    private void OnLineChanged() {
        // update the contacts list
        contacts = new List<Contact>(wraps.Count + 2) {head};
        contacts.AddRange(wraps);
        contacts.Add(tail);

        // update the renderpoints based on changed contacts
        renderPoints = new List<Vector3>();
        foreach (var contact in contacts) {
            if(contact != null) renderPoints.AddRange(contact.renderPoints);
        }
    }

    private void Add(Contact contact) {
        wraps.Add(contact);
        OnLineChanged();
    }

    private int RemoveAll(Contact contact) { return RemoveAll(contact.host.gameObject); }
    private int RemoveAll(GameObject gameObject) {
        int result = wraps.RemoveAll(wrap => wrap.host.gameObject.Equals(gameObject));
        OnLineChanged();
        return result;
    }

    public Contactable GetHead() { return head.host; }
    public Contactable GetTail() { return tail.host; }
    
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
    private void ReverseList() {
        // swap head and tail
        var temp = tail;
        tail = head;
        head = temp;
        // reverse all wrapping points
        wraps.Reverse();
        // todo: reverse all prev and next Contact references
    }

    /**
     * Clears the entire line, removing everything.
     */
    public void Clear() {
        head = null;
        tail = null;
        wraps.Clear();
        OnLineChanged();
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