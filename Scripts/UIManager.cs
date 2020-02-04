using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using WerewolfAPI;
//using WerewolfClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject[] canvas;
    public GameObject pymng, memory;

    //LOGIN
    public InputField[] login;
    public string log;
    public Text logtext;

    //MENU
    public string pyname;
    public Text pytext;

    //JOIN GAME
    public InputField ip, room;
    public GameObject loading;

	// Use this for initialization
	void Start () {

        if (!GameObject.Find("Memory(Clone)"))
        {
            Instantiate(memory, transform.position, transform.rotation);
        }
        login[1].contentType = InputField.ContentType.Password;
        pyname = log = "";
	}
	
	// Update is called once per frame
	void Update () {

        pytext.text = Memory.playerName;
        logtext.text = log;

        Memory.ip = ip.text;
        Memory.room = room.text;

        if (Memory.suddenGoto != -1)
        {
            Goto(Memory.suddenGoto);
            Memory.suddenGoto = -1;
        }
    }

    public void Goto(int index)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            if (i != index) canvas[i].SetActive(false);
            else canvas[i].SetActive(true);
        }
    }

    public void SignUp()
    {
        //pymng.GetComponent<WerewolfModel>().SignUp("http://project-ile.net:23416/werewolf/", login[0].text, login[1].text);
    }

    public void SignIn()
    {
        pyname = Memory.playerName = login[0].text;
        Goto(2);
        //pymng.GetComponent<WerewolfModel>().SignIn("http://project-ile.net:23416/werewolf/", login[0].text, login[1].text);
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("GamePlayer");
    }

    public void Cards()
    {
        SceneManager.LoadScene("CardInfo");
    }

    public void Stats()
    {
        SceneManager.LoadScene("Stats");
    }

}
