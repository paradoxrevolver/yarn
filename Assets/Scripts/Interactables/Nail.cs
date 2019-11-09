using System;
using UnityEngine;
using UnityEngine.UI;

public class Nail : Interactable {
    // A Nail manages its own state
    public enum State {
        Raised,
        Lowered,
    }
    private State _state;

    // Stores a reference to the mesh and the position it started in.
    private GameObject _mesh;
    private Vector3 _meshOffsetDefault;
    private Vector3 _meshOffsetGoal;
    [SerializeField] private Vector3 meshLowerOffset = new Vector3(0, -2, 0);
    [SerializeField] private float meshMoveSpeed = 0.1f;

    protected override void Awake() {
        base.Awake();
        _state = State.Raised;
        _mesh = GetComponentInChildren<MeshRenderer>().gameObject;
        _meshOffsetDefault = _mesh.transform.position;
        _meshOffsetGoal = _meshOffsetDefault;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            Interact();
        }
    }

    public override void Interact() {
        ToggleState();
        UpdateMeshGoal();
    }

    private void Update() {
        UpdateMesh();
    }

    // Toggles between the two possible states for a Nail.
    private void ToggleState() { _state = _state == State.Raised ? State.Lowered : State.Raised; }

    // Updates the Nail's mesh.
    private void UpdateMesh() {
        _mesh.transform.position = Vector3.Lerp(_mesh.transform.position, _meshOffsetGoal, meshMoveSpeed);
    }
    
    // Updates the goal position of the Nail's mesh based on other factors.
    private void UpdateMeshGoal() {
        _meshOffsetGoal = _state == State.Raised
            ? _meshOffsetDefault
            : _meshOffsetDefault + meshLowerOffset;
    }
}