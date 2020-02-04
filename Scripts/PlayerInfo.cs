using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    //REQUIRE 5 PLAYER
    public static int limitPlayer = GameController.requirePlayerNum;

    //8-11 IS CENTER
    public static string[,] info = new string[11, 10];  //isConnected playerName startRole currentRole

    public static int playerCount;

	void Start () {

        //RESET PLAYER
        for (int i = 0; i < 8; i++)
        {
            DeletePlayer(i);
        }

        info[8, 1] = "Left Center";
        info[9, 1] = "Middle Center";
        info[10, 1] = "Right Center";
        playerCount = 0;
    }
	
	void Update () {
		
	}

    public int AddPlayer(string name)
    {
        int id = -1;

        for (int i = 0; i < limitPlayer; i++)
        {
            if (info[i, 0] == null)
            {
                id = i;
                info[i, 0] = "1";
                info[i, 1] = name;
                Debug.Log("ADD PLAYER : " + info[i, 1] + " (" + id + ")");
                break;
            }
        }

        playerCount++;
        return id;
    }

    public void DeletePlayer(int id)
    {
        Debug.Log("DELETE PLAYER : " + info[id, 1] + " (" + id + ")");

        for (int j = 0; j < 10; j++)
        {
            if (j == 0) info[id, j] = null;
            else info[id, j] = "";
        }
        playerCount--;
    }
}
