using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : Interactable {
    public string goToLevel;
    public Text doorText;
    public override void Interact(PlayerManager player)
    {
        SceneManager.LoadScene(goToLevel);
    }
    protected override void Awake()
    {
        base.Awake();
        doorText.text = goToLevel;
    }
}