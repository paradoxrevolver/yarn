using System;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    private struct Contact {
        private Vector3 position;
        private float interiorAngle;
        private bool isClockwise;
        private GameObject source;
    }
    private List<Contact> contacts;
    
    public enum State {
        Normal,
        Held,
        Unraveling,
        Settled,
        Destroyed,
    }
    private State state;

    public bool IsDestroyed() { return state == State.Destroyed; }

    protected override void Awake() {
        base.Awake();
        state = State.Normal;
    }

    private void FixedUpdate() {
        RaycastHit hit;
        
    }

    public override void Interact() {
        print("Yarn was interacted with and nothing happened.");
    }

    private void OnDrawGizmos() {
        
    }
}