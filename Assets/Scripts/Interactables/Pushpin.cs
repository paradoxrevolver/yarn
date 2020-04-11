using System;

public class Pushpin : Interactable {

    public Yarn connectedYarn { get; protected set; }

    public InteractContactable interactContactable { get; protected set; }

    public enum State {
        Untied,
        Tied,
        Done,
    }
    private State state;

    private bool CheckState(State state) { return state == this.state; }
    public bool IsUntied() { return CheckState(State.Untied); }
    public bool IsTied() { return CheckState(State.Tied); }
    public bool IsDone() { return CheckState(State.Done); }

    private void SetState(State state) { this.state = state; }

    protected override void Awake() {
        base.Awake();
        state = State.Untied;
        connectedYarn = null;
        interactContactable = GetComponent<InteractContactable>();
    }

    public override void Interact(Player player) {
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
                if (player.IsHolding()) {
                    // start a line of yarn
                    SetState(State.Tied);
                    player.StartPulling(connectedYarn, this);
                    connectedYarn.StartYarnLine(this);
                    print("Yarn was just tied to a Pushpin, starting a line.");
                    
                } else if (player.IsPulling()) {
                    // finish a line of yarn
                    SetState(State.Tied);
                    player.FinishPulling(connectedYarn, this);
                    connectedYarn.FinishYarnLine(this);
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
                // connected yarn needs to have a yarn line to be detached
                if (connectedYarn.yarnLine == null) break;
                
                if (player.IsNormal()) {
                    // resume editing a line of yarn
                    SetState(State.Untied);
                    player.EditPulling(connectedYarn, this);
                    connectedYarn.EditYarnLine(this);
                    print("Yarn was just untied from a Pushpin, to edit a line.");
                } else if (player.IsPulling()) {
                    // undo the line of yarn entirely
                    SetState(State.Untied);
                    player.UndoPulling(connectedYarn, this);
                    connectedYarn.UndoYarnLine(this);
                    print("Yarn was just untied from a Pushpin, undoing a line.");
                
                }
                break;
            }
        }
    }
}