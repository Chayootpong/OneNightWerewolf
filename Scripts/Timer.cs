using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public int beforeNight, nightTime, beforeVote, afterVote;
    public static int mulNight = 0;
    int tw, elapsedtw, currenttw;
    public Text showTime, logTime, showSubTime, subLogTime;
    public string[] order;

    public static string roleTurn;
    public static int roleTime, roleIndex;

    GameObject gamemng;

    //SHOW ACTION
    public GameObject nightBlock;
    bool isCheckWerewolf;

    //SET BUTTON
    public GameObject[] pyButton, shade;
    public GameObject voteBTN;

    //SHOW START GAME
    public GameObject[] startAssets;

    void Start () {

        gamemng = GameObject.Find("Game Manager");
        elapsedtw = (int)Time.time;
        currenttw = roleIndex = 0;
        roleTime = 14; //15-1
        isCheckWerewolf = false;
        nightTime = nightTime * mulNight;
        beforeVote = 60;
        showTime.text = beforeNight / 60 + " : " + (beforeNight % 60).ToString("00");
        startAssets[0].SetActive(true);
        //Invoke("InvokeAt1", 1f);
        Invoke("InvokeAt2", 1f);
        Invoke("InvokeAt3", 2f);
    }

    void Update () {

        tw = (int)Time.time;

        if (currenttw != elapsedtw - tw)
        {
            currenttw = elapsedtw - tw;
            ChangeTime();
        }

        if (showTime.text[0] == '0' && showTime.text[4] == '0')
        {
            showTime.color = logTime.color = new Color32(231, 47, 58, 255);
        }
        else showTime.color = logTime.color = Color.white;

        SetButton();
    }

    public void ChangeTime()
    {
        //BASE CASE FIRST PLAYER'S ROLE
        if (beforeNight == 0)
        {
            if (Client.role == "Copycat")
            {
                nightBlock.SetActive(false);
                gamemng.GetComponent<ClientUI>().ShowAction(Client.role);
            }
            else nightBlock.SetActive(true);
            
            beforeNight = -1;
        }

        //START VOTE
        if (beforeVote == 0)
        {
            ClientGameController.action = "Vote";
            beforeVote = -1;
        }

        //SEND VOTE RESULT
        if (afterVote == -1)
        {
            gamemng.GetComponent<ClientGameController>().SendSelectedVote();
            afterVote = -2;
        }

        //TIME RUN
        if (beforeNight >= 0)
        {
            showTime.text = beforeNight / 60 + " : " + (beforeNight % 60).ToString("00");
            beforeNight -= 1;
            logTime.text = "Before Night";
        }

        else if (nightTime > 0)
        {
            showTime.text = nightTime / 60 + " : " + (nightTime % 60).ToString("00");
            showSubTime.text = (roleTime + 1).ToString("00");
            nightTime -= 1;
            logTime.text = "Before Dawn";
            roleTurn = order[roleIndex];

            if (roleTime == 0 && roleIndex >= 0)
            {
                roleIndex++;
                roleTime = 14; //Manual Set 15-1
                Invoke("DelayBlock", 1f);
            }
            else
            {
                roleTime--;
                isCheckWerewolf = false;
            }
        }

        else if (beforeVote > 0)
        {
            roleTurn = null;
            nightBlock.SetActive(false);
            showSubTime.text = "";
            showTime.text = beforeVote / 60 + " : " + (beforeVote % 60).ToString("00");
            beforeVote -= 1;
            logTime.text = "Before Vote";
        }

        else if (afterVote > -1)
        {
            voteBTN.SetActive(true);
            showTime.text = afterVote / 60 + " : " + (afterVote % 60).ToString("00");
            afterVote -= 1;
            logTime.text = "Voting !!!";
        }
    }

    public void SetOrder(string[] arr)
    {
        order = new string[mulNight];

        for (int i = 0; i < order.Length; i++)
        {
            order[i] = arr[i + 1];
        }
    }

    public void DelayBlock()
    {
        if (order[roleIndex] != Client.role) nightBlock.SetActive(true);
        else
        {
            nightBlock.SetActive(false);
            gamemng.GetComponent<ClientUI>().ShowAction(Client.role);
        }

        if (!isCheckWerewolf)
        {
            isCheckWerewolf = true;
            gamemng.GetComponent<ClientGameController>().SendCheckWerewolf();
        }
    }

    public void SetButton()
    {
        int p_start = 0, p_end = 7, c_start = 8, c_end = 10;
        
        if (nightTime > 0) //MAIN NIGHT
        {
            if ((order[roleIndex] == "Doppleganger" && ClientGameController.action == "Reveal") || //SELECT PLAYER WITHOUT YOURSELF
            (order[roleIndex] == "Mystic" && ClientGameController.action == "Reveal") ||
            (order[roleIndex] == "Alpha" && ClientGameController.action == "Transform") ||
            (order[roleIndex] == "Witch" && ClientGameController.action == "Exchange") ||
            (order[roleIndex] == "Usurpator" && ClientGameController.action == "Preexchange") ||
            (order[roleIndex] == "Investigator" && ClientGameController.action == "Reveal") ||
            (order[roleIndex] == "Troublemaker" && ClientGameController.action == "Preexchange") ||
            (order[roleIndex] == "Troublemaker" && ClientGameController.action == "Exchange") ||
            (order[roleIndex] == "Seer" && ClientGameController.action == "Reveal" && ClientUI.numCard == 2))
            {
                p_start = 0; p_end = 4; //END = all player - 1, 10 for center (Manual)
                c_start = -1; c_end = -1; //NOT USE = -1
                subLogTime.text = "Select Player";
            }
            else if ((order[roleIndex] == "Werewolf" && ClientGameController.action == "Reveal") || //SELECT CENTER
                     (order[roleIndex] == "Witch" && ClientGameController.action == "Reveal") ||
                     (order[roleIndex] == "Repose" && ClientGameController.action == "Preexchange") ||
                     (order[roleIndex] == "Seer" && ClientGameController.action == "Reveal" && ClientUI.numCard == 1) ||
                     (order[roleIndex] == "Copycat" && ClientGameController.action == "Reveal"))
            {
                p_start = -1; p_end = -1;
                c_start = 8; c_end = 10;
                subLogTime.text = "Select Card";
            }          
            else
            {
                p_start = -1; p_end = -1;
                c_start = -1; c_end = -1;
                subLogTime.text = "";
            }
        }

        else if (beforeVote >= 0)
        {
            p_start = -1; p_end = -1; 
            c_start = -1; c_end = -1;
        }

        else if (afterVote >= 0) //MAIN VOTE
        {
            if (ClientGameController.action == "Vote") //SELECT PLAYER WITHOUT YOURSELF
            {
                p_start = 0; p_end = 4; //END = all player - 1, 10 for center (Manual)
                c_start = -1; c_end = -1; //NOT USE = -1
                subLogTime.text = "Vote";
            }
            else
            {
                p_start = -1; p_end = -1;
                c_start = -1; c_end = -1;
                subLogTime.text = "Revote";
            }
        }
        
        else
        {
            p_start = -1; p_end = -1;
            c_start = -1; c_end = -1;
            voteBTN.SetActive(false);
            subLogTime.text = "";
        }

        for (int i = 0; i < 11; i++)
        {
            if ((i >= p_start && i <= p_end && i != Client.id) || (i >= c_start && i <= c_end))
            {
                pyButton[i].GetComponent<Button>().enabled = true;
                shade[i].SetActive(true);
            }
            else
            {
                pyButton[i].GetComponent<Button>().enabled = false;
                shade[i].SetActive(false);
            }
        }
    }

    public void InvokeAt1()
    {
        startAssets[1].SetActive(true);
    }

    public void InvokeAt2()
    {
        startAssets[2].SetActive(true);
    }

    public void InvokeAt3()
    {
        for (int i = 0; i < 2; i++)
        {
            startAssets[i].SetActive(false);
        }
        gamemng.GetComponent<ClientGameController>().Reveal(Client.id);
    }
}
