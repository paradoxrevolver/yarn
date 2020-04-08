using System;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;
    public LineRenderer lineRenderer;
    public GameObject mesh;

    private LevelManager levelManager;
    [HideInInspector] public YarnLine yarnLine;

    public enum State {
        Normal,
        Destroyed,
    }
    private State state;

    public bool IsDestroyed() { return state == State.Destroyed; }
    
    protected override void Awake() {
        base.Awake();
        state = State.Normal;
        yarnLine = null;
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate() {
        // update the physics of the yarn line
        //yarnLine?.FixedUpdate();
        // todo: allow yarn to update when things are ready
    }

    private void Update() {
        // update the visuals of the yarn line
        //yarnLine?.Update();

        // update the points the LineRenderer is rendering
        if (lineRenderer && yarnLine != null) {
            var linePoints = new List<Vector3>();
            foreach (var c in yarnLine.contacts) linePoints.AddRange(c.renderPoints);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }
    }

    public override void Interact(PlayerManager player) {
        // the player picks up the yarn if they have their arms free
        if (playerManager.CheckState(PlayerManager.State.Normal)) {
            PickUp();
        }
    }

    private void HideMesh() { mesh.SetActive(false); }

    private void ShowMesh() { mesh.SetActive(true); }

    private void PickUp() {
        // give the player the yarn object and tell them to start holding
        playerManager.SetState(PlayerManager.State.Holding);
        playerManager.yarnHeld = this;
        
        // this yarn is no longer interactable
        playerInteract.RemoveInteractable(this);
        
        // parent the yarn to the player and position it in their arms
        transform.SetParent(player.transform);
        transform.localPosition = positionYarnInPlayersArms;
        
        print("The player just picked up some yarn.");
    }

    public void PutDown() {
        // return the player to normal, take it from the player
        playerManager.SetState(PlayerManager.State.Normal);
        playerManager.yarnHeld = null;
        
        // unparent the yarn, pulling it out to the outermost level
        transform.parent = null;
        
        // calculate a new position for the yarn on the grid and set it there
        Vector3 positionOnGround = new Vector3(
            Mathf.Round(player.transform.localPosition.x/2f)*2f,
            0,
            Mathf.Round(player.transform.localPosition.z/2f)*2f);
        transform.localPosition = positionOnGround;
        
        print("The player has dropped the yarn.");
    }
    
    /* Starts a line of yarn on the given pushpin. */
    public void TieStart(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Normal);
        if(mesh) HideMesh();
        
        yarnLine = new YarnLine(this, playerManager.playerContactable.GetNewContact(), pushpin.interactContactable.GetNewContact());
    }

    /* Finishes off a line of yarn on the given pushpin. */
    public void TieEnd(Pushpin pushpin) {
        
    }

    /* Untie a line of yarn from a pushpin to continue to edit the line. */
    public void UntieStart(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Pulling);
        if(mesh) ShowMesh();
        
        // todo: remove the pushpin's contact, add the player
    }

    /* Untie a line of yarn from a pushpin to remove the line. */
    public void UntieEnd(Pushpin pushpin) {
        
    }

  
}