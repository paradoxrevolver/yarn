using UnityEngine;

public class Nail : Interactable {
    // A Nail manages its own state
    public enum State {
        Raised,
        Lowered,
    }
    private State state;

    // Stores a reference to the mesh and the position it started in.
    private GameObject mesh;
    private Vector3 meshOffsetDefault;
    private Vector3 meshOffsetGoal;
    [SerializeField] private Vector3 meshLowerOffset = new Vector3(0, -2, 0);
    [SerializeField] private float meshMoveSpeed = 0.1f;

    protected override void Awake() {
        base.Awake();
        state = State.Raised;
        mesh = GetComponentInChildren<MeshRenderer>().gameObject;
        meshOffsetDefault = mesh.transform.position;
        meshOffsetGoal = meshOffsetDefault;
    }

    public override void Interact() {
        ToggleState();
        UpdateMeshGoal();
    }

    private void Update() {
        UpdateMesh();
    }

    // Toggles between the two possible states for a Nail.
    private void ToggleState() { state = state == State.Raised ? State.Lowered : State.Raised; }

    // Updates the Nail's mesh.
    private void UpdateMesh() {
        mesh.transform.position = Vector3.Lerp(mesh.transform.position, meshOffsetGoal, meshMoveSpeed);
    }
    
    // Updates the goal position of the Nail's mesh based on other factors.
    private void UpdateMeshGoal() {
        meshOffsetGoal = state == State.Raised
            ? meshOffsetDefault
            : meshOffsetDefault + meshLowerOffset;
    }
}