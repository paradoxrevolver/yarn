using System;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public GameObject normalMesh;
    public GameObject holdingMesh;

    public Yarn yarnHeld { get; set; }
    public PlayerContactable playerContactable { get; protected set; }
    
    public enum State {
        Normal,
        Holding,
        Pulling,
    }

    private State state;
    private const State DEFAULT_STATE = State.Normal;

    private bool CheckState(State state) { return state == this.state; }
    public bool IsNormal() { return CheckState(State.Normal); }
    public bool IsHolding() { return CheckState(State.Holding); }
    public bool IsPulling() { return CheckState(State.Pulling); }

    private void SetState(State state) {
        if(state == this.state) Debug.LogWarning($"{this} was told to set state to {state}, but was already of state {this.state}.");
        this.state = state;
        
        // perform updates that can be entirely determined from state
        if(IsNormal()) ShowNormalMesh();
        else ShowHoldingMesh();
    }

    /* Player, not holding yarn, is given yarn to hold. */
    public void GiveYarn(Yarn yarn) {
        if (yarnHeld != null) throw new Exception($"{this} was given yarn, but already had yarn!");
        
        yarnHeld = yarn;
        SetState(State.Holding);
    }

    /* Player, already holding yarn, loses their yarn. */
    public void RemoveYarn(Yarn yarn) {
        if (yarnHeld != yarn) throw new Exception($"{this} was told to remove yarn that wasn't the yarn being held!");
        
        yarnHeld = null;
        SetState(State.Normal);
    }

    /* Player, already holding yarn, starts pulling a line of yarn from a Pushpin. */
    public void StartPulling(Yarn yarn, Pushpin pushpin) {
        if (yarn != yarnHeld) throw new Exception($"{this} was told to start pulling yarn from a pushpin, but StartPulling was given yarn different from the yarn the player is holding.");
        SetState(State.Pulling);
    }

    /* Player, previously holding yarn, finishes pulling a line of yarn at a Pushpin. */
    public void FinishPulling(Yarn yarn, Pushpin pushpin) {
        if(yarn != yarnHeld) throw new Exception($"{this} was told to finish pulling yarn at a pushpin, but FinishPulling was given yarn different from the yarn the player is holding.");
        RemoveYarn(yarn);
    }

    /* Player, holding nothing, wants to edit a line of yarn from a Pushpin. */
    public void EditPulling(Yarn yarn, Pushpin pushpin) {
        if (yarnHeld != null) throw new Exception($"{this} was told to edit a line of yarn at a pushpin, but the player was already holding yarn.");
        if (pushpin.connectedYarn != yarn) throw new Exception($"{this} was told to edit a line of yarn at a pushpin, but EditPulling was given a pushpin and yarn that were not connected!");
        // give the player the yarn and then move directly into a pulling state,
        // because this yarn and pushpin are already connected
        GiveYarn(yarn);
        SetState(State.Pulling);
    }

    /* Player, already pulling yarn, wants to undo a line of yarn they just started from a Pushpin. */
    public void UndoPulling(Yarn yarn, Pushpin pushpin) {
        SetState(State.Holding);
    }

    private void ShowNormalMesh() {
        if(normalMesh) normalMesh.SetActive(true);
        if(holdingMesh) holdingMesh.SetActive(false);
    }

    private void ShowHoldingMesh() {
        if(holdingMesh) holdingMesh.SetActive(true);
        if(normalMesh) normalMesh.SetActive(false);
    }

    private void Awake() {
        SetState(DEFAULT_STATE);
        playerContactable = GetComponent<PlayerContactable>();
    }
}
