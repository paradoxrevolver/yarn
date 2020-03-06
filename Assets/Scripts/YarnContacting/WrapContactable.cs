using UnityEngine;

/**
 * If a Yarn object wrapping around this Contactable should send a Contact to that Yarn,
 * put this Behaviour on that GameObject.
 */

public class WrapContactable : Contactable {
    // this is the radius of a circle on the XZ plane that the yarn line can hit to contact this object.
    [Tooltip("Specifies the radius of the infinitely tall cylinder that represents the WrapContactable.")]
    public float radius = 0.1f;

    protected override void Awake() {
        base.Awake();
        // if we're a WrapContactable, we better remove ourselves from our own list of wrap contactables
        wrapContactables.Remove(this);
    }

    public CircleContact GetNewContact(Vector3 initialPoint, float initialAngle, Contact prevContact, Contact nextContact = null) {
        return new CircleContact(this, initialPoint, initialAngle, prevContact, nextContact);
    }

    /**
     * Determines if, from one Contact to the other's position, a wrap has been made around a wrappee.
     * The WrapResult will specify whether a wrap has occurred or not.
     */
    public static WrapResult CheckWrapAround(Contact from, WrapContactable wrappee, Contact to) {
        // this is the point that all vectors under consideration are based from
        var hostPosition = from.tailRenderPoint;

        var positionDelta = to.host.currentPosition - to.host.pastPosition;
        
        // these are the vectors from the host position to the other contact, before and after.
        Vector3 oldVecToWrapper = to.headRenderPoint - hostPosition;
        Vector3 newVecToWrapper = to.headRenderPoint - hostPosition;

        // get both lines from the host that are tangent to the wrappee
        Vector3[] wrappeeLines = FindTangentPointsToCircle(wrappee.transform.position - hostPosition, wrappee.radius);
        var wrappeeLine = wrappeeLines[1];
        
        // pick one based on the direction of the old vec to the new vec
        // if the signed angle is position, rotation is clockwise, pick the clockwise-most wrappee line
        if (Vector3.SignedAngle(oldVecToWrapper, newVecToWrapper, Vector3.up) >= 0) {
            wrappeeLine = wrappeeLines[0];
        }
        
        
        
        return new WrapResult();
    }

    public struct WrapResult {
        public bool wrapOccurred;
        public WrapContactable wrappee;
        public Vector3 wrapPosition;
        public float initialAngle;

        public WrapResult(bool wrapOccurred, WrapContactable wrappee, Vector3 wrapPosition, float initialAngle) {
            this.wrapOccurred = wrapOccurred;
            this.wrappee = wrappee;
            this.wrapPosition = wrapPosition;
            this.initialAngle = initialAngle;
        }
    }

    /**
     * Returns true if, from oldVec to newVec, it has wrapped around baseVec on the XZ plane.
     */
    public static bool VectorHasWrappedVector(Vector3 baseVec, Vector3 oldVec, Vector3 newVec) {
        float oldAngle = Vector3.SignedAngle(baseVec, oldVec, Vector3.up);
        float newAngle = Vector3.SignedAngle(baseVec, newVec, Vector3.up);
        
        // if the angle between baseVec to the other vector has flipped (one is positive, one is negative)
        // and if either the old or the new vector is longer than baseVec
        // then one vector has wrapped another
        return (oldAngle * newAngle < 0) && 
               (oldVec.magnitude > baseVec.magnitude || newVec.magnitude > baseVec.magnitude);
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