
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int normalState  = 1;
    public int wonState     = 0;
    public int lostState    = 0;

    // private int push_pin_count = GameManager.push_pins.count;
    // private int push_pin_complete_count;
    // private int yarn_string_state;

    // Start is called before the first frame update
    void Start()
    {
        
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

    void levelComplete()
    {
        normalState = 0;
        wonState = 1;
        lostState = 0;
        // Display "Level Complete"
        // Probably calling a gameManager function
    }

    void levelFailed()
    {
        normalState = 0;
        wonState = 0;
        lostState = 1;

        // Display "Level Failed"
        // Probably calling a gameManager function
    }

    void disappearLevel()
    {
        normalState = 1;
        wonState = 0;
        lostState = 0;

        // If showing a display, turn off display and continue game
        // Probably calling a gameManager function
    }

    bool playerStuck()
    {
        // Check if the player can connect yarn to anything?
        // Not really sure how to check if the player is out of moves
        // Might not really need this function if it's too hard to implement
        return false;
    }


}
