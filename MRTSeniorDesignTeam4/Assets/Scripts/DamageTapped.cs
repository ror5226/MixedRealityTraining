using UnityEngine;
using System.Collections;
using System;

public class DamageTapped : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSelect() {
        GameObject.Find("DamageInfo").transform.FindChild("DamageInfo").gameObject.SetActive(true);
    }

}
