using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsVisual : MonoBehaviour {
    public Text villagerStat;
    public Text werewolfStat;
    public Text othersStat;
    public Text totalStat;

    // Use this for initialization
    void Start () {
        string[] score = Stats.readScore();
        villagerStat.text = score[0];
        werewolfStat.text = score[1];
        othersStat.text = score[2];
        totalStat.text = score[3];
    }
}
