using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static int requirePlayerNum = 3, nightActionCount = 0;
    public string[] selectedPattern;
    public static int[] voteSlot = new int[8];
    public static bool isReadyToCheckResult;
    bool isSetStartRole;
    string orderPattern;
    public GameObject nwmng;

    public static string[] order = { "Copycat", "Doppleganger", "Werewolf", "Alpha", "Mystic", "Minion", "Seer", "Investigator",
                                     "Usurpator", "Witch", "Troublemaker", "Repose" };

    public static string[] werewolfTeam = { "Werewolf", "Alpha", "Mystic", "Dream Wolf" };
    public static string[] werewolfSup = { "Minion" };
    public static string[] villagerTeam = { "Usurpator", "Doppleganger", "Witch", "Repose", "Copycat", "Seer", "Investigator", "Troublemaker", "Villager" };
    public static string[] outlawTeam = { "Apocalypse" };

    public static string[][] role3 = new string[3][];
    public static string[] type3_1 = { "Usurpator", "Minion", "Doppleganger", "Alpha", "Mystic", "Witch" };
    public static string[] type3_2 = { "Usurpator", "Apocalypse", "Doppleganger", "Alpha", "Witch", "Dream Wolf" };
    public static string[] type3_3 = { "Werewolf", "Werewolf", "Usurpator", "Apocalypse", "Repose", "Doppleganger" };

    public static string[][] role5 = new string[3][];
    public static string[] type5_1 = { "Werewolf", "Seer", "Repose", "Mystic", "Apocalypse", "Investigator", "Witch", "Troublemaker" };
    public static string[] type5_2 = { "Werewolf", "Usurpator", "Troublemaker", "Doppleganger", "Alpha", "Witch", "Copycat", "Villager" };
    public static string[] type5_3 = { "Seer", "Troublemaker", "Apocalypse", "Copycat", "Alpha", "Mystic", "Investigator", "Witch" };


    void Start () {

        role3[0] = type3_1; role3[1] = type3_2; role3[2] = type3_3;
        role5[0] = type5_1; role5[1] = type5_2; role5[2] = type5_3;

        if (requirePlayerNum <= 3) selectedPattern = Shuffle(role3[Random.Range(0, role3.Length)]);
        else if (requirePlayerNum == 5) selectedPattern = Shuffle(role5[Random.Range(0, role5.Length)]);

        bool isFlag = false;

        for (int i = 0; i < order.Length; i++)
        {
            for (int j = 0; j < selectedPattern.Length; j++)
            {
                if (order[i] == selectedPattern[j] && !isFlag)
                {
                    nightActionCount++;
                    orderPattern += "|" + order[i];
                    isFlag = true;
                }              
            }

            isFlag = false;
        }

        isSetStartRole = false;
        isReadyToCheckResult = false;
    }
	
	void Update () {

        if (PlayerInfo.playerCount == requirePlayerNum && !isSetStartRole)
        {
            isSetStartRole = true;

            Debug.Log("START GAME!!!");

            selectedPattern = Shuffle(selectedPattern);

            int order = 0;

            for (int i = 0; i < requirePlayerNum; i++)
            {
                PlayerInfo.info[i, 2] = PlayerInfo.info[i, 3] = selectedPattern[order];
                SelectTeam(i);
                voteSlot[i] = 0;
                nwmng.GetComponent<Server>().SendPlayerRole(i + "|" + PlayerInfo.info[i, 2]);
                order++;
                //Debug.Log(PlayerInfo.info[i, 1] + " plays as " + PlayerInfo.info[i, 2]);
            }

            for (int i = 8; i < 11; i++)
            {
                PlayerInfo.info[i, 2] = PlayerInfo.info[i, 3] = selectedPattern[order];
                order++;
                //Debug.Log(PlayerInfo.info[i, 1] + " is " + PlayerInfo.info[i, 2]);
            }

            nwmng.GetComponent<Server>().SendPlayerName();
            nwmng.GetComponent<Server>().SendStartGame(nightActionCount + orderPattern);
        }		
	}

    public string[] Shuffle(string[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            Swap(list, i, Random.Range(0, list.Length));
        }
        return list;
    }

    public void Swap(string[] arr, int x, int y)
    {
        string temp = arr[x];
        arr[x] = arr[y];
        arr[y] = temp;
    }

    public void SelectTeam(int index)
    {
        //WEREWOLF TEAM = 1, WEREWOLF SUPPORT = 2, VILLAGER TEAM = 3, OUTLAW TEAM = 4
        for (int i = 0; i < werewolfTeam.Length; i++) if (PlayerInfo.info[index, 2] == werewolfTeam[i]) PlayerInfo.info[index, 0] = "1";
        for (int i = 0; i < werewolfSup.Length; i++) if (PlayerInfo.info[index, 2] == werewolfSup[i]) PlayerInfo.info[index, 0] = "2";
        for (int i = 0; i < villagerTeam.Length; i++) if (PlayerInfo.info[index, 2] == villagerTeam[i]) PlayerInfo.info[index, 0] = "3";
        for (int i = 0; i < outlawTeam.Length; i++) if (PlayerInfo.info[index, 2] == outlawTeam[i]) PlayerInfo.info[index, 0] = "4";
    }

    public void ResultTeam(int index)
    {
        //WEREWOLF TEAM = 1, WEREWOLF SUPPORT = 2, VILLAGER TEAM = 3, OUTLAW TEAM = 4
        for (int i = 0; i < werewolfTeam.Length; i++) if (PlayerInfo.info[index, 3] == werewolfTeam[i]) PlayerInfo.info[index, 0] = "1";
        for (int i = 0; i < werewolfSup.Length; i++) if (PlayerInfo.info[index, 3] == werewolfSup[i]) PlayerInfo.info[index, 0] = "2";
        for (int i = 0; i < villagerTeam.Length; i++) if (PlayerInfo.info[index, 3] == villagerTeam[i]) PlayerInfo.info[index, 0] = "3";
        for (int i = 0; i < outlawTeam.Length; i++) if (PlayerInfo.info[index, 3] == outlawTeam[i]) PlayerInfo.info[index, 0] = "4";
    }

    public void InvokeDelay()
    {
        isReadyToCheckResult = true;
        Invoke("CheckResult", 3f);
    }

    public void CheckResult()   //CHECK NOT GOOD
    {
        int maxIndex = -1, maxValue = 0, type = 0;

        for (int i = 0; i < 8; i++)
        {
            if (maxValue < voteSlot[i])
            {
                maxIndex = i;
            }
        }

        //TRY CATCH
        if (maxIndex != -1)
        {
            if (PlayerInfo.info[maxIndex, 0] == "") type = 0;
            else type = int.Parse(PlayerInfo.info[maxIndex, 0]);
        }
        else type = 0;

        nwmng.GetComponent<Server>().ReturnVote(maxIndex, type);
    }
}
