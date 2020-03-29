using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public enum State
    {
        Normal,
        Won,
        Lost
    }

    private State state;
    
    private GameObject[] pushpinArray;
    private GameObject[] yarnArray;
    public HashSet<Contactable> allContactables;

    public RectTransform levelCompleteUI;
    public RectTransform levelFailedUI;

    // private int push_pin_count = GameManager.getPushPinCount();
    // private int push_pin_complete_count;
    // private int yarn_string_state;

    // Start is called before the first frame update
    void Awake() {
        LevelNormal();
        
        pushpinArray = GameObject.FindGameObjectsWithTag("Pushpin");
        yarnArray = GameObject.FindGameObjectsWithTag("Yarn");

        allContactables = new HashSet<Contactable>(FindObjectsOfType<Contactable>());
        //Util.PrintList(allContactables);
        //levelCompleteUI.gameObject.SetActive(true);
    }

    

    // Update is called once per frame
    void Update()
    {
        // push_pin_count = GameManager.push_pins_complete.count;
        // yarn_string_state = GameManager.yarn_string.state;

        // check if level is complete from game objects
        // int x=0;

        // if(push_pin_count == push_pin_complete_count)
        //  { levelComplete();}

        // if(push_pin_count != push_pin_complete_count)
        //  { disappearLevel();}

        // if(yarn_string_state == 0 || player_stuck == true)
        //  { levelFailed();}
    }

    void CheckLevelStatus()
    {
        // Check if the yarn is destroyed
        bool yarnDestroyed = false;
        foreach (GameObject obj in yarnArray)
        {
            if (obj.GetComponent<Yarn>().IsDestroyed())
            {
                yarnDestroyed = true;
            }
        }
        if (yarnDestroyed)
        {
            LevelFailed();
        }


        // Check if the pushpins are tied
        bool pushpinsDone = true;
        foreach (GameObject obj in pushpinArray) {
            if (!obj.GetComponent<Pushpin>().IsDone())
                pushpinsDone = false;
        }
        if (pushpinsDone) LevelComplete();
        else LevelNormal();
    }

    void LevelComplete() {
        state = State.Won;
        if (levelCompleteUI != null) levelCompleteUI.gameObject.SetActive(true);
        // Display "Level Complete"
        // Probably calling a gameManager function
        // The level complete function must show buttons to continue or go back to level
        // Once you beat a level, if the next level is still locked, unlock it.  Else, do nothing (if statement)
        // Button nextLevel will let player go to next level
        // Alternatively, go back to hub world
    }

    void LevelFailed()
    {
        state = State.Lost;
        if (levelFailedUI != null) levelFailedUI.gameObject.SetActive(true);
        

        // Display "Level Failed"
        // Probably calling a gameManager function
        // Send data that level is complete to game manager
        // UI:  prompt for redo, call level reset
        // Alternatively, go back to hub world
    }

    void LevelNormal()
    {
        if (levelCompleteUI != null) levelCompleteUI.gameObject.SetActive(false);
        if (levelFailedUI != null) levelFailedUI.gameObject.SetActive(false);

        state = State.Normal;

        // If showing a display, turn off display and continue game
        // Probably calling a gameManager function
    }


}
