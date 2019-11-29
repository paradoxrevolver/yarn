public class Pushpin : Interactable {
    public enum State {
        Untied,
        Tied,
        Done,
    }
    private State state;
    
    public bool IsDone() { return state == State.Done; }

    // the yarn that is connected to this pushpin. can only be one
    private Yarn connectedYarn;
    
    protected override void Awake() {
        base.Awake();
        state = State.Untied;
        connectedYarn = null;
    }

    public override void Interact() {
        YarnUpdate();
    }

    /**
     * A Pushpin will attempt to update the player and any yarn they're holding
     * based on the states of those objects.
     */
    private void YarnUpdate() {
        Yarn playerYarn = playerManager.YarnHeld;
        
        /*
         * Yarn can be attached to this Pushpin if:
         * - This Pushpin is Untied to anything
         *     - The player is holding onto yarn, in which case the player wants to start a line of yarn
         *     - The player is pulling yarn, in which case the player wants to finish a line of yarn
         */
        if (state == State.Untied) {
            if (playerManager.CheckState(PlayerManager.State.Holding)) {
                // start a line of yarn
                state = State.Tied;
                playerManager.SetState(PlayerManager.State.Pulling);
                connectedYarn = playerYarn;
                connectedYarn.AddContact(gameObject);
                print("Yarn was just tied to a Pushpin, starting a line.");

            } else if (playerManager.CheckState(PlayerManager.State.Pulling)) {
                // finish a line of yarn
                state = State.Tied;
                connectedYarn = playerYarn;
                connectedYarn.TieOff(this);
                print("Yarn was just tied to a Pushpin, ending a line.");
            }
        }
        
        /*
         * Yarn can be detached from this Pushpin if:
         * - This Pushpin is Tied OR Done already
         *     - The player is pulling yarn, which case they want to return to just holding yarn
         *         - This Pushpin must be part of this line of yarn AND be the only thing other than the player
         *     - The player is normal, in which case they want to change an existing line of yarn
         *         - This Pushpin must be either the first or last in the line of yarn AND there are at least two contacts
         */
        else if (state == State.Tied || state == State.Done) {
            if (playerManager.CheckState(PlayerManager.State.Pulling)
                    && connectedYarn.ContactCount() == 2
                    && connectedYarn == playerYarn) {
                // return to holding yarn
                state = State.Untied;
                playerManager.SetState(PlayerManager.State.Holding);
                connectedYarn.RemoveContactAll(gameObject);
                connectedYarn = null;
                print("Yarn was just untied from a Pushpin, undoing a line.");
                
            } else if (playerManager.CheckState(PlayerManager.State.Normal)
                    && connectedYarn.ContactCount() >= 2
                    && connectedYarn.IsContactAtEnd(gameObject)) {
                // start changing a line of yarn again
                state = State.Untied;
                connectedYarn.UntieFrom(this);
                connectedYarn = null;
                print("Yarn was just untied from a Pushpin, to edit a line.");
            }
        }
    }
}