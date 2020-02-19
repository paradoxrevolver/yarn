using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    //public UIManager UI;
    //GameObject[] PauseComponents;
    //GameObject PauseMenuPointer;
    //bool trueFalse;
    //gameObject is Pause Menu that this script is attached to

    // Start is called before the first frame update
    void Start()
    {
        //PauseComponents = GameObject.FindGameObjectsWithTag("PauseMenu");
        //PauseMenuPointer = gameObject;
        /*for (int i = 0; i < PauseComponents.Length; i++)
        {
            PauseComponents[i].SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        Debug.Log("PauseToggled");
        //if (PauseComponents.GetComponent<"PauseMenu">().enabled = true)
        //{

        //    GetComponent<PauseMenuScript>().enabled = false;
        //    GetComponentInChildren<PauseMenuScript>().enabled = false;
        //    Time.timeScale = 1.0f;
        //}
        //else
        //{
        //    GetComponent<PauseMenuScript>().enabled = true;
        //    GetComponentInChildren<PauseMenuScript>().enabled = true;
        //    Time.timeScale = 0f;
        //}
    }


}
