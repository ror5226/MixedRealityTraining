using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ReadText : Singleton<ReadText> {

    public TextToSpeechManager textToSpeechManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Uses Text to Speech Manager to say whatever string is passed in
    public void Say(string s)
    {
        if(textToSpeechManager != null)
        {
            textToSpeechManager.SpeakText(s);
        }
    }
}
