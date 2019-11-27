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
        if(playerManager.CheckState(PlayerManager.State.Holding)
                && state == State.Untied) 
            AttachYarn(playerManager.YarnHeld);
    }

    private void AttachYarn(Yarn yarn) {
        state = State.Tied;
        if(playerManager.CheckState(PlayerManager.State.Holding))
            playerManager.SetState(PlayerManager.State.Pulling);
        yarn.AddContact(new Yarn.Contact(gameObject));
        print("The player just attached yarn to a pushpin.");
    }

    private void DetachYarn() {
        state = State.Untied;
        playerManager.SetState(PlayerManager.State.Holding);
    }
}