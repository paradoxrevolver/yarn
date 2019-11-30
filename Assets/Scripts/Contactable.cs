using System.Collections.Generic;
using UnityEngine;

/**
 * A GameObject with this class has the ability to become a Contact in a line of yarn.
 *
 * Disabling this component will prevent it from becoming a Contact.
 */
public class Contactable : MonoBehaviour {
    /**
     * A PotentialContact is created by a currently existing Contact somewhere in the scene
     * because it cares about this Contactable's angle to it.
     */
    public struct PotentialContact {
        /*
         * the Contact that would become a parent of this Contactable
         * if this were to become a Contact
         */
        public Contact parent;
        
        // the Contactable behind this PotentialContact
        public Contactable source;
        
        // the angle this Contactable has with the parent Contact
        public float parentAngle;
        public float parentRadius;

        public PotentialContact(Contact parent, Contactable source) {
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

    /*
     * Every Contactable keeps track of which Contacts consider it a PotentialContact
     * and maintains the PotentialContact objects which represent this relationship.
     */
    private Dictionary<Contact, PotentialContact> potentialParents = new Dictionary<Contact, PotentialContact>();

    public PotentialContact GetPotentialContact(Contact c) {
        return potentialParents.ContainsKey(c) ? potentialParents[c] : default;
    }

    public void AddPotentialContact(Contact c, PotentialContact pc) {
        // adding always acts a replace if this key already exists.
        if (potentialParents.ContainsKey(c)) potentialParents.Remove(c);
        potentialParents.Add(c, pc);
    }
}
