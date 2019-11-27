
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

    // private int push_pin_count = GameManager.getPushPinCount();
    // private int push_pin_complete_count;
    // private int yarn_string_state;

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Normal;
        
        pushpinArray = GameObject.FindGameObjectsWithTag("Pushpin");
        yarnArray = GameObject.FindGameObjectsWithTag("Yarn");

     
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
    }

    void LevelComplete()
    {
        state = State.Won;
        // Display "Level Complete"
        // Probably calling a gameManager function
    }

    void LevelFailed()
    {
        state = State.Lost;

        // Display "Level Failed"
        // Probably calling a gameManager function
    }

    void LevelNormal()
    {
        state = State.Normal;

        // If showing a display, turn off display and continue game
        // Probably calling a gameManager function
    }


}
