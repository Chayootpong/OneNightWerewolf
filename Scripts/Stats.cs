using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    static public Stats hInstance;

    private void Start()
    {
        if (hInstance == null)
            hInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Load Statistic Scene
    /// </summary>
    public void Load()
    {
        SceneManager.LoadScene("Stats");
    }

    /// <summary>
    /// Saving scores of each factions
    /// </summary>
    /// <param name="fileName">Name of faction (villagerStat, werewolfStat, othersStat)</param>
    static public void SetScore(string role, string result)
    {
        string fileName = "";
        if (role == "Usurpator") fileName = "villagerStat";
        else if (role == "Minion") fileName = "werewolfStat";
        else if (role == "Doppleganger") fileName = "villagerStat";
        else if (role == "Alpha") fileName = "werewolfStat";
        else if (role == "Mystic") fileName = "werewolfStat";
        else if (role == "Witch") fileName = "villagerStat";
        else if (role == "Apocalypse") fileName = "othersStat";
        else if (role == "Dream Wolf") fileName = "werewolfStat";
        else if (role == "Werewolf") fileName = "werewolfStat";
        else if (role == "Repose") fileName = "villagerStat";
        else if (role == "Villager") fileName = "villagerStat";
        else if (role == "Investigator") fileName = "villagerStat";
        else if (role == "Troublemaker") fileName = "villagerStat";
        else if (role == "Seer") fileName = "villagerStat";
        else if (role == "Copycat") fileName = "villagerStat";

        if (result == "Win")
        {
            fileName = fileName + "Win";
        }
        else if (result == "Lose")
        {
            fileName = fileName + "Lose";
        }

        if (PlayerPrefs.HasKey(fileName))
        {
            PlayerPrefs.SetInt(fileName, PlayerPrefs.GetInt(fileName) + 1);
        }
        else
        {
            PlayerPrefs.SetInt(fileName, 1);
        }
        PlayerPrefs.Save();
    }

    static public string[] readScore()
    {
        //Number of wins
        int _villagerStatWin = PlayerPrefs.GetInt("villagerStatWin", 0);
        int _werewolfStatWin = PlayerPrefs.GetInt("werewolfStatWin", 0);
        int _othersStatWin = PlayerPrefs.GetInt("othersStatWin", 0);
        //Number of losts
        int _villagerStatLose = PlayerPrefs.GetInt("villagerStatLose", 0);
        int _werewolfStatLose = PlayerPrefs.GetInt("werewolfStatLose", 0);
        int _othersStatLose = PlayerPrefs.GetInt("othersStatLose", 0);
        int _totalWin = _villagerStatWin + _werewolfStatWin + _othersStatWin;
        int _total = _totalWin + _villagerStatLose + _werewolfStatLose + _othersStatLose;

        string[] result = new string[4];
        if (_villagerStatWin + _villagerStatLose == 0)
        {
            result[0] = "-";
        }
        else
        {
            result[0] = ((double)_villagerStatWin / (double)_villagerStatWin + (double)_villagerStatLose).ToString() + "%";
        }

        if (_werewolfStatWin + _werewolfStatLose == 0)
        {
            result[1] = "-";
        }
        else
        {
            result[1] = ((double)_werewolfStatWin / (double)_werewolfStatWin + (double)_werewolfStatLose).ToString() + "%";
        }

        if (_othersStatWin + _othersStatLose == 0)
        {
            result[2] = "-";
        }
        else
        {
            result[2] = ((double)_othersStatWin / (double)_othersStatWin + (double)_othersStatLose).ToString() + "%";
        }

        if(_total == 0)
        {
            result[3] = "Total: -";
        }
        else
        {
            result[3] = "Total: " + ((double)_totalWin / (double)_total).ToString() + "%";
        }

        return result;
    }
}
