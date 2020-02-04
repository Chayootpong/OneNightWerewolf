using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour {

    public static string ip, room, playerName;
    public static int suddenGoto;

	// Use this for initialization
	void Start () {

        suddenGoto = -1;		
	}
	
	// Update is called once per frame
	void Update () {

        DontDestroyOnLoad(gameObject);
    }
}
