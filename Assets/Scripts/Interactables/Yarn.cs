using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;
    
    /**
     * A Contact represents a single node in the list of objects that
     * this Yarn object is connected to.
     */
    public struct Contact {
        // the GameObject that owns this Contact
        public GameObject source;
        
        // the angle from the previous Contact to the next
        // note that this value is null for the last Contact
        public float angle;
        
        // a list of calculated points that the yarn is actually rendered at
        public List<Vector3> renderPoints;

        public Contact(GameObject source, float angle = 0) {
            this.source = source;
            this.angle = angle;
            renderPoints = new List<Vector3>();
        }
    }
    private List<Contact> contacts;

    public enum State {
        Normal,
        Destroyed,
    }
    private State state;

    public bool IsDestroyed() { return state == State.Destroyed; }

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
            AddContact(new Contact(player));
        }
    }

    public void AddContact(Contact c) { contacts.Add(c); }
    public void RemoveContact(Contact c) { contacts.Remove(c); }

    private void PickUp() {
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