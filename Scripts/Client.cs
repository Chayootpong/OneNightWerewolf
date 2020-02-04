using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Client : MonoBehaviour {

    GameObject memory, pymng, gamemng;
    static NetworkClient client;
    StringMessage msg;
    public static int id, voteTarget;
    public static string role;
    bool breaker;

    //TEST
    //public Text testRole;

	void Start () {

        //BASE SETTING
        memory = GameObject.Find("Memory");
        pymng = GameObject.Find("Player Manager");
        gamemng = GameObject.Find("Game Manager");
        client = new NetworkClient();
        msg = new StringMessage();
        id = voteTarget = -1;
        breaker = false;
        Connect();

        //REGIST SET CLIENT
        //USE PORT CODE, CLIENT START AT 1
        //THREE DIGITS LEFTS TYPE LIKES IN MOBILE. SUCH AS ABC = 222
        client.RegisterHandler(1437, SetIDRequest); //CIDR : CLIENT IDREQUEST
        client.RegisterHandler(1242, SetChat); //CCHA : CLIENT CHAT
        client.RegisterHandler(1796, ReceivePlayerName); //CPYN : CLIENT PLAYERNAME
        client.RegisterHandler(1738, SetReveal); //CREV : CLIENT REVEAL
        client.RegisterHandler(1784, ReceiveStartGame); //CSTG : CLIENT STARTGAME
        client.RegisterHandler(1797, ReceivePlayerRole); //CSTG : CLIENT PLAYERROLE
        client.RegisterHandler(1868, SetVote); //CVOT : CLIENT VOTE
        client.RegisterHandler(1929, SetWerewolf); //CWEW : CLIENT WEREWOLF
    }

    public void Connect()
    {
        client.Connect(Memory.ip, int.Parse(Memory.room));
    }

    void Update () {

        if (client.isConnected && id == -1 && !breaker)
        {
            breaker = true;
            SendIDRequest();
        }

        //if (client.isConnected) Debug.Log("Connecting...");
        //else Debug.Log("Disconnecting...");
    }

    //SEND FUNCTIONS

    public void SendIDRequest()
    {
        msg.value = Memory.playerName;
        client.Send(0437, msg);
    }

    public void SendDisJoin()
    {
        msg.value = id.ToString();
        client.Send(0345, msg);
        msg.value = "DISJOIN|" + Memory.playerName + " has disconnected.";
        client.Send(0242, msg);
        Memory.suddenGoto = 2;
        SceneManager.LoadScene("Menu");
    }

    public void SendChat(string text)
    {
        msg.value = text;
        client.Send(0242, msg);
    }

    public void SendReveal(int target)
    {
        msg.value = id + "|" + target;
        client.Send(0738, msg);
    }

    public void SendChangeRole(int target, string changeRole, string command)
    {
        msg.value = target + "|" + changeRole + "|" + command;
        client.Send(0247, msg);
    }

    public void SendExchange(int first, int second)
    {
        msg.value = first + "|" + second;
        client.Send(0392, msg);
    }

    public void SendVote(int vote)
    {
        if (vote > -1)
        {
            msg.value = vote.ToString();
            client.Send(0868, msg);
        }
    }

    public void SendWerewolf()
    {
        msg.value = null;
        client.Send(0929, msg);
    }

    //SET FUNCTIONS

    public void SetIDRequest(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;

        if (id == -1)
        {
            id = int.Parse(msg.value);
        }
    }

    public void SetChat(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        pymng.GetComponent<ClientChat>().Reader(deltas[0], deltas[1]);
    }

    public void SetReveal(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');

        if (id == int.Parse(deltas[0]))
        {
            gamemng.GetComponent<ClientUI>().ShowReveal(deltas[1]);
        }
    }

    public void SetVote(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        ClientUI.result = deltas[id + 1];
        int target = int.Parse(deltas[0]);
        SendReveal(id);
        if (target >= 0) voteTarget = target;
    }

    public void SetWerewolf(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        gamemng.GetComponent<ClientUI>().SetFangWerewolf(deltas);
    }

    //RECEIVE FUNCTIONS

    public void ReceivePlayerName(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        gamemng.GetComponent<ClientGameController>().UpdateName(msg.value);
    }

    public void ReceiveStartGame(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        Timer.mulNight = int.Parse(deltas[0]);
        gamemng.GetComponent<Timer>().SetOrder(deltas);
        gamemng.GetComponent<Timer>().enabled = true;
    }

    public void ReceivePlayerRole(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        if (id == int.Parse(deltas[0])) role = deltas[1];
    }
}
