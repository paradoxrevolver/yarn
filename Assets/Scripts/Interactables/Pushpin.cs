public class Pushpin : Interactable {

    private Yarn connectedYarn;

    public InteractContactable interactContactable { get; protected set; }

    public enum State {
        Untied,
        Tied,
        Done,
    }
    private State state;
    
    public bool IsDone() { return state == State.Done; }
    protected override void Awake() {
        base.Awake();
        state = State.Untied;
        connectedYarn = null;
        interactContactable = GetComponent<InteractContactable>();
    }

    public override void Interact(PlayerManager player) {
        base.Interact(player);
        connectedYarn = player.yarnHeld;

        switch (state) {
            /*
             * Yarn can be attached to this Pushpin if:
             * - This Pushpin is Untied to anything
             *     - The player is holding onto yarn, in which case the player wants to start a line of yarn
             *     - The player is pulling yarn, in which case the player wants to finish a line of yarn
             */
            case State.Untied: {
                if (playerManager.CheckState(PlayerManager.State.Holding)) {
                    state = State.Tied;
                    playerManager.SetState(PlayerManager.State.Pulling);
                    connectedYarn.TieStart(this);
                    print("Yarn was just tied to a Pushpin, starting a line.");
                    
                } else if (playerManager.CheckState(PlayerManager.State.Pulling)) {
                    state = State.Tied;
                    playerManager.SetState(PlayerManager.State.Normal);
                    connectedYarn.TieEnd(this);
                    print("Yarn was just tied to a Pushpin, ending a line.");
                }
                break;
            }
            /*
             * Yarn can be detached from this Pushpin if:
             * - This Pushpin is Tied OR Done already
             *     - The player is pulling yarn, in which case they want to return to just holding yarn
             *         - This Pushpin must be part of this line of yarn AND be the only thing other than the player
             *     - The player is normal, in which case they want to change an existing line of yarn
             *         - This Pushpin must be either the first or last in the line of yarn
             */
            case State.Tied:
            case State.Done: {
                if (playerManager.CheckState(PlayerManager.State.Pulling)
                        && connectedYarn.yarnLine != null) {
                    // return to holding yarn
                    state = State.Untied;
                    playerManager.SetState(PlayerManager.State.Holding);
                    connectedYarn.UntieEnd(this);
                    print("Yarn was just untied from a Pushpin, undoing a line.");
                
                } else if (playerManager.CheckState(PlayerManager.State.Normal)
                        && connectedYarn.yarnLine != null) {
                    // start changing a line of yarn again
                    state = State.Untied;
                    connectedYarn.UntieStart(this);
                    print("Yarn was just untied from a Pushpin, to edit a line.");
                }
                break;
            }
        }
    }
}