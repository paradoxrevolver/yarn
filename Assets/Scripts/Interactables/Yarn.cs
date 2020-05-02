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
            linePoints.AddRange(yarnLine.renderPoints);
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

    /* A Player picks up this Yarn. */
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

    /* A Player puts down this Yarn. */
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
    
    /* Starts a line of yarn between a Player and a Pushpin. */
    public void StartYarnLine(Player player, Pushpin pushpin) {
        yarnLine = new YarnLine(this, player.playerContactable.GetNewContact(), pushpin.interactContactable.GetNewContact());
    }

    /* Finishes off this Yarn's line on the given Pushpin. */
    public void FinishYarnLine(Pushpin pushpin) {
        if(mesh) HideMesh();
        yarnLine.SetTail(pushpin.interactContactable.GetNewContact());
    }

    /* Unties this Yarn's line from a Pushpin to allow a Player to edit the line. */
    public void EditYarnLine(Pushpin pushpin, Player player) {
        if(mesh) ShowMesh();
        
        // add the player on the same side of the YarnLine that the given Pushpin is at
        if (yarnLine.GetHead().gameObject.Equals(pushpin.gameObject)) {
            yarnLine.SetHead(player.playerContactable.GetNewContact());
        } else if (yarnLine.GetTail().gameObject.Equals(pushpin.gameObject)) {
            yarnLine.SetTail(player.playerContactable.GetNewContact());
        } else {
            throw new Exception($"{this} was told to edit its YarnLine, but was given a Pushpin that isn't on either side of the YarnLine!");
        }
    }

    /* Undoes this Yarn's line from a Pushpin to remove the line. */
    public void UndoYarnLine(Pushpin pushpin) {
        yarnLine.Clear();
        yarnLine = null;
    }
}