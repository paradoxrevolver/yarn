using System.Collections.Generic;
using UnityEngine;

public class Contact {
    // this Contact is in this Yarn's contacts list
    public Yarn yarn;
    
    // this Contact's GameObject
    public GameObject source;
    
    private LevelManager levelManager;
    
    /*
     * the line that this Contact is responsible for checking.
     * leaves from this Contact's last renderPoint and goes to the first
     * renderPoint in the next Contact.
     *
     * note that this value may be null if this Contact has no next Contact.
     */
    public Vector3 yarnLine;
    public Vector2 yarnLineXZ;
    
    // a list of calculated points that the yarn is actually rendered at
    public List<Vector3> renderPoints;

    public Contact(Yarn yarn, GameObject source, LevelManager levelManager) {
        this.yarn = yarn;
        this.source = source;
        this.levelManager = levelManager;
        renderPoints = new List<Vector3>();
    }

    /* Update the render points for this Contact. */
    public void UpdateRenderPoints() {
        // todo: show render points properly by using values from Contactable class
        renderPoints.Clear();
        renderPoints.Add(source.transform.position);
    }

    /* Update the line that this Contact has to the nextContact following it. */
    public void UpdateLine(Contact nextContact) {
        // next contact must exist to find values for it!
        if (nextContact == null) return;
        
        // update the values this Contact has with the next Contact.
        // use render points to determine this.
        yarnLine = nextContact.renderPoints[0] - renderPoints[renderPoints.Count - 1];
        yarnLineXZ = new Vector2(yarnLine.x, yarnLine.z);
    }
    
    /*
     * Updates the potential contacts list for this Contact.
     *
     * If a potential contact's angle and radius changes in a way that suggests it
     * crossed over the line this Contact owns, this Contact will be responsible for
     * adding it as a new Contact to the yarn line.
     */
    public void UpdatePotentialContacts(Contact nextContact, int index) {
        if (nextContact == null) return;
        
        // select only the Contactables which are actually candidates
        HashSet<Contactable> newPotential = new HashSet<Contactable>();
        foreach (var c in levelManager.allContactables) {
            if (c.gameObject != source 
                && c.gameObject != nextContact.source
                && c.enabled) {
                newPotential.Add(c);
            }
        }
        
        /*
         * for each Contactable that we care about this update, check to see if it has an old angle on record.
         * if it does, we can check to see how that angle has changed since the previous frame.
         */
        foreach (var c in newPotential) {
            var oldPC = c.GetPotentialContact(this);
            var newPC = new Contactable.PotentialContact(this, c);
            
            if (!oldPC.Equals(default(Contactable.PotentialContact))) {
                // so we found an old record of this potential contact. cool.
                // get the angles of both of these
                var oldAngle = oldPC.parentAngle;
                var newAngle = newPC.parentAngle;
                var newRadius = newPC.parentRadius;
                
                // has this Contactable rotated in a way that crossed over this Contact's angle?
                // and is this Contactable now within distance of this Contact's line?
                if (oldAngle * newAngle < 0 && newRadius < yarnLineXZ.magnitude) {
                    // in that case, this potential contact becomes a new contact
                    yarn.InsertContact(newPC.source.gameObject, index);
                }
            }
            // update, or add for the first time, the newer potential contact.
            c.AddPotentialContact(this, newPC);
        }
    }
}