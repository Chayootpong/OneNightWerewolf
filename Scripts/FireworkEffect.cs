using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkEffect : MonoBehaviour {

    public GameObject eff;
    GameObject spawn, gamemng;

    AudioSource soundbox, soundrole;
    public AudioClip magic;

    void Start () {

        soundbox = GetComponent<AudioSource>();
        soundrole = GetComponent<AudioSource>();
        gamemng = GameObject.Find("Game Manager");
		
	}
	
	void Update () {
		
	}

    public void SpawnFirework()
    {
        spawn = Instantiate(eff, new Vector3(0, 0, 20), transform.rotation);
        Invoke("DestroyEffect", 1f);
        soundbox.clip = magic;
        soundbox.Play();
        soundrole.clip = gamemng.GetComponent<ClientUI>().soundclip[Reveal.index];
        soundrole.Play();
    }

    public void DestroyEffect()
    {
        Destroy(spawn);
    }
}
