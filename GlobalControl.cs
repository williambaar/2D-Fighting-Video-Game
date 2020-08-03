using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates an instance of an object that stores variables for use throughout multiple scenes.

public class GlobalControl : MonoBehaviour
{

    public State Char1; //Stores character select options
    public State Char2;
    public MapState Map; //Stores map select option
    public int Win = 0; //Stores the winner ID

    public static GlobalControl Instance;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); //Keeps global object from being destroyed between scenes
            Instance = this; //Creates an instance of the global object
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
