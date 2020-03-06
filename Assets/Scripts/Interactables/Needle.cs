public class Needle : Interactable {
    public enum State {
        Raised,
        Lowered,
    }
    private State state;
    private Lowerable meshLowerable;

    protected override void Awake() {
        base.Awake();
        state = State.Raised;
        meshLowerable = GetComponentInChildren<Lowerable>();
    }

    public override void Interact(PlayerManager player) {
        ToggleState();
    }

    private void ToggleState() {
        state = state == State.Raised ? State.Lowered : State.Raised;
        UpdateFromState();
    }

    private void UpdateFromState() {
        if (state == State.Lowered) meshLowerable.Lower();
        else meshLowerable.Raise();
    }

}