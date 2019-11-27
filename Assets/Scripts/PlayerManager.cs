using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum State {
        Normal,
        Holding,
        Pulling,
    }
    private State state;

    public Yarn YarnHeld { get; set; }

    private void Awake() {
        state = State.Normal;
    }
    public void SetState(State state) { this.state = state; }
    public bool CheckState(State state) { return state == this.state; }
}
