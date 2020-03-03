using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
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
    public Vector3 pastPosition;
    public Vector3 currentPosition;

    private void Awake() {
        var position = transform.position;
        pastPosition = position;
        currentPosition = position;
    }

    protected virtual void FixedUpdate() {
        // update past and current positions
        pastPosition = currentPosition;
        currentPosition = transform.position;
    }
}

/**
 * If a Yarn object wrapping around this Contactable should send a Contact to that Yarn,
 * put this Behaviour on that GameObject.
 */
public class WrapContactable : Contactable {
    // this is the radius of a circle on the XZ plane that the yarn line can hit to contact this object.
    [Tooltip("Specifies the radius of the infinitely tall cylinder that represents the WrapContactable.")]
    public float radius = 0.1f;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        // todo: do whatever needs to be done with past and current positions
    }

    public CircleContact GetNewContact(Vector3 initialPoint, float initialAngle, Contact prevContact, Contact nextContact = null) {
        return new CircleContact(this, initialPoint, initialAngle, prevContact, nextContact);
    }

    /**
     * Returns true if, from the host to the wrapper's position, the wrapper has wrapped around the wrappee.
     */
    public static bool CheckWrapAround(Contactable host, WrapContactable wrappee, Contactable wrapper) {
        var hostPosition = host.transform.position;
        
        Vector3 vecToObj = wrappee.transform.position - hostPosition;
        Vector3 oldVecToWrapper = wrapper.pastPosition - hostPosition;
        Vector3 newVecToWrapper = wrapper.transform.position - hostPosition;
        
        float oldAngle = Vector3.SignedAngle(vecToObj, oldVecToWrapper, Vector3.up);
        float newAngle = Vector3.SignedAngle(vecToObj, newVecToWrapper, Vector3.up);
        // if the angle from the wrapper to the object has flipped (one is positive, one is negative)
        // and if either the old or the new vector to the wrapper is longer than the vector to the object
        // then the wrapper has successfully wrapped around
        return (oldAngle * newAngle < 0) && 
               (oldVecToWrapper.magnitude > vecToObj.magnitude || newVecToWrapper.magnitude > vecToObj.magnitude);
    }
    
    /**
     * This method returns both points on a circle on the XZ axis that is tangent from the base of vecToCircleCenter.
     * 
     * Starting from the base of vecToCircleCenter, two lines can always be drawn tangent to a circle centered at the
     * end of vecToCircleCenter with a radius of radius. Both of these points are returned in a Vector3[], with
     * element 0 being the vector counter-clockwise (negative angle) from vecToCircleCenter and element 1 being
     * the other.
     */
    private static Vector3[] FindTangentPointsToCircle(Vector3 vecToCircleCenter, float radius) {
        // this is the angle between vecToCircleCenter to both tangents
        float angleToTangents = Mathf.Asin(radius / vecToCircleCenter.magnitude);
        Vector3 negativeTangent = Quaternion.Euler(0, angleToTangents, 0) * vecToCircleCenter;
        Vector3 positiveTangent = Quaternion.Euler(0, -angleToTangents, 0) * vecToCircleCenter;
        return new [] {
            Vector3.Project(vecToCircleCenter, negativeTangent),
            Vector3.Project(vecToCircleCenter, positiveTangent)
        };
    }
}

/**
 * If the player interacting with this Contactable should send a Contact to the player,
 * put this Behaviour on that GameObject.
 */
public class InteractContactable : Contactable {
    [Tooltip("This is the offset from the GameObject's position where yarn will be attached.")]
    public Vector3 pointOffset;
    
    public PointContact GetNewContact() {
        return new PointContact(this);
    }
}