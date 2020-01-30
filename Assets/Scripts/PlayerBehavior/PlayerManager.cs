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

    public GameObject normalMesh;
    public GameObject holdingMesh;

    public Yarn YarnHeld { get; set; }

    private void Awake() {
        SetState(State.Normal);
    }

    public void SetState(State state) {
        this.state = state;
        UpdateMeshFromState();
    }
    public bool CheckState(State state) { return state == this.state; }

    private void UpdateMeshFromState() {
        if (CheckState(State.Normal)) {
            if(normalMesh) normalMesh.SetActive(true);
            if(holdingMesh) holdingMesh.SetActive(false);
        } else {
            if(holdingMesh) holdingMesh.SetActive(true);
            if(normalMesh) normalMesh.SetActive(false);
        }
    }
}
