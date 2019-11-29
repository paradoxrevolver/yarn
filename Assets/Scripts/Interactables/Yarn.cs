using System;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;

    public enum State {
        Normal,
        Destroyed,
    }
    private State state;

    public bool IsDestroyed() { return state == State.Destroyed; }

    /**
     * PotentialContacts are all objects which could potentially become
     * a Contact. Contacts maintain a list of these and raises an alarm
     * if they 
     */
    public struct PotentialContact {
        // the GameObject that could become a potential Contact
        public GameObject source;
        
        // the angle this PotentialContact has with the owning Contact
        public float parentAngle;
    }
    
    /**
     * A Contact represents a single node in the list of objects that
     * this Yarn object is connected to.
     */
    public class Contact {
        // the GameObject that owns this Contact
        public GameObject source;
        
        // the angle from this Contact to the next Contact
        // calculated on the XZ plane relative to Vector3.right
        // note that this value is null for the last contact in a list
        public float angle;
        
        // a list of calculated points that the yarn is actually rendered at
        public List<Vector3> renderPoints;

        // a list of potential contacts from the previous frame, for anticipating new contacts
        public List<PotentialContact> oldPotential;

        public Contact(GameObject source, float angle = 0) {
            this.source = source;
            this.angle = angle;
            renderPoints = new List<Vector3>();
            oldPotential = new List<PotentialContact>();
        }
    }
    private List<Contact> contacts;

    public void AddContact(GameObject obj) {
        contacts.Add(new Contact(obj));
    }
    
    /* Remove all Contacts with obj as its source. */
    public void RemoveContactAll(GameObject obj) {
        foreach(Contact c in contacts.FindAll(c => c.source == obj))
            contacts.Remove(c);
    }
    
    /*
     * Remove a Contacts with obj as its source only at the beginning or end of the contacts list.
     *
     * Additionally causes the contacts list to reverse order if trying to remove the contact from the front.
     * This is done in the event that the player tries to remove the yarn from the opposite end, so that
     * the yarn's contacts are always added from the end of the list.
     */
    public void RemoveContactFromEnd(GameObject obj) {
        if(contacts[0].source == obj) contacts.Reverse();
        if(contacts[contacts.Count - 1].source == obj) contacts.RemoveAt(contacts.Count - 1);
    }

    /* Does the contacts list contain any Contact with obj as a source? */
    public bool ContainsContact(GameObject obj) {
        return contacts.Exists(c => c.source == obj);
    }

    /* Is obj a Contact at the beginning or end of the contacts list? */
    public bool IsContactAtEnd(GameObject obj) {
        return contacts[0].source == obj || contacts[contacts.Count - 1].source == obj;
    }
    
    public int ContactCount() { return contacts.Count; }

    public GameObject mesh;

    protected override void Awake() {
        base.Awake();
        state = State.Normal;
        contacts = new List<Contact>();
    }

    private void FixedUpdate() {
        UpdateAllRenderPoints();
    }

    /**
     * Runs through every contact and makes sure that all renderPoints
     * lists are valid.
     */
    private void UpdateAllRenderPoints() {
        foreach(Contact c in contacts)
            UpdateRenderPoints(c);
    }

    /**
     * Checks a single Contact to make sure that its renderPoints
     * list is valid.
     */
    private void UpdateRenderPoints(Contact c) {
        c.renderPoints.Clear();
        c.renderPoints.Add(c.source.transform.position);
    }

    public override void Interact() {
        // the player picks up the yarn if they have their arms free
        if (playerManager.CheckState(PlayerManager.State.Normal)) {
            PickUp();
        }
    }

    private void HideMesh() { mesh.SetActive(false); }

    private void ShowMesh() { mesh.SetActive(true); }

    private void PickUp() {
        AddContact(player.gameObject);
        playerManager.SetState(PlayerManager.State.Holding);
        playerManager.YarnHeld = this;
        transform.SetParent(player.transform);
        transform.localPosition = positionYarnInPlayersArms;
        print("The player just picked up some yarn.");
    }

    private void PutDown() {
        playerManager.SetState(PlayerManager.State.Normal);
        playerManager.YarnHeld = null;
        transform.parent = null;
        transform.localPosition = player.transform.localPosition;
        print("The player has dropped the yarn.");
    }

    /* Untie this Yarn object from the given Pushpin, returning control to the player. */
    public void UntieFrom(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Pulling);
        RemoveContactFromEnd(pushpin.gameObject);
        AddContact(player.gameObject);
        if(mesh) ShowMesh();
    }
    
    /* Ties this Yarn off on the given Pushpin, removing control from the player. */
    public void TieOff(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Normal);
        RemoveContactFromEnd(player);
        AddContact(pushpin.gameObject);
        if(mesh) HideMesh();
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