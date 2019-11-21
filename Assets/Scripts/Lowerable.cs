using UnityEngine;

/**
 * This script is capable of lowering or raising the
 * GameObject it is attached to.
 */
public class Lowerable : MonoBehaviour {
    [Tooltip("How much should the GameObject be lowered by?")]
    public Vector3 lowerOffset = new Vector3(0, -2, 0);
    [Tooltip("How fast should it move?")]
    public float moveSpeed = 0.1f;
    
    private Vector3 offsetDefault;
    private Vector3 offsetGoal;

    private void Awake() {
        offsetDefault = transform.position;
        offsetGoal = offsetDefault;
    }

    private void Update() {
        transform.position = Vector3.Lerp(
            transform.position,
            offsetGoal,
            moveSpeed);
    }

    public void Lower() { offsetGoal = offsetDefault + lowerOffset; }

    public void Raise() { offsetGoal = offsetDefault; }
}
