using System;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;
    public LineRenderer lineRenderer;
    public GameObject mesh;

    private LevelManager levelManager;
    private YarnLine yarnLine;

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

    private void Update() {
        // update the visuals of the yarn line
        yarnLine?.Update();

        // update the points the LineRenderer is rendering
        if (lineRenderer && yarnLine != null) {
            var linePoints = new List<Vector3>();
            foreach (var c in yarnLine.contacts) linePoints.AddRange(c.renderPoints);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }
    }

    public override void Interact() {
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
        playerManager.YarnHeld = this;
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
        playerManager.YarnHeld = null;
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
    
    /* Ties this Yarn off on the given Pushpin, removing control from the player. */
    public void TieTo(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Normal);
        if(mesh) HideMesh();
        
        // todo: switch the pushpin and player
    }

    /* Untie this Yarn object from the given Pushpin, returning control to the player. */
    public void UntieFrom(Pushpin pushpin) {
        playerManager.SetState(PlayerManager.State.Pulling);
        if(mesh) ShowMesh();
        
        // todo: remove the pushpin's contact, add the player
    }
}