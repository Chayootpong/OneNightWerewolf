using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour {

    public static int index;
    public GameObject[] card;
    GameObject spawn;

    void OnEnable()
    {
        spawn = Instantiate(card[index], new Vector3(0, -7, 7), transform.rotation);
    }

    void Start () {       
		
	}
	
	void Update () {
		
	}

    public void CloseReveal()
    {
        Destroy(spawn);
        gameObject.SetActive(false);
    }
}
