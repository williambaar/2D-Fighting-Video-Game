using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is used to create and return the attributes of a MapState object
//MapStates are used to change and determine the maps from the character select screen

[CreateAssetMenu(menuName = "MapState")] //Creates option to create a MapState object
public class MapState : ScriptableObject
{
    [SerializeField] Sprite charsprite; //Stores image for select screen
    [SerializeField] MapState[] nextStates; //Stores other states to trasistion to
    [SerializeField] int ID; //ID number to track which state is currently active

    public Sprite GetStateSprite()
    {
        return charsprite;
    }

    public int GetStateID()
    {
        return ID;
    }

    public MapState[] GetNextStates()
    {
        return nextStates;
    }
}
