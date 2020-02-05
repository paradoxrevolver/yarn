using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Door : Interactable {
    public string goToLevel;
    public Text doorText;
    public override void Interact() {
        SceneManager.LoadScene(goToLevel);
    }
    private void Awake()
    {
        doorText.text = goToLevel;
    }
}