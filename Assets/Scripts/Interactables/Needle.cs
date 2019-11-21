using UnityEngine.UI;

public class Needle : Interactable {
    public enum State {
        Raised,
        Lowered,
    }

    private State state;

    public override void Interact() {
        ToggleState();
        UpdateMesh();
    }

    private void UpdateMesh() { 
        
    }

    private void ToggleState() { state = state == State.Raised ? State.Lowered : State.Raised; }

}