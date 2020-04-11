using System;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : Interactable {
    public Vector3 positionYarnInPlayersArms;
    public LineRenderer lineRenderer;
    public GameObject mesh;

    private LevelManager levelManager;
    public YarnLine yarnLine { get; private set; }

    public enum State {
        Normal,
        Destroyed,
    }
    private State state;

    private bool CheckState(State state) { return state == this.state; }
    public bool IsNormal() { return CheckState(State.Normal); }
    public bool IsDestroyed() { return CheckState(State.Destroyed); }
    
    protected override void Awake() {
        base.Awake();
        state = State.Normal;
        yarnLine = null;
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate() {
        // update the physics of the yarn line
        yarnLine?.PhysicsUpdate();
    }

    private void Update() {
        // update the visuals of the yarn line
        yarnLine?.Draw();

        // update the points the LineRenderer is rendering
        if (lineRenderer && yarnLine != null) {
            var linePoints = new List<Vector3>();
            foreach (var c in yarnLine.contacts) linePoints.AddRange(c.renderPoints);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
        }
    }

    public override void Interact(Player player) {
        base.Interact(player);
        // the player picks up the yarn if they have their arms free
        if (player.IsNormal()) {
            GetPickedUp(player);
        }
    }

    private void HideMesh() { mesh.SetActive(false); }

    private void ShowMesh() { mesh.SetActive(true); }

    private void GetPickedUp(Player player) {
        // give the player this yarn
        player.GiveYarn(this);
        
        // this yarn is no longer interactable
        playerInteract.RemoveInteractable(this);
        
        // parent the yarn to the player and position it in their arms
        transform.SetParent(player.transform);
        transform.localPosition = positionYarnInPlayersArms;
        
        print("The player just picked up some yarn.");
    }

    public void GetPutDown(Player player) {
        // return the player to normal, take it from the player
        player.RemoveYarn(this);
        
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
    public void StartYarnLine(Pushpin pushpin) {
        yarnLine = new YarnLine(this, player.playerContactable.GetNewContact(), pushpin.interactContactable.GetNewContact());
    }

    /* Finishes off a line of yarn on the given pushpin. */
    public void FinishYarnLine(Pushpin pushpin) {
        if(mesh) HideMesh();
    }

    /* Untie a line of yarn from a pushpin to continue to edit the line. */
    public void EditYarnLine(Pushpin pushpin) {
        if(mesh) ShowMesh();
        
        // todo: remove the pushpin's contact, add the player
    }

    /* Untie a line of yarn from a pushpin to remove the line. */
    public void UndoYarnLine(Pushpin pushpin) {
        
    }
}