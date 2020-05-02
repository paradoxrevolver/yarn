using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/**
 * A Contact is a single instance of a Yarn object being connected to something.
 *
 * Only Contactables can create Contacts of themselves.
 */
public abstract class Contact {
    // uses host for GameObject data and Transform
    public Contactable host { get; protected set; }
    
    // needs prev and nextContacts for calculating angles
    public Contact prevContact { get; set; }
    public Contact nextContact { get; set; }
    
    // render points describe how the yarn is visually connected to this Contact
    public List<Vector3> renderPoints { get; protected set; }
    public Vector3 headRenderPoint => renderPoints[0];
    public Vector3 tailRenderPoint => renderPoints[renderPoints.Count-1];

    // the vector entering this Contact from a previous Contact
    public Vector3 arrivingVector {
        get {
            if (prevContact == null) {
                Debug.LogError($"{host.name} is hosting a Contact that was asked for an arriving vector, " +
                               $"but this Contact has no previous Contact!");
            }
            return headRenderPoint - prevContact.tailRenderPoint;
        }
    }

    // the vector leaving this Contact to the next Contact
    public Vector3 leavingVector {
        get {
            if (prevContact == null) {
                Debug.LogError($"{host.name} is hosting a Contact that was asked for a leaving vector, " +
                               $"but this Contact has no next Contact!");
            }
            return nextContact.headRenderPoint - tailRenderPoint;
        }
    }

    /**
     * Updates all of this Contact's data based on the position of the nextContact.
     *
     * It's assumed that the nextContact is a fluidly moving GameObject, so all calculations
     * are performed between previous and upcoming data.
     */
    public abstract void Update(Contact nextContact);
}

public class CircleContact : Contact {
    // how many times has this CircleContact been wrapped around?
    public int rotations { get; }
    // what angle does this CircleContact's line leave at compared to its initial angle?
    public float angle { get; }

    /**
     * A CircleContact can only be created once:
     * - We know the Contactable that is hosting the CircleContact
     * - We know the initial render point that caused the CircleContact
     * - We know the initial angle of contact so we can tell which way yarn is wrapping
     * - We may know the previous Contact or the next Contact
     */
    public CircleContact(
            WrapContactable host, 
            Vector3 initialPoint, 
            float initialAngle, 
            Contact prevContact = null,
            Contact nextContact = null) {
        this.host = host;
        this.prevContact = prevContact;
        this.nextContact = nextContact;
        // render points always has at least one point in it, it may never be empty
        renderPoints = new List<Vector3>{initialPoint};
        rotations = 0;
        angle = initialAngle;
    }

    public override void Update(Contact nextContact) {
        
    }
}

public class PointContact : Contact {
    public PointContact(
            InteractContactable host,
            Contact prevContact = null,
            Contact nextContact = null) {
        this.host = host;
        this.prevContact = prevContact;
        this.nextContact = nextContact;
        renderPoints = new List<Vector3>{host.transform.position + host.pointOffset};
    }

    public override void Update(Contact nextContact) {
        
    }
}