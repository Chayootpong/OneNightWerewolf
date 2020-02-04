using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientUI : MonoBehaviour {

    //SHOW CHAT
    public RectTransform chat, order;
    public GameObject blur, reveal;
    int num;

    //SHOW ACTION
    public GameObject maxBlur;
    public GameObject[] actionSlot;

    //SHOW RESULT
    public static string result;
    public GameObject[] showResult;
    public Sprite[] portraitSlot;
    public Image portrait;

    //BLOCK CASE
    bool isDoppelStart, isCopycatStart;
    bool isDoubleEffect, isEnd;
    public static int numCard;

    //SET WEREWOLF
    public GameObject[] fangs;
    int werewolfCount = 10;

    //SOUND
    public AudioSource soundbox;
    public AudioClip[] soundclip;
    public AudioClip[] resSound;
    public AudioClip flip;

	void Start () {

        num = 0;
        result = "";
        isDoppelStart = isCopycatStart = isDoubleEffect = isEnd = false;
	}
	
	void Update () {

        if (num == 0)
        {
            blur.SetActive(false);
            chat.anchoredPosition = Vector3.Lerp(chat.anchoredPosition, new Vector3(500, 15, 0), Time.deltaTime * 10);
            order.anchoredPosition = Vector3.Lerp(order.anchoredPosition, new Vector3(0, 285, 0), Time.deltaTime * 10);
        }
        else if (num == 1)
        {
            blur.SetActive(true);
            chat.anchoredPosition = Vector3.Lerp(chat.anchoredPosition, new Vector3(298, 15, 0), Time.deltaTime * 10);
        }
        else if (num == 2)
        {
            blur.SetActive(true);
            order.anchoredPosition = Vector3.Lerp(order.anchoredPosition, new Vector3(0, 195, 0), Time.deltaTime * 10);
        }

        if (result != "" && !isEnd)
        {
            isEnd = true;

            portrait.sprite = portraitSlot[Client.voteTarget];

            StartCoroutine("ShowResult1");
            StartCoroutine("ShowResult2");
            StartCoroutine("ShowResult3");
            StartCoroutine("ShowResult4");
            StartCoroutine("ShowResult5");
        }
    }

    public void LerpMove(int i)
    {
        num = i;
    }

    public void ShowAction(string role)
    {
        maxBlur.SetActive(true);

        if (role == "Usurpator") actionSlot[0].SetActive(true);
        else if (role == "Doppleganger") actionSlot[1].SetActive(true);
        else if (role == "Alpha") actionSlot[2].SetActive(true);
        else if (role == "Mystic") actionSlot[3].SetActive(true);
        else if (role == "Witch") actionSlot[4].SetActive(true);
        else if (role == "Werewolf" && werewolfCount <= 1) actionSlot[5].SetActive(true);
        else if (role == "Repose") actionSlot[6].SetActive(true);
        else if (role == "Investigator") actionSlot[7].SetActive(true);
        else if (role == "Troublemaker") actionSlot[8].SetActive(true);
        else if (role == "Seer")
        {
            actionSlot[9].SetActive(true);
            actionSlot[10].SetActive(true);
        }
        else if (role == "Copycat") actionSlot[11].SetActive(true);
        else maxBlur.SetActive(false);
    }

    public void ShowReveal(string role)
    {
        if (role == "Usurpator") Reveal.index = 0;
        else if (role == "Minion") Reveal.index = 1;
        else if (role == "Doppleganger") Reveal.index = 2;
        else if (role == "Alpha") Reveal.index = 3;
        else if (role == "Mystic") Reveal.index = 4;
        else if (role == "Witch") Reveal.index = 5;
        else if (role == "Apocalypse") Reveal.index = 6;
        else if (role == "Dream Wolf") Reveal.index = 7;
        else if (role == "Werewolf") Reveal.index = 8;
        else if (role == "Repose") Reveal.index = 9;
        else if (role == "Villager") Reveal.index = 10;
        else if (role == "Investigator") Reveal.index = 11;
        else if (role == "Troublemaker") Reveal.index = 12;
        else if (role == "Seer") Reveal.index = 13;
        else if (role == "Copycat") Reveal.index = 14;

        reveal.SetActive(true);

        //FOR COPYCAT
        if (Client.role == "Copycat")
        {
            if (isCopycatStart)
            {
                Client.role = role;
                GetComponent<ClientGameController>().ChangeRole(Client.id, role, "Copycat");
            }
            else isCopycatStart = true;
        }
        //FOR DOPPLEGANGER
        else if (Client.role == "Doppleganger")
        {
            if (isDoppelStart)
            {
                Client.role = role;
                GetComponent<ClientGameController>().ChangeRole(Client.id, role, "Doppleganger");
            }
            else isDoppelStart = true;
        }
        //FOR WITCH
        else if (Client.role == "Witch")
        {
            ClientGameController.action = "Exchange";
        }
        //FOR INVESTIGATOR
        else if (Client.role == "Investigator")
        {
            if (role == "Werewolf" || role == "Alpha" || role == "Mystic")
            {
                GetComponent<ClientGameController>().ChangeRole(Client.id, "Werewolf", Client.role);
            }
            else if (role == "Tanner")
            {
                GetComponent<ClientGameController>().ChangeRole(Client.id, "Tanner", Client.role);
            }
            else if (!isDoubleEffect && GetComponent<Timer>().beforeNight <= 0)
            {
                ClientGameController.action = "Reveal";
                isDoubleEffect = true;
            }
        }
        //FOR SEER
        else if (Client.role == "Seer")
        {
            if (numCard == 2 && !isDoubleEffect)
            {
                ClientGameController.action = "Reveal";
                isDoubleEffect = true;
            }
        }
    }

    public void SetFangWerewolf(string[] list)
    {
        if (Client.role == "Werewolf" || Client.role == "Alpha" || Client.role == "Mystic" || Client.role == "Minion")
        {
            for (int i = 0; i < 8; i++) //MANUAL SET (5 PLAYERS)
            {
                if (list[i] == "1") fangs[i].SetActive(true);
                else fangs[i].SetActive(false);
            }
            werewolfCount = int.Parse(list[8]);
        }
    }

    IEnumerator ShowResult1()
    {
        yield return new WaitForSeconds(3f);
        reveal.GetComponent<Reveal>().CloseReveal();
        showResult[5].SetActive(true);
        showResult[6].SetActive(true);
        showResult[7].SetActive(true);
    }

    IEnumerator ShowResult2()
    {
        yield return new WaitForSeconds(6f);
        showResult[5].SetActive(false);
        showResult[6].SetActive(false);
        showResult[7].SetActive(false);
        GetComponent<ClientGameController>().Reveal(Client.voteTarget);
    }

    IEnumerator ShowResult3()
    {
        yield return new WaitForSeconds(9f);
        reveal.GetComponent<Reveal>().CloseReveal();
    }

    IEnumerator ShowResult4()
    {
        yield return new WaitForSeconds(10f);
        showResult[4].SetActive(true);
    }

    IEnumerator ShowResult5()
    {
        yield return new WaitForSeconds(11f);
        if (result == "Win")
        {
            showResult[0].SetActive(true);
            showResult[2].SetActive(true);
            
            if (!soundbox.isPlaying)
            {
                soundbox.clip = resSound[0];
                soundbox.Play();
            }
        }
        else if (result == "Lose")
        {
            showResult[1].SetActive(true);
            showResult[3].SetActive(true);

            if (!soundbox.isPlaying)
            {
                soundbox.clip = resSound[1];
                soundbox.Play();
            }
        }

        //stat collection
        Stats.SetScore(Client.role, result);
    }
}
