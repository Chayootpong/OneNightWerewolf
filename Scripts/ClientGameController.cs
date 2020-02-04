using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGameController : MonoBehaviour {

    public Text[] pynameList;
    GameObject nwmng;
    int target, exTarget;
    public static int vote = -1;
    public static string action;

    //STORAGE ACTION
    public static string revealRole;

    //CLIENT UI PARTS
    public GameObject voteBTN;

    void Start () {

        nwmng = GameObject.Find("Network Manager");
        ClearAction();
	}
	
	void Update () {

        if (action == "Reveal" && target != -1)
        {
            ClearAction();
            Reveal(target);
            if (Client.role == "Witch")
            {
                exTarget = target;
            }
        }

        else if (action == "Transform" && target != -1) //FOR ALPHA, ONLY ALPHA HAVE TRANSFORM ABILITY (DOPPLEGANGER DOESN'T USE TRANSFORM)
        {
            ClearAction();
            ChangeRole(target, "Werewolf", Client.role);
        }

        else if (action == "Preexchange" && target != -1)
        {
            exTarget = target;
            ClearAction();

            if (Client.role == "Usurpator" || Client.role == "Repose")
            {
                Exchange(exTarget, Client.id);
            }
            else action = "Exchange";
        }

        else if (action == "Exchange" && target != -1)
        {
            ClearAction();
            Exchange(exTarget, target);
        }

        else if (action == "Vote" && target != -1)
        {
            vote = target;
            ClearAction();
            voteBTN.SetActive(false);
        }

        else if (action.Substring(0, action.Length - 1) == "SubReveal")
        {
            ClientUI.numCard = int.Parse(action.Replace(action.Substring(0, action.Length - 1), ""));
            action = "Reveal";
        }
	}

    public void UpdateName(string nameList)
    {
        string[] deltas = nameList.Split('|');
        for (int i = 0; i < 8; i++)
        {
            pynameList[i].text = deltas[i];
        }
    }

    public void UseAction(string act)
    {
        action = act;
        GetComponent<ClientUI>().maxBlur.SetActive(false);
    }

    public void Reveal(int target)
    {
        nwmng.GetComponent<Client>().SendReveal(target);
    }

    public void ChangeRole(int target, string changeRole, string command)
    {
        nwmng.GetComponent<Client>().SendChangeRole(target, changeRole, command);
    }

    public void Exchange(int first, int second)
    {
        nwmng.GetComponent<Client>().SendExchange(first, second);

        //FOR USURPATOR
        if (Client.role == "Usurpator") Reveal(Client.id);
    }

    public void SendSelectedVote()
    {
        nwmng.GetComponent<Client>().SendVote(vote);
    }

    public void SetTarget(int selected)
    {
        target = selected;
    }

    public void ClearAction()
    {
        target = -1;
        action = "";
    }

    public void ClearVote()
    {
        ClearAction();
        vote = -1;
        action = "Vote";
        voteBTN.SetActive(true);
    }

    public void SendCheckWerewolf()
    {
        nwmng.GetComponent<Client>().SendWerewolf();
    }
}
