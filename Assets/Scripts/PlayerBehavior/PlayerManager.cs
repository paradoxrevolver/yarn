using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    public GameObject normalMesh;
    public GameObject holdingMesh;

    public Yarn yarnHeld { get; set; }
    
    public PlayerContactable playerContactable { get; protected set; }
    
    public enum State {
        Normal,
        Holding,
        Pulling,
    }

    private const State DEFAULT_STATE = State.Normal;
    private State state;

    private void Awake() {
        SetState(DEFAULT_STATE);
        playerContactable = GetComponent<PlayerContactable>();
    }

    public void SetState(State state) {
        this.state = state;
        UpdateFromState();
    }
    public bool CheckState(State state) { return state == this.state; }

    private void UpdateFromState() {
        if (CheckState(State.Normal)) {
            if(normalMesh) normalMesh.SetActive(true);
            if(holdingMesh) holdingMesh.SetActive(false);
        } else {
            if(holdingMesh) holdingMesh.SetActive(true);
            if(normalMesh) normalMesh.SetActive(false);
        }
    }
}
