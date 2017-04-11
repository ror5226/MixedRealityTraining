using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ReadText : Singleton<ReadText> {

    public TextToSpeechManager textToSpeechManager;
    AccessPanel a;

    // Use this for initialization
    void Start () {
        if (AccessPanel.Instance == null) {
            Debug.Log("No AccessPanel Instance");
        }
        else {
            a = AccessPanel.Instance;
        }
    }

    // Uses Text to Speech Manager to say whatever is in the current open information panel
    public void Say()
    {
        if(textToSpeechManager != null)
        {
            if (!textToSpeechManager.IsSpeaking())
                textToSpeechManager.SpeakText(a.getSpeechText());
            else
                textToSpeechManager.StopSpeaking();
        }
    }

    public void SayQuestion() {
        if (textToSpeechManager != null) {
            if (!textToSpeechManager.IsSpeaking())
                textToSpeechManager.SpeakText(a.getQuestionSpeech());
            else
                textToSpeechManager.StopSpeaking();
        }
    }

    // Uses Text to Speech Manager to say whatever string is passed in
    public void Say(string s)
    {
        if (textToSpeechManager != null)
        {
            if (!textToSpeechManager.IsSpeaking())
                textToSpeechManager.SpeakText(s);
            else
                textToSpeechManager.StopSpeaking();
        }
    }
}
