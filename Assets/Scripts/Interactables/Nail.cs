public class Nail : Interactable {
    // A Nail manages its own state
    public enum State {
        Raised,
        Lowered,
    }
    private State state;

    // Stores a reference to the mesh and the position it started in.
    private Lowerable meshLowerable;

    protected override void Awake() {
        base.Awake();
        state = State.Raised;
        meshLowerable = GetComponentInChildren<Lowerable>();
    }

    public override void Interact(Player player) {
        ToggleState();
    }

    // Toggles between the two possible states for a Nail.
    private void ToggleState() {
        state = state == State.Raised ? State.Lowered : State.Raised;
        UpdateFromState();
    }

    private void UpdateFromState() {
        if (state == State.Lowered) meshLowerable.Lower();
        else meshLowerable.Raise();
    }
}