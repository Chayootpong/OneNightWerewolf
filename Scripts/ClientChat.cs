using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientChat : MonoBehaviour {

    public InputField chattext;
    public GameObject content, nwmng;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Chatter()
    {
        nwmng.GetComponent<Client>().SendChat(Memory.playerName + "|" + chattext.text);
        chattext.text = "";
    }

    public void Reader(string name, string text)
    {
        if (name == "DISJOIN")
        {
            content.GetComponent<Text>().text += "\n<color=#ff0000ff>" + text + "</color>";
        }
        else
        {
            content.GetComponent<Text>().text += "\n<b>" + name + ":</b> " + text;
        }
    }
}
