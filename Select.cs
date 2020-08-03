using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is used to change the states on the character select screen on input so that
// characters and maps can be selected from sets of 3.

public class Select : MonoBehaviour
{
    
    [SerializeField] Image Char1; //Stores icon for first player
    [SerializeField] Image Char2; //Stores icon for second player
    [SerializeField] Image MapIm; //Stores image for map
    [SerializeField] State startState; //Initializes the character states
    [SerializeField] MapState startMap; //Initializes the map state

    State state1; //State for first player
    State state2; //State for second player
    MapState mp; //State for map

    // Start is called before the first frame update
    void Start()
    {
        state1 = startState;
        state2 = startState;
        mp = startMap;
        Char1.GetComponent<Image>().sprite = state1.GetStateSprite(); //These put the proper images on the select screen
        Char2.GetComponent<Image>().sprite = state2.GetStateSprite();
        MapIm.GetComponent<Image>().sprite = mp.GetStateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
        SaveChoice();
    }

    private void ManageState() //Changes the states based on player input
    {
        var nextStates = state1.GetNextStates(); //Gets list of available character states for P1

        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Left Pressed");
            state1 = nextStates[1];
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Right Pressed");
            state1 = nextStates[0];
        }

        nextStates = state2.GetNextStates(); //Gets list of available character states for P2

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("A Pressed");
            state2 = nextStates[1];
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("D Pressed");
            state2 = nextStates[0];
        }

        var mapnextStates = mp.GetNextStates(); //Gets list of available map states

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R Pressed");
            mp = mapnextStates[0];
        }

        Char1.GetComponent<Image>().sprite = state1.GetStateSprite(); //Updates the images on the screen based on current state
        Char2.GetComponent<Image>().sprite = state2.GetStateSprite();
        MapIm.GetComponent<Image>().sprite = mp.GetStateSprite();
    }

    private void SaveChoice() //Saves the states to the global object
    {
        GlobalControl.Instance.Char1 = state1;
        GlobalControl.Instance.Char2 = state2;
        GlobalControl.Instance.Map = mp;
    }
}
