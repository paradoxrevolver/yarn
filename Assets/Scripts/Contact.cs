using System;
using System.Collections.Generic;
using System.Linq;
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

    // the initial angular velocity the player had when this Contact was created
    private float initialAngle;
    
    // a list of calculated points that the yarn is actually rendered at
    public List<Vector3> renderPoints;

    // a list of candidates that this Contact might turn into a proceeding contact
    public Dictionary<Contactable, Candidate> candidates;

    public Contact(Yarn yarn, GameObject source, LevelManager levelManager, float initialAngle) {
        this.yarn = yarn;
        this.source = source;
        this.levelManager = levelManager;
        this.initialAngle = initialAngle;
        renderPoints = new List<Vector3>();
        candidates = new Dictionary<Contactable, Candidate>();
        UpdateRenderPoints();
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
     * Updates the list of Candidates that this Contact is watching.
     *
     * If a Candidate has changed in some way this update that suggests it should
     * become a new Contact, this Contact will add it.
     */
    public void UpdateCandidates(Contact nextContact, int index) {
        if (nextContact == null) return;
        
        /*
         * Create a new list of candidates. 
         */
        Dictionary<Contactable, Candidate> newCandidates = new Dictionary<Contactable, Candidate>();
        foreach (var c in levelManager.allContactables) {
            if (c.gameObject != source 
                && c.gameObject != nextContact.source
                && c.enabled) {
                newCandidates.Add(c, new Candidate(this, c));
            }
        }
        
        /*
         * For each Candidate this Contact cares about this update, check to see if it has an old
         * record. If it does, check several criteria to see if it changed in a way that makes it
         * deserve to become a Contact. If it doesn't, 
         */
        foreach (var candidate in newCandidates) {
            var oldCandidate = GetCandidate(candidate.Key);
            var newCandidate = candidate.Value;
            
            if (!oldCandidate.Equals(default(Candidate))) {
                // so we found an old record of this candidate. cool.
                // get the angles of both of these
                var oldAngle = oldCandidate.parentAngle;
                var newAngle = newCandidate.parentAngle;
                var newRadius = newCandidate.parentRadius;
                
                // has this Candidate rotated in a way that crossed over this Contact's angle?
                // and is this Candidate now within distance of this Contact's line?
                // and is the new angle close-ish to zero so we aren't connecting backwards?
                if (oldAngle * newAngle < 0 
                        && newRadius < yarnLineXZ.magnitude
                        && Math.Abs(newAngle) < 90) {
                    // if all of this is true, this candidate becomes a new contact
                    yarn.InsertContact(newCandidate.source.gameObject, index + 1, newAngle);
                }
            }
            // update the list of candidates, deleting the old ones
            candidates = newCandidates;
        }
    }

    public void UpdateUnraveled(Contact prevContact) {
        var prevYarnLineXZ = prevContact.yarnLineXZ;
        var angleToPrevious = Vector2.SignedAngle(prevYarnLineXZ, yarnLineXZ);
        if (angleToPrevious * initialAngle < 0
                && Math.Abs(angleToPrevious) < 90) {
            yarn.RemoveContact(this);
            // todo: does this delete this contact properly?
        }
    }

    private Candidate GetCandidate(Contactable c) {
        return candidates.ContainsKey(c) ? candidates[c] : default;
    }
    
    /**
     * A Candidate is created by a currently existing Contact somewhere in the scene
     * because it cares about this Contactable's angle to it.
     */
    public struct Candidate {
        /*
         * the Contact that is interested in the details of this Candidate
         */
        public Contact parent;
        
        // the Contactable behind this Candidate
        public Contactable source;
        
        // the angle this Contactable has with the parent Contact
        public float parentAngle;
        public float parentRadius;

        public Candidate(Contact parent, Contactable source) {
            this.parent = parent;
            this.source = source;
            
            /*
             * get the signed angle between the parent Contact's yarnLineXZ
             * and the line between the parent and this Contactable
             */
            var parentPos = parent.renderPoints[parent.renderPoints.Count - 1];
            var sourcePos = source.transform.position;
            var vecParentToThis = sourcePos - parentPos;
            var vecParentToThisXZ = new Vector2(vecParentToThis.x, vecParentToThis.z);
            parentAngle = Vector2.SignedAngle(parent.yarnLineXZ, vecParentToThisXZ);
            parentRadius = vecParentToThisXZ.magnitude;
        }
    }
}