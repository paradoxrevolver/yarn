using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;

    private LevelManager levelManager;
    
    public enum State {
        Normal,
        Destroyed,
    }
    private State state;

    public bool IsDestroyed() { return state == State.Destroyed; }
    
    /**
     * A Contact represents a single node in the list of objects that
     * this Yarn object is connected to.
     */
    private List<Contact> contacts;
    
    public void AddContact(GameObject obj) {
        contacts.Add(new Contact(this, obj, levelManager, 0));
    }

    public void InsertContact(GameObject obj, int index, float initialAngle) {
        contacts.Insert(index, new Contact(this, obj, levelManager, initialAngle));
    }
    
    /* Remove all Contacts with obj as its source. */
    public void RemoveContactAll(GameObject obj) {
        foreach(Contact c in contacts.FindAll(c => c.source == obj))
            contacts.Remove(c);
    }
    
    public void RemoveContact(Contact c) { contacts.Remove(c); }
    
    /*
     * Remove a Contacts with obj as its source, but only at the beginning or end of the contacts list.
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
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate() { UpdateAllContacts(); }

    private void UpdateAllContacts() {
        // first, update the render points for all contacts.
        foreach(Contact c in contacts)
            c.UpdateRenderPoints();
        
        // next, update the angles that all contacts have to their next contact.
        for(var i = 0; i < contacts.Count - 1; i++)
            contacts[i].UpdateLine(contacts[i + 1]);
        
        // update the potential contacts list, adding new proceeding contacts if necessary.
        for(var i = 0; i < contacts.Count - 1; i++)
            contacts[i].UpdatePotentialContacts(contacts[i + 1], i);

        // Delete unravelled contacts
        for (var i = 1; i < contacts.Count - 1; i++)
            contacts[i].updateUnravelled(contacts[i - 1]);
    }


    public override void Interact() {
        // the player picks up the yarn if they have their arms free
        if (playerManager.CheckState(PlayerManager.State.Normal)) {
            PickUp();
        }
        else if(playerManager.CheckState(PlayerManager.State.Pulling))
        {
            PutDown();
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