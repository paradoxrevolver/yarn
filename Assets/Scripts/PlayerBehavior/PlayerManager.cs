using System;
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

    private const State DEFAULT_STATE = State.Normal;
    private State state;

    public GameObject normalMesh;
    public GameObject holdingMesh;

    public Yarn YarnHeld { get; set; }

    private void Start() {
        SetState(DEFAULT_STATE);
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
