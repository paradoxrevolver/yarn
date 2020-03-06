using UnityEngine.SceneManagement;

public class Door : Interactable {
    public string goToLevel;
    public override void Interact(PlayerManager player) {
        SceneManager.LoadScene(goToLevel);
    }
}