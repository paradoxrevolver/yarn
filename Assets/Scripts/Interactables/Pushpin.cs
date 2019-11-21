public class Pushpin : Interactable {
    public enum State {
        Untied,
        Tied,
        Done,
    }

    private State state;

    protected override void Awake() {
        base.Awake();
        state = State.Untied;
    }

    public void SetState(State newState) { state = newState; }
    public bool IsDone() { return state == State.Done; }

    public override void Interact() {
        print("A Pushpin was interacted with and nothing happened.");
    }
}