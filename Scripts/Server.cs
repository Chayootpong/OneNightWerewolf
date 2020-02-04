using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour {

    StringMessage msg;
    public int room;
    string localIP;
    GameObject pymng, gamemng;

    //OUTPUT
    public Text showIP, showRoom;

    void Start () {

        //BASE SETTING
        pymng = GameObject.Find("Player Manager");
        gamemng = GameObject.Find("Game Manager");
        msg = new StringMessage();
        localIP = LocalIPAddress();
        NetworkServer.Listen(room);

        //REGIST RECEIVE SERVER
        //USE PORT CODE, SERVER START AT 0
        //THREE DIGITS LEFTS TYPE LIKES IN MOBILE. SUCH AS ABC = 222
        NetworkServer.RegisterHandler(0437, ReceiveIDRequest); //SIDR : SERVER IDREQUEST
        NetworkServer.RegisterHandler(0345, ReceiveDisJoin); //SDIJ : SERVER DISJOIN
        NetworkServer.RegisterHandler(0242, ReceiveChat); //SCHA : SERVER CHAT
        NetworkServer.RegisterHandler(0738, ReceiveReveal); //SREV : SERVER REVEAL
        NetworkServer.RegisterHandler(0247, ReceiveChangeRole); //SCHR : SERVER CHANGEROLE
        NetworkServer.RegisterHandler(0392, ReceiveExchange); //SEXC : SERVER EXCHANGE
        NetworkServer.RegisterHandler(0868, ReceiveVote); //SVOT : SERVER VOTE
        NetworkServer.RegisterHandler(0929, ReceiveWerewolf); //SWEW : SERVER WEREWOLF
    }
	
	void Update () {

        showIP.text = localIP;
        showRoom.text = room.ToString();		
	}

    //RECEIVE FUNCTIONS

    public void ReceiveIDRequest(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        ReturnIDRequest(pymng.GetComponent<PlayerInfo>().AddPlayer(msg.value));
    }

    public void ReceiveDisJoin(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        pymng.GetComponent<PlayerInfo>().DeletePlayer(int.Parse(msg.value));
    }

    public void ReceiveChat(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        ReturnChat(msg.value);
    }

    public void ReceiveReveal(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        int target = int.Parse(deltas[1]);
        Debug.Log(PlayerInfo.info[target, 1] + "'s role is " + PlayerInfo.info[target, 3]);
        ReturnReveal(deltas[0], PlayerInfo.info[target, 3]);
    }

    public void ReceiveChangeRole(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        int target = int.Parse(deltas[0]);

        if (deltas[2] == "Doppleganger" || deltas[2] == "Alpha" || deltas[2] == "Copycat")
        {
            PlayerInfo.info[target, 2] = deltas[1];
        }
        PlayerInfo.info[target, 3] = deltas[1];

        gamemng.GetComponent<GameController>().SelectTeam(target);
        Debug.Log(PlayerInfo.info[target, 1] + " changes to " + PlayerInfo.info[target, 3]);
    }

    public void ReceiveExchange(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        int first = int.Parse(deltas[0]);
        int second = int.Parse(deltas[1]);
        string temp = PlayerInfo.info[first, 3];
        PlayerInfo.info[first, 3] = PlayerInfo.info[second, 3];
        PlayerInfo.info[second, 3] = temp;
        gamemng.GetComponent<GameController>().SelectTeam(first);
        gamemng.GetComponent<GameController>().SelectTeam(second);
        Debug.Log(PlayerInfo.info[first, 1] + " becomes " + PlayerInfo.info[first, 3] + " (Before : " + PlayerInfo.info[second, 3] + ")");
        Debug.Log(PlayerInfo.info[second, 1] + " becomes " + PlayerInfo.info[second, 3] + " (Before : " + PlayerInfo.info[first, 3] + ")");
    }

    public void ReceiveVote(NetworkMessage message)
    {
        msg.value = message.ReadMessage<StringMessage>().value;
        int selected = int.Parse(msg.value);
        GameController.voteSlot[selected]++;
        if (!GameController.isReadyToCheckResult) gamemng.GetComponent<GameController>().InvokeDelay();
    }

    public void ReceiveWerewolf(NetworkMessage message)
    {
        ReturnWerewolf();
    }

    //RETURN FUNCTIONS

    public void ReturnIDRequest(int id)
    {
        if (id != -1)
        {
            msg.value = id.ToString();
            NetworkServer.SendToAll(1437, msg);
        }
    }

    public void ReturnChat(string chat)
    {
        msg.value = chat.ToString();
        NetworkServer.SendToAll(1242, msg);
    }

    public void ReturnReveal(string id, string role)
    {
        msg.value = id + "|" + role;
        NetworkServer.SendToAll(1738, msg);
    }

    public void ReturnVote(int index, int res)
    {
        msg.value = index.ToString();

        for (int i = 0; i < 8; i++)
        {
            gamemng.GetComponent<GameController>().ResultTeam(i);
        }

        for (int i = 0; i < 8; i++)
        {
            if (res == 1) //VILLAGER WINS
            {
                if (PlayerInfo.info[i, 0] == "3") msg.value += "|Win";
                else msg.value += "|Lose";
            }

            else if (res == 4) //APOCALYPSE WINS
            {
                if (PlayerInfo.info[i, 0] == "4") msg.value += "|Win";
                else msg.value += "|Lose";
            }

            else //2 && 3
            {
                if (PlayerInfo.info[i, 0] == "3" || PlayerInfo.info[i, 0] == "4") msg.value += "|Lose";
                else msg.value += "|Win";
            }
        }

        Debug.Log(msg.value);
        NetworkServer.SendToAll(1868, msg);
    }

    public void ReturnWerewolf()
    {
        int count = 0;
        msg.value = null;

        for (int i = 0; i < 8; i++)
        {
            if (PlayerInfo.info[i, 0] == "1")
            {
                msg.value += 1 + "|";
                count++;
            }
            else msg.value += 0 + "|";
        }
        msg.value += count;
        NetworkServer.SendToAll(1929, msg);
    }

    //SEND FUNCTIONS

    public void SendPlayerName()
    {
        msg.value = null;
        for (int i = 0; i < 7; i++)
        {
            msg.value += PlayerInfo.info[i, 1] + "|";
        }
        msg.value += PlayerInfo.info[7, 1];
        NetworkServer.SendToAll(1796, msg);
    }

    public void SendStartGame(string order)
    {
        msg.value = order;
        NetworkServer.SendToAll(1784, msg);
    }

    public void SendPlayerRole(string role)
    {
        msg.value = role;
        NetworkServer.SendToAll(1797, msg);
    }

    //GET LOCAL ID

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                //break;
            }
        }
        return localIP;
    }
}
