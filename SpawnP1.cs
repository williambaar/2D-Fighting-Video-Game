using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to spawn the chosen character for player 1.
//There is a script called SpawnP2 that does the same for the second player.

public class SpawnP1 : MonoBehaviour
{

    [SerializeField] GameObject[] playerones; //Stores all character prefabs
    [SerializeField]
    private Transform spawn; //Stores spawning position

    // Start is called before the first frame update
    void Start()
    {
        int charChoice = GlobalControl.Instance.Char1.GetIDNum(); //Gets the ID number for the caracter selected
        Instantiate(playerones[charChoice-1], spawn.position, spawn.rotation); //Spawns character prefab based on ID num
    }

}
